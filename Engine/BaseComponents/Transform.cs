using Engine.BaseTypes;
using Microsoft.Xna.Framework;

namespace Engine.BaseComponents;

public class Transform : Component
{
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
        Scale = Sprite.ResolutionCoefficient * ActualGame.ResolutionCoefficient;
        return this;
    }

    public Vector2 Up => Vector2.Transform(new(0, -1), Matrix.CreateRotationZ(Rotation));

    public Vector2 Right => Vector2.Transform(new(1, 0), Matrix.CreateRotationZ(Rotation));

    public static Vector2 GlobalUp => -Vector2.UnitY;

    public static Vector2 GlobalRight => Vector2.UnitX;
}
