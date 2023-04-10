using Game.BaseTypes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game.BuiltInComponents;

internal class Sprite : Component
{
    internal Color TextureColor { get; set; } = Color.White;

    internal Texture2D Texture { get; set; }

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

    internal override void Draw()
    {
        if (Texture != null)
        {
            GameObject.ActualGame.DrawSpace.Draw(
                Texture,
                GameObject.Transform.Position,
                null,
                TextureColor,
                GameObject.Transform.Rotation,
                new Vector2(Texture.Width / 2f, Texture.Height / 2f),
                GameObject.Transform.Scale,
                SpriteEffects,
                GameObject.Transform.LayerDepth
                );
        }
    }
}
