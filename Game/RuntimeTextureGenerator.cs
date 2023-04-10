using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game;

public static class RuntimeTextureGenerator
{
    public static Texture2D GenerateTexture(GraphicsDevice device, int width, int height, Func<int, Color> paint)
    {
        var texture = new Texture2D(device, width, height);
        var data = new Color[width * height];
        for (var pixel = 0; pixel < data.Length; pixel++) data[pixel] = paint(pixel);
        texture.SetData(data);
        return texture;
    }
}
