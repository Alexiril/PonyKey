using Game.BaseTypes;
using Microsoft.Xna.Framework;

namespace Game.BuiltInComponents;

internal class Transform : Component
{
    internal Vector2 Position { get; set; }

    internal Transform SetPosition(Vector2 position)
    {
        Position = position;
        return this;
    }

    internal float LayerDepth { get; set; }

    internal Transform SetLayerDepth(float layerDepth)
    {
        LayerDepth = layerDepth;
        return this;
    }

    internal float Rotation { get; set; }

    internal Transform SetRotation(float rotation)
    {
        Rotation = rotation;
        return this;
    }

    internal float Scale { get; set; } = 1;

    internal Transform SetScale(float scale)
    {
        Scale = scale;
        return this;
    }

    internal Vector2 Up => Vector2.Transform(new(0, -1), Matrix.CreateRotationZ(Rotation));

    internal Vector2 Right => Vector2.Transform(new(1, 0), Matrix.CreateRotationZ(Rotation));
}
