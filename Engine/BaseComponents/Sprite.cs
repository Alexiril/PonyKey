using Engine.BaseTypes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.BaseComponents;

public class Sprite : Component
{
    public Sprite() {}

    public Sprite(Sprite sprite) : base(sprite)
    {
        TextureColor = sprite.TextureColor;
        Texture = sprite.Texture;
        SpriteEffects = sprite.SpriteEffects;
    }

    public Color TextureColor { get; set; } = Color.White;

    public Texture2D Texture { get; set; }

    public SpriteEffects SpriteEffects { get; set; } = SpriteEffects.None;

    public Sprite SetTexture(Texture2D texture)
    {
        Texture = texture;
        return this;
    }

    public Sprite SetSpriteEffects(SpriteEffects effects)
    {
        SpriteEffects = effects;
        return this;
    }

    public Sprite SetTextureColor(Color color)
    {
        TextureColor = color;
        return this;
    }

    public int Width => Texture.Width;

    public int Height => Texture.Height;

    public Vector2 Size => new(Width, Height);

    public float ResolutionCoefficient =>
        .5f * (Width / Master.ViewportSize.X) + .5f * (Height / Master.ViewportSize.Y);

    public override void Draw()
    {
        if (Texture == null) return;
        Master.DrawSpace.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);
        Master.DrawSpace.Draw(
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
        Master.DrawSpace.End();
    }
}
