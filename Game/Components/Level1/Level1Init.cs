using Engine.BaseTypes;
using Game.Components.Common;

namespace Game.Components.Level1;

public class Level1Init: Component
{
    public override void Start()
    {
        GameObject.Find("HelperText")[0].GetComponent<MovingText>()
            .SetShouldDestroy(_ => _.Transform.Position.Y < -_.Sprite.Height);
    }
}
