using Engine.BaseComponents;
using Engine.BaseTypes;
using Game.Components.Common;

namespace Game.Components.MainMenu;

internal class MainMenuInit: Component
{
    public override void Start()
    {
        GameObject.Find("Logo")[0].GetComponent<MovingText>()
            .SetShouldDestroy(_ => _.Transform.Position.Y < -_.Sprite.Height);
        GameObject.Find("ExitButton")[0].GetComponent<SpriteButton>()
            .SetOnPointerUp(_ => Engine.BaseSystems.Game.Exit());
        GameObject.Find("DisplayButton")[0].GetComponent<SpriteButton>()
            .SetOnPointerUp(_ => Engine.BaseSystems.Game.ChangeVideoMode());
        GameObject.Find("InfoButton")[0].GetComponent<SpriteButton>()
            .SetOnPointerUp(_ => System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo{FileName ="https://github.com/Alexiril/PonyKey",UseShellExecute=true}));
    }
}
