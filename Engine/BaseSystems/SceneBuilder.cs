#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using Engine.BaseTypes;
using Microsoft.Xna.Framework;
using Newtonsoft.Json.Linq;

namespace Engine.BaseSystems;

internal static class SceneBuilder
{
    public static Scene BuildScene(string json)
    {
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
                var kind = Type.GetType(((JProperty)component).Name);
                if (kind?.BaseType != typeof(Component)) continue;
                var instance = gameObject.AddComponent(kind);
                if (component.First == null) continue;
                foreach (var parameter in component.First.Children())
                {
                    var property = kind.GetProperty(((JProperty)parameter).Name);
                    if (property == null)
                        throw new Exception($"Cannot find property {((JProperty)parameter).Name} in {kind}");
                    var value = CompileValue(parameter.First);
                    if (value?.GetType() != property.PropertyType && value != null)
                        value = Convert.ChangeType(value, property.PropertyType);
                    property.SetValue(instance, value);
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
            _ => value
        };

    private static object? CompileValue(JToken? token) =>
        token == null
            ? default
            : token.GetType() == typeof(JValue)
                ? CompileString((token as JValue)?.Value)
                : ((JProperty?)token.First)?.Name switch
                {
                    "@Vector" => GetVectorValue(token.First as JProperty),
                    "@Color" => GetColorValue(((JProperty?)token.First)?.Value.ToString()),
                    "@FloatValue" => GetFloatValue(((JProperty?)token.First)?.Value),
                    "@Asset" => GetAssetValue(((JProperty?)token.First)?.Value),
                    "@BoolValue" => GetBoolValue(((JProperty?)token.First)?.Value),
                    "@TextValue" => GetTextValue(((JProperty?)token.First)?.Value),
                    "@SpecValue" => CompileValue(((JProperty?)token.First)?.Value),
                    "@Multiply" => GetMultipliedValue(((JProperty?)token.First)?.Value as JArray),
                    "@None" => null,
                    _ => throw new Exception($"Unexpected token type {((JProperty?)token.First)?.Name}")
                };

    private static Vector2 GetVectorValue(JProperty? value)
    {
        if (value == null) return default;
        var x = CompileValue(value.Value.SelectToken("X"));
        var y = CompileValue(value.Value.SelectToken("Y"));
        return new Vector2((float)(x ?? 0), (float)(y ?? 0));
    }

    private static float GetMultipliedValue(JArray? values) =>
        values?.Aggregate(1f, (current, value) =>
            current * float.Parse(CompileString(value)?.ToString() ?? string.Empty)) ?? default;

    private static float GetFloatValue(JToken? value)
    {
        if (value == null) return default;
        var resultValue = CompileValue(value);
        return !string.IsNullOrEmpty(resultValue?.ToString())
            ? float.Parse(resultValue as string ?? string.Empty)
            : default;
    }

    private static bool GetBoolValue(JToken? value)
    {
        if (value == null) return default;
        var resultValue = CompileValue(value);
        return !string.IsNullOrEmpty(resultValue?.ToString())
            ? bool.Parse(resultValue as string ?? string.Empty)
            : default;
    }

    private static string? GetTextValue(JToken? value)
    {
        if (value == null) return default;
        var resultValue = CompileValue(value);
        return !string.IsNullOrEmpty(resultValue?.ToString()) ? resultValue as string ?? string.Empty : default;
    }


    private static object? GetAssetValue(JToken? value)
    {
        var asset = value?.SelectToken("asset")?.Value<string>();
        var type = value?.SelectToken("type")?.Value<string>();
        if (asset == null || type == null)
            return default;
        switch (asset)
        {
            case "Content":
                var actualType = typeof(Microsoft.Xna.Framework.Game)
                    .Assembly
                    .ExportedTypes
                    .First(t => t.Name == type);
                var filename = value?.SelectToken("filename")?.Value<string>();
                return filename != null
                    ? Convert.ChangeType(EngineContent.LoadContent<object>(filename), actualType)
                    : default;
            case "SVG":
                var assetName = value?.SelectToken("assetName")?.Value<string>();
                var size = (Vector2)(CompileValue(value?.SelectToken("size")) ?? Vector2.Zero);
                return type switch
                {
                    "Texture" => EngineContent.LoadSvgTexture(assetName, size),
                    "Animation" => EngineContent.LoadSvgAnimation(assetName, size),
                    _ => default
                };
            default:
                return default;
        }
    }

    private static Color GetColorValue(string? value)
    {
        if (value == null) return default;
        var clrColor = System.Drawing.Color.FromName(value);
        return new Color(clrColor.R, clrColor.G, clrColor.B, clrColor.A);
    }
}
