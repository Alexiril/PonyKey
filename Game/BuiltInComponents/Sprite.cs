using Game.BaseTypes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game.BuiltInComponents;

public class Sprite : Component
{
    public string LoadingTextureName;

    public Texture2D Texture = null;

    public SpriteEffects SpriteEffects = SpriteEffects.None;

    public Color TextureColor = Color.White;

    public override void LoadContent()
    {
        Texture = GameObject.ActualGame.Content.Load<Texture2D>(LoadingTextureName);
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
