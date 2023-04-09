using Game.BaseTypes;
using Microsoft.Xna.Framework;

namespace Game.BuiltInComponents;

public class Transform : Component
{
    public Vector2 Position { get; private set; } = new Vector2();

    public Transform SetPosition(Vector2 position)
    {
        Position = position;
        return this;
    }

    public float LayerDepth { get; private set; } = 0;

    public Transform SetLayerDepth(float layerDepth)
    {
        LayerDepth = layerDepth;
        return this;
    }

    public float Rotation { get; private set; } = 0;

    public Transform SetRotation(float rotation)
    {
        Rotation = rotation;
        return this;
    }

    public float Scale { get; private set; } = 1;

    public Transform SetScale(float scale)
    {
        Scale = scale;
        return this;
    }
}
