#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Engine.BaseTypes;
using Microsoft.Xna.Framework;
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
        result.SetBackgroundColor((Color)(CompileValue(jsonScene.SelectToken("BackgroundColor")) ?? Color.White));
        var gameObjects = jsonScene.SelectToken("GameObjects");
        foreach (var gameObject in (jsonScene.SelectToken("Hierarchy")?.Values<string>() ?? new List<string>())
                 .Select(objectName => result.AddGameObject(new GameObject(objectName))))
        {
            var components = gameObjects?.SelectToken(gameObject.Name);
            if (components == null) continue;
            foreach (var component in components)
            {
                Type? kind;
                object? instance;
                if (((JProperty)component).Name != "GameObject")
                {
                    kind = engineAssembly.GetType(((JProperty)component).Name)
                           ?? gameAssembly.GetType(((JProperty)component).Name);
                    if (kind == null || !kind.IsSubclassOf(typeof(Component)))
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
                    var value = CompileValue(parameter.First);
                    var property = kind.GetProperty(((JProperty)parameter).Name);
                    if (property == null)
                    {
                        var method = kind.GetMethod($"Set{((JProperty)parameter).Name}")
                                     ?? kind.GetMethod(((JProperty)parameter).Name);
                        if (method == null)
                            throw new Exception($"Cannot find property {((JProperty)parameter).Name} in {kind}");
                        if (value?.GetType() != method.GetParameters()[0].ParameterType && value != null)
                            value = value is not Vector4 vector4
                                ? Convert.ChangeType(value, method.GetParameters()[0].ParameterType)
                                : RemakeVector(method.GetParameters()[0].ParameterType, vector4);
                        method.Invoke(instance, new[] { value });
                    }
                    else
                    {
                        if (value?.GetType() != property.PropertyType && value != null)
                            value = value is not Vector4 vector4
                                ? Convert.ChangeType(value, property.PropertyType)
                                : RemakeVector(property.PropertyType, vector4);
                        property.SetValue(instance, value);
                    }
                }
            }
        }

        return result;
    }

    private static object? CompileString(object? value) =>
        value?.ToString() switch
        {
            "@lsb" => Game.LoadingScreenBackground,
            "@centerX" => Game.ViewportCenter.X,
            "@centerY" => Game.ViewportCenter.Y,
            "@sizeX" => Game.ViewportSize.X,
            "@sizeY" => Game.ViewportSize.Y,
            "@resolution" => Game.ResolutionCoefficient,
            _ => float.TryParse(value?.ToString(), out var valF) ? valF :
                bool.TryParse(value?.ToString(), out var valB) ? valB : value
        };

    private static object? CompileValue(object? token) =>
        token == null
            ? default
            : token.GetType() == typeof(JValue)
                ? CompileString((token as JValue)?.Value)
                : token is string
                    ? CompileString(token)
                    : token is JToken jToken
                        ? ((JProperty?)jToken.First)?.Name switch
                        {
                            "@Vector" => GetVectorValue(((JProperty?)jToken.First)?.Value),
                            "@Color" => GetColorValue(((JProperty?)jToken.First)?.Value.ToString()),
                            "@Asset" => GetAssetValue(((JProperty?)jToken.First)?.Value),
                            "@Multiply" => GetMultipliedValue(((JProperty?)jToken.First)?.Value as JArray),
                            "@Add" => GetAddedValue(((JProperty?)jToken.First)?.Value as JArray),
                            "@PlayerSettings" => GetPlayerSettings(((JProperty?)jToken.First)?.Value),
                            "@None" => null,
                            _ => throw new Exception($"Unexpected token type {((JProperty?)jToken.First)?.Name}")
                        }
                        : default;

    private static Vector4 GetVectorValue(JToken? value)
    {
        if (value == null) throw new NullReferenceException("Not correct vector value");
        var x = CompileValue(value.SelectToken("X"));
        var y = CompileValue(value.SelectToken("Y"));
        var z = CompileValue(value.SelectToken("Z"));
        var w = CompileValue(value.SelectToken("W"));
        return new((float)(x ?? 0f), (float)(y ?? 0f), (float)(z ?? 0f), (float)(w ?? 0f));
    }

    private static dynamic? GetPlayerSettings(JToken? value)
    {
        if (value == null) throw new NullReferenceException("Not correct player settings value");
        var val = CompileValue(value.SelectToken("value"));
        var def = CompileValue(value.SelectToken("default"));
        return PlayerSettings.GetValue(val?.ToString()) ?? def;
    }

    private static dynamic GetMultipliedValue(JArray? values)
    {
        if (values == null) throw new NullReferenceException("Not correct values to multiply");
        return values.Select(value => CompileValue(value) ?? 1).Aggregate<dynamic, dynamic>(1f, (current, val) =>
            current * (float.TryParse(val.ToString(), out float floatValue) ? floatValue : val));
    }

    private static dynamic? GetAddedValue(JArray? values) =>
        values?
            .Select(value => CompileValue(value) ?? 1)
            .Aggregate<dynamic, dynamic?>(null, (current, val) =>
            current != null
                ? current + (float.TryParse(val.ToString(), out float floatValue) ? floatValue : val)
                : float.TryParse(val.ToString(), out floatValue)
                    ? floatValue
                    : val);

    private static object? GetAssetValue(JToken? value)
    {
        var asset = value?.SelectToken("asset")?.Value<string>();
        var type = value?.SelectToken("type")?.Value<string>();
        if (asset == null || type == null)
            throw new Exception($"Not correct asset information, asset: {asset}, type: {type}");
        switch (asset)
        {
            case "Content":
                var actualType = typeof(Microsoft.Xna.Framework.Game)
                    .Assembly
                    .ExportedTypes
                    .First(t => t.Name == type);
                var filename = value?.SelectToken("filename")?.Value<string>();
                if (filename == null)
                    throw new Exception("No filename for the asset");
                return Convert.ChangeType(EngineContent.LoadContent<object>(filename), actualType);
            case "SVG":
                var assetName = value?.SelectToken("assetName")?.Value<string>();
                var size = (Vector2)RemakeVector(typeof(Vector2),
                    (Vector4)(CompileValue(value?.SelectToken("size")) ?? Vector4.Zero));
                return type switch
                {
                    "Texture" => EngineContent.LoadSvgTexture(assetName, size),
                    "Animation" => EngineContent.LoadSvgAnimation(assetName, size),
                    _ => throw new Exception($"Unknown asset type {type}")
                };
            default:
                throw new Exception($"Unknown asset case {asset}");
        }
    }

    private static Color GetColorValue(string? value)
    {
        if (value == null) throw new NullReferenceException("Not correct color value");
        var clrColor = System.Drawing.Color.FromName(value);
        return new Color(clrColor.R, clrColor.G, clrColor.B, clrColor.A);
    }

    private static object RemakeVector(Type type, Vector4 vector4)
    {
        if (type == typeof(Vector2))
            return new Vector2(vector4.X, vector4.Y);
        if (type == typeof(Vector3))
            return new Vector3(vector4.X, vector4.Y, vector4.Z);
        throw new Exception("Not correct vector");
    }
}
