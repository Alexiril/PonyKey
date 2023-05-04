using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.BaseSystems;

public static class TextureGenerator
{
    public static Texture2D GenerateTexture(int width, int height, Func<int, int, Color> paint)
    {
        var texture = new Texture2D(Game.GraphicsDevice, width, height);
        var data = new Color[width * height];
        for (var pixel = 0; pixel < data.Length; pixel++)
            data[pixel] = paint(pixel % width, pixel > width ? (int)MathF.Ceiling((float)pixel / width) : 0);
        texture.SetData(data);
        return texture;
    }
}
