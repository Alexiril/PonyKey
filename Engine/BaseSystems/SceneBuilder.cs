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
                var kind = engineAssembly.GetType(((JProperty)component).Name)
                           ?? gameAssembly.GetType(((JProperty)component).Name);
                if (kind == null || !kind.IsSubclassOf(typeof(Component)))
                    throw new Exception($"Not correct component type {((JProperty)component).Name}");
                var instance = gameObject.AddComponent(kind);
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
                            value = Convert.ChangeType(value, method.GetParameters()[0].ParameterType);
                        method.Invoke(instance, new[] { value });
                    }
                    else
                    {
                        if (value?.GetType() != property.PropertyType && value != null)
                            value = Convert.ChangeType(value, property.PropertyType);
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
            _ => value
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
                            "@FloatValue" => GetFloatValue(((JProperty?)jToken.First)?.Value),
                            "@Asset" => GetAssetValue(((JProperty?)jToken.First)?.Value),
                            "@BoolValue" => GetBoolValue(((JProperty?)jToken.First)?.Value),
                            "@TextValue" => GetTextValue(((JProperty?)jToken.First)?.Value),
                            "@SpecValue" => CompileValue(((JProperty?)jToken.First)?.Value),
                            "@Multiply" => GetMultipliedValue(((JProperty?)jToken.First)?.Value as JArray),
                            "@Add" => GetAddedValue(((JProperty?)jToken.First)?.Value as JArray),
                            "@PlayerSettings" => GetPlayerSettings(((JProperty?)jToken.First)?.Value),
                            "@None" => null,
                            _ => throw new Exception($"Unexpected token type {((JProperty?)jToken.First)?.Name}")
                        }
                        : default;

    private static object GetVectorValue(JToken? value)
    {
        if (value == null) throw new NullReferenceException("Not correct vector value");
        var x = CompileValue(value.SelectToken("X"));
        var y = CompileValue(value.SelectToken("Y"));
        var z = CompileValue(value.SelectToken("Z"));
        var w = CompileValue(value.SelectToken("W"));
        return z == null && w == null
            ? new Vector2((float)(x ?? 0), (float)(y ?? 0))
            : w == null && z != null
                ? new Vector3((float)(x ?? 0), (float)(y ?? 0), (float)z)
                : new Vector4((float)(x ?? 0), (float)(y ?? 0), (float)(z ?? 0), (float)(w ?? 0));
    }

    private static object? GetPlayerSettings(JToken? value)
    {
        if (value == null) throw new NullReferenceException("Not correct player settings value");
        var val = CompileString(value.SelectToken("value"));
        var def = CompileValue(value.SelectToken("default"));
        return PlayerSettings.GetValue(val?.ToString()) ?? def;
    }

    private static float GetMultipliedValue(JArray? values) =>
        values?.Aggregate(1f, (current, value) =>
            current * float.Parse(CompileValue(value)?.ToString() ?? "1")) ?? default;

    private static float GetAddedValue(JArray? values) =>
        values?.Aggregate(0f, (current, value) =>
            current + float.Parse(CompileValue(value)?.ToString() ?? "0")) ?? default;


    private static float GetFloatValue(JToken? value)
    {
        if (value == null) throw new NullReferenceException("Not correct float value");
        var resultValue = CompileValue(value);
        if (!string.IsNullOrEmpty(resultValue?.ToString()))
            return float.Parse(resultValue.ToString() ?? "0");
        throw new Exception($"Not correct float value: {resultValue}, {value}");
    }

    private static bool GetBoolValue(JToken? value)
    {
        if (value == null) throw new NullReferenceException("Not correct bool value");
        var resultValue = CompileValue(value);
        if (!string.IsNullOrEmpty(resultValue?.ToString()))
            return bool.Parse(resultValue.ToString() ?? "false");
        throw new Exception($"Not correct bool value: {resultValue}, {value}");
    }

    private static string? GetTextValue(JToken? value)
    {
        if (value == null) throw new NullReferenceException("Not correct text value");
        var resultValue = CompileValue(value);
        return !string.IsNullOrEmpty(resultValue?.ToString()) ? resultValue as string ?? string.Empty : default;
    }


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
                var size = (Vector2)(CompileValue(value?.SelectToken("size")) ?? Vector2.Zero);
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
}
