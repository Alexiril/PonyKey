using Game.BaseTypes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game.BuiltInComponents;

public class Sprite : Component
{
    public Color TextureColor { get; private set; } = Color.White;

    public Texture2D Texture { get; private set; }

    public SpriteEffects SpriteEffects { get; private set; } = SpriteEffects.None;

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

    public override void Draw()
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
