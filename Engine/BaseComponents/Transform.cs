using Engine.BaseTypes;
using Microsoft.Xna.Framework;
using Game = Engine.BaseSystems.Game;
#if DEBUG
using Microsoft.Xna.Framework.Graphics;
using Engine.BaseSystems;
#endif

namespace Engine.BaseComponents;

public class Transform : Component
{
    public Transform() {}

    public Transform(Transform transform) : base(transform)
    {
        Position = transform.Position;
        LayerDepth = transform.LayerDepth;
        Rotation = transform.Rotation;
        Scale = transform.Scale;
    }

    public Vector2 Position { get; set; }

    public Transform SetPosition(Vector2 position)
    {
        Position = position;
        return this;
    }

    public float LayerDepth { get; set; }

    public Transform SetLayerDepth(float layerDepth)
    {
        LayerDepth = layerDepth;
        return this;
    }

    public float Rotation { get; set; }

    public Transform SetRotation(float rotation)
    {
        Rotation = rotation;
        return this;
    }

    public float Scale { get; set; } = 1;

    public Transform SetScale(float scale)
    {
        Scale = scale;
        return this;
    }

    public Transform SetScaleFromSprite()
    {
        Scale = Sprite.ResolutionCoefficient * Game.ResolutionCoefficient;
        return this;
    }

    public Vector2 Up => Vector2.Transform(new(0, -1), Matrix.CreateRotationZ(Rotation));

    public Vector2 Right => Vector2.Transform(new(1, 0), Matrix.CreateRotationZ(Rotation));

    public static Vector2 GlobalUp => -Vector2.UnitY;

    public static Vector2 GlobalRight => Vector2.UnitX;

#if DEBUG

    public override void Draw()
    {
        if (!Game.DebugPointsOn) return;
        if (_debugTexture == null)
            GenerateDebugTexture();
        Game.DrawSpace.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);
        if (_debugTexture != null)
            Game.DrawSpace.Draw(
                _debugTexture,
                Position,
                null,
                Color.White,
                0,
                new Vector2(_debugTexture.Width / 2f, _debugTexture.Height / 2f),
                1,
                SpriteEffects.None,
                0
            );
        Game.DrawSpace.DrawString(Game.DebugFont, GameObject.Name, Position, Color.White * .5f);
        Game.DrawSpace.End();

    }

    private Texture2D _debugTexture;

    private void GenerateDebugTexture() =>
        _debugTexture = TextureGenerator.GenerateTexture(
            70,
            70,
            (x, y) => Color.IndianRed * (5f / ((x - 35) * (x - 35) + (y - 35) * (y - 35))));
#endif

}
