#nullable enable
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Engine.BaseTypes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json.Linq;

namespace Engine.BaseSystems;

internal static class SceneBuilder
{
    public static Scene BuildScene(string json)
    {
        var engineAssembly = Assembly.GetAssembly(typeof(Game));
        var gameAssembly = Assembly.GetEntryAssembly();
        if (engineAssembly == null || gameAssembly == null)
            throw new Exception("Cannot load assembly");
        var jsonScene = JObject.Parse(
            new string(json.Where(x => (!char.IsControl(x) || x == '\n') && char.IsAscii(x)).ToArray()));
        var result = new Scene(jsonScene.SelectToken("Name")?.Value<string>() ?? "BTempScene");
        result.SetBackgroundColor((Color)(
            CompileValue(jsonScene.SelectToken("BackgroundColor"), typeof(Color)) ?? Color.White));
        var gameObjects = jsonScene.SelectToken("GameObjects");
        foreach (var gameObject in (gameObjects ?? new JArray())
                 .Select(objectName => result.AddGameObject(new GameObject(((JProperty)objectName).Name))))
        {
            var components = gameObjects?.SelectToken(gameObject.Name);
            if (components == null) continue;
            foreach (var component in components)
            {
                Type? kind;
                object? instance;
                if (((JProperty)component).Name != "GameObject")
                {
                    kind = engineAssembly.GetTypes()
                        .Concat(gameAssembly.GetTypes())
                        .Where((type, _) => (type.Name == ((JProperty)component).Name ||
                                             type.FullName == ((JProperty)component).Name) &&
                                            type.IsSubclassOf(typeof(Component)))
                        .FirstOrDefault();
                    if (kind == null)
                        throw new Exception($"Not correct component type {((JProperty)component).Name}");
                    instance = gameObject.AddComponent(kind);
                }
                else
                {
                    kind = typeof(GameObject);
                    instance = gameObject;
                }

                if (component.First == null) continue;
                foreach (var parameter in component.First.Children())
                {
                    var property = kind.GetProperty(((JProperty)parameter).Name);
                    if (property == null)
                    {
                        var method = kind.GetMethod($"Set{((JProperty)parameter).Name}")
                                     ?? kind.GetMethod(((JProperty)parameter).Name);
                        if (method == null)
                            throw new Exception($"Cannot find property {((JProperty)parameter).Name} in {kind}");
                        var value = CompileValue(parameter.First, method.GetParameters()[0].ParameterType);
                        if (value == null || value.GetType() == method.GetParameters()[0].ParameterType)
                            method.Invoke(instance, new[] { value });
                        else
                            throw new Exception(
                                $"Not correct parameter type was returned. Value = {value}, wanted type: {method.GetParameters()[0].ParameterType}");
                    }
                    else
                    {
                        var value = CompileValue(parameter.First, property.PropertyType);
                        if (value == null || value.GetType() == property.PropertyType)
                            property.SetValue(instance, property.PropertyType.IsValueType ? value ?? default : value);
                        else
                            throw new Exception(
                                $"Not correct property type was returned. Value = {value}, wanted type: {property.PropertyType}");
                    }
                }
            }
        }

        return result;
    }

    private static bool IsNumericType(Type type) =>
        Type.GetTypeCode(type) switch
        {
            TypeCode.Byte => true,
            TypeCode.SByte => true,
            TypeCode.UInt16 => true,
            TypeCode.UInt32 => true,
            TypeCode.UInt64 => true,
            TypeCode.Int16 => true,
            TypeCode.Int32 => true,
            TypeCode.Int64 => true,
            TypeCode.Decimal => true,
            TypeCode.Double => true,
            TypeCode.Single => true,
            _ => false
        };

    private struct Operator
    {
        public Func<float, float, float> Evaluate { get; }
        public readonly int Precedence;
        public readonly int Association;

        public Operator(int precedence, int association, Func<float, float, float> evaluate)
        {
            Precedence = precedence;
            Association = association;
            Evaluate = evaluate;
        }
    }

    private static float Calculator(string? value)
    {
        if (value == null)
            return default;

        var operators = new Dictionary<string, Operator>
            {
                { "+", new Operator(2, -1, (a, b) => a + b) },
                { "-", new Operator(2, -1, (a, b) => a - b) },
                { "*", new Operator(3, -1, (a, b) => a * b) },
                { "/", new Operator(3, -1, (a, b) => a / b) },
                { "^", new Operator(4, 1, MathF.Pow) }
            };
        var output = new Stack<object>();
        var operatorsStack = new Stack<string>();

        new Regex(@"(\b\w*[\.]?\w+\b|[\(\)\+\*\-\/\^])")
            .Matches(value).Select(match => match.Value).ToList().ForEach(token =>
            {
                if (operators.TryGetValue(token, out var o))
                {
                    while (operatorsStack.Count > 0)
                    {
                        var ot = operatorsStack.Peek();
                        if (operators.TryGetValue(ot, out var op) && (
                                (o.Association == -1 && o.Precedence <= op.Precedence) ||
                                (o.Association == 1 && o.Precedence < op.Precedence))
                           )
                            output.Push(operatorsStack.Pop());
                        else
                            break;
                    }
                    operatorsStack.Push(token);
                }
                else switch (token)
                {
                    case "(":
                        operatorsStack.Push("(");
                        break;
                    case ")":
                    {
                        var parenthesesMatch = false;
                        while (operatorsStack.Count > 0)
                        {
                            if (operatorsStack.Peek() == "(")
                            {
                                parenthesesMatch = true;
                                break;
                            }
                            output.Push(operatorsStack.Pop());
                        }
                        if (!parenthesesMatch) throw new Exception($"Sorry, parentheses don't match: {value}");
                        operatorsStack.Pop();
                        break;
                    }
                    default:
                        if (float.TryParse(token,
                                NumberStyles.Float | NumberStyles.AllowThousands,
                                CultureInfo.InvariantCulture,
                                out var f))
                            output.Push(f);
                        else switch (token)
                        {
                            case "x":
                                output.Push(Game.ViewportSize.X);
                                break;
                            case "y":
                                output.Push(Game.ViewportSize.Y);
                                break;
                            case "r":
                                output.Push(Game.ResolutionCoefficient);
                                break;
                            default:
                                throw new Exception($"Unknown token {token}");
                        }
                        break;
                }
            });

        while (operatorsStack.Count > 0)
            output.Push(operatorsStack.Pop());

        var result = new Queue<object>(output.Reverse());
        var vars = new Stack<float>();

        while (result.Count > 0)
        {
            var @object = result.Dequeue();
            switch (@object)
            {
                case float f:
                    vars.Push(f);
                    break;
                case string op:
                {
                    var right = vars.Pop();
                    var left = vars.Pop();
                    vars.Push(operators.TryGetValue(op, out var actualOperator)
                        ? actualOperator.Evaluate(left, right)
                        : throw new Exception($"Not correct operator {op}"));
                    break;
                }
            }
        }

        return vars.Peek();
    }

    private static object? CompileValue(object? token, Type resultType) =>
        token switch
        {
            null or JValue { Value: null } => default,
            JValue { Value: true or false } value => value.Value,
            JValue jValue => CompileValue(jValue.Value, resultType),
            string => Convert.ChangeType(resultType == typeof(string)
                ? token
                : IsNumericType(resultType) && (token = (token.ToString() ?? string.Empty).Replace(" ", "")) != null
                    ? Calculator(token.ToString())
                    : resultType == typeof(Color)
                        ? GetColorValue(token)
                        : token?.ToString() == "@lsb"
                            ? Game.LoadingScreenBackground
                            : float.TryParse(token?.ToString(),
                                NumberStyles.Float | NumberStyles.AllowThousands,
                                CultureInfo.InvariantCulture,
                                out var valF)
                                ? valF
                                : bool.TryParse(token?.ToString(), out var valB)
                                    ? valB
                                    : throw new Exception($"Not understandable string value: {token}"),
                resultType),
            JArray jArray => resultType == typeof(Vector4) ||
                             resultType == typeof(Vector3) ||
                             resultType == typeof(Vector2)
                ? RemakeVector(resultType, GetVectorValue(jArray))
                : resultType == typeof(Color)
                    ? GetColorValue(token)
                    : throw new Exception($"Unexpected token type {resultType} : {token}"),
            JToken jToken => ((JProperty?)jToken.First)?.Name switch
            {
                "Asset" => GetAssetValue(((JProperty?)jToken.First)?.Value, resultType),
                "PlayerSettings" => GetPlayerSettings(((JProperty?)jToken.First)?.Value, resultType),
                _ => throw new Exception($"Unexpected token type {((JProperty?)jToken.First)?.Name}")
            },
            _ =>
                IsNumericType(resultType) && IsNumericType(token.GetType())
                    ? Convert.ChangeType(token, resultType)
                    : default
        };


    private static Vector4 GetVectorValue(JArray value) =>
        new(
            (float)((value.Count > 0
                ? float.TryParse(value[0].ToString(),
                    NumberStyles.Float | NumberStyles.AllowThousands,
                    CultureInfo.InvariantCulture, out var x) ? x : CompileValue(value[0].ToString(), typeof(float))
                : 0f) ?? 0f),
            (float)((value.Count > 1
                ? float.TryParse(value[1].ToString(),
                    NumberStyles.Float | NumberStyles.AllowThousands,
                    CultureInfo.InvariantCulture, out var y) ? y : CompileValue(value[1].ToString(), typeof(float))
                : 0f) ?? 0f),
            (float)((value.Count > 2
                ? float.TryParse(value[2].ToString(),
                    NumberStyles.Float | NumberStyles.AllowThousands,
                    CultureInfo.InvariantCulture, out var z) ? z : CompileValue(value[2].ToString(), typeof(float))
                : 0f) ?? 0f),
            (float)((value.Count > 3
                ? float.TryParse(value[3].ToString(),
                    NumberStyles.Float | NumberStyles.AllowThousands,
                    CultureInfo.InvariantCulture, out var w) ? w : CompileValue(value[3].ToString(), typeof(float))
                : 0f) ?? 0f)
        );

    private static object? GetPlayerSettings(JToken? value, Type resultType)
    {
        if (value == null) throw new NullReferenceException("Not correct player settings value");
        var val = CompileValue(value.SelectToken("value"), typeof(string));
        var def = CompileValue(value.SelectToken("default"), resultType);
        return Convert.ChangeType(PlayerSettings.GetValue(val?.ToString())?? def, resultType);
    }

    private static object? GetAssetValue(JToken? value, Type? resultType)
    {
        if (resultType == null)
            throw new Exception("Asset should be imported with a known type, but was null. Asset info: {value}");
        var asset = value?.SelectToken("asset")?.Value<string>();
        if (asset == null)
            throw new Exception($"Not correct asset information, asset: {asset}");
        switch (asset)
        {
            case "Content":
                var filename = value?.SelectToken("filename")?.Value<string>();
                if (filename == null)
                    throw new Exception("No filename for the asset");
                return Convert.ChangeType(EngineContent.LoadContent<object>(filename), resultType);
            case "SVG":
                var assetName = value?.SelectToken("assetName")?.Value<string>();
                var size = (Vector2)(CompileValue(value?.SelectToken("size"), typeof(Vector2)) ?? Vector2.Zero);
                if (resultType == typeof(Texture2D))
                    return EngineContent.LoadSvgTexture(assetName, size);
                if (resultType == typeof(AnimationInformation))
                    return EngineContent.LoadSvgAnimation(assetName, size);
                throw new Exception($"Not correct SVG asset type {resultType}");
            default:
                throw new Exception($"Unknown asset case {asset}");
        }
    }

    private static Color GetColorValue(object? value)
    {
        float r, g, b, a;
        if (value?.ToString() == null) throw new NullReferenceException("Not correct color value");
        if (value is JArray jArray)
            if (jArray.Count == 4 &&
                (r = (float)(CompileValue(jArray[0], typeof(float)) ?? 0f)) is >= 0 and <= 1 &&
                (g = (float)(CompileValue(jArray[1], typeof(float)) ?? 0f)) is >= 0 and <= 1 &&
                (b = (float)(CompileValue(jArray[2], typeof(float)) ?? 0f)) is >= 0 and <= 1 &&
                (a = (float)(CompileValue(jArray[3], typeof(float)) ?? 0f)) is >= 0 and <= 1)
                return new Color(r, g, b, a);
            else throw new Exception($"Not correct color value {jArray}");
        var clrColor = System.Drawing.Color.FromName(value.ToString() ?? string.Empty);
        return new Color(clrColor.R, clrColor.G, clrColor.B, clrColor.A);
    }

    private static object RemakeVector(Type? type, Vector4 vector4)
    {
        if (type == typeof(Vector2))
            return new Vector2(vector4.X, vector4.Y);
        if (type == typeof(Vector3))
            return new Vector3(vector4.X, vector4.Y, vector4.Z);
        if (type == typeof(Vector4))
            return vector4;
        throw new Exception("Not correct vector");
    }
}
