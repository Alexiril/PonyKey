using Engine.BaseTypes;

namespace Engine.BaseComponents;

public class LoadingSpinner: Component
{
    public float Speed { get; set; } = 1;

    public LoadingSpinner SetSpeed(float speed)
    {
        Speed = speed;
        return this;
    }

    public override void Update() => Transform.Rotation += DeltaTime * Speed;
}
