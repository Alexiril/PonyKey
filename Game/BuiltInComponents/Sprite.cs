
using Game.BaseTypes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#if DEBUG
using System.IO;
#endif

namespace Game.BuiltInComponents;

internal class Sprite : Component
{
    internal Color TextureColor { get; set; } = Color.White;

    internal Texture2D Texture
    {
        get => _texture;
        set
        {
            _texture = value;
#if DEBUG
            _haveSaved = false;
#endif
        }
    }

    internal SpriteEffects SpriteEffects { get; set; } = SpriteEffects.None;

    internal Sprite SetTexture(Texture2D texture)
    {
        Texture = texture;
        return this;
    }

    internal Sprite SetSpriteEffects(SpriteEffects effects)
    {
        SpriteEffects = effects;
        return this;
    }

    internal Sprite SetTextureColor(Color color)
    {
        TextureColor = color;
        return this;
    }

    internal int Width => Texture.Width;

    internal int Height => Texture.Height;

    internal Vector2 Size => new(Width, Height);

    internal float ResolutionCoefficient =>
        .5f * (Width / ActualGame.ViewportSize.X) + .5f * (Height / ActualGame.ViewportSize.Y);

    internal override void Draw()
    {
        if (Texture != null)
        {
#if DEBUG
            if (!_haveSaved)
            {
                if (!Directory.Exists("assetImageDebug")) Directory.CreateDirectory("assetImageDebug");
                var debugStream = new FileStream($"assetImageDebug/temp_{GameObject.ObjectName}.png",
                    FileMode.OpenOrCreate);
                Texture.SaveAsPng(debugStream, Width, Height);
                debugStream.Dispose();
                _haveSaved = true;
            }

#endif
            ActualGame.DrawSpace.Draw(
                Texture,
                Transform.Position,
                null,
                TextureColor,
                Transform.Rotation,
                new Vector2(Texture.Width / 2f, Texture.Height / 2f),
                Transform.Scale,
                SpriteEffects,
                Transform.LayerDepth
            );
        }
    }

#if DEBUG

    private bool _haveSaved;

#endif

    private Texture2D _texture;
}
