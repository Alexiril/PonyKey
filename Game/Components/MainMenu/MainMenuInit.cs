using Engine.BaseTypes;
using Game.Components.Common;

namespace Game.Components.MainMenu;

public class MainMenuInit: Component
{
    public override void Start()
    {
        GameObject.Find("Logo")[0].GetComponent<MovingText>()
            .SetShouldDestroy(_ => _.Transform.Position.Y < -_.Sprite.Height);

    }
}
