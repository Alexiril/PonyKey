using Engine.BaseTypes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Game = Engine.BaseSystems.Game;

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

    public Vector2 Center => new((float)Width / 2, (float)Height / 2);

    public Vector2 Size => new(Width, Height);

    public float ResolutionCoefficient =>
        .5f * (Width / Game.ViewportSize.X) + .5f * (Height / Game.ViewportSize.Y);

    public void AppendTexture(Texture2D texture, Vector2 position)
    {
        var rt = new RenderTarget2D(
            Game.GraphicsDevice,
            texture.Width > Texture.Width ? texture.Width : Texture.Width,
            texture.Height > Texture.Height ? texture.Height : Texture.Height
        );
        var targets = Game.GraphicsDevice.GetRenderTargets();
        Game.GraphicsDevice.SetRenderTarget(rt);
        Game.GraphicsDevice.Clear(Color.Transparent);
        Game.DrawSpace.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);
        Game.DrawSpace.Draw(Texture, new Vector2(0, 0), Color.White);
        Game.DrawSpace.Draw(texture, position, Color.White);
        Game.DrawSpace.End();
        Game.GraphicsDevice.SetRenderTargets(targets);
        Texture = rt;
    }

    public override void Draw()
    {
        if (Texture == null) return;
        Game.DrawSpace.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);
        Game.DrawSpace.Draw(
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
        Game.DrawSpace.End();
    }
}
