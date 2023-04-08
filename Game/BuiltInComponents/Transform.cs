using Game.BaseTypes;
using Microsoft.Xna.Framework;

namespace Game.BuiltInComponents;

public class Transform : Component
{
    public Vector2 Position { get; set; } = new Vector2();
    public float LayerDepth { get; set; } = 0;
    public float Rotation { get; set; } = 0;
    public float Scale { get; set; } = 1;
}
