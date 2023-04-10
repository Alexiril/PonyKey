using Game.BaseTypes;
using Game.BuiltInComponents;
using Game.Components.MainMenu;
using Microsoft.Xna.Framework;

namespace Game.Levels;

internal class MainMenu : ILevel
{
    public Scene GetScene(InternalGame actualGame)
    {
        var result = new Scene(actualGame).SetBackgroundColor(Color.SkyBlue);
        result.AddGameObject(new GameObject("Background")).AddComponent<Sprite>()
            .SetTexture(actualGame.LoadSvg("MainMenuBackground", actualGame.ViewportSize)).GameObject.Transform
            .SetPosition(actualGame.ViewportCenter);
        result.AddGameObject(new GameObject("Logo")).AddComponent<Sprite>()
            .SetTexture(actualGame.LoadSvg("MainMenuLogo", actualGame.ViewportSize)).GameObject.Transform
            .SetPosition(actualGame.ViewportCenter).SetRotation(25).GameObject.AddComponent<MoveLogoEntrance>();
        var inputTrigger = result.AddGameObject(new GameObject("PlayButton")).AddComponent<Sprite>()
            .SetTexture(actualGame.LoadSvg("PlayButtonMainMenu",
                new Vector2(220, 220) * actualGame.ResolutionCoefficient))
            .GameObject.Transform.SetPosition(new(actualGame.ViewportCenter.X, actualGame.ViewportSize.Y * 1.5f))
            .GameObject.AddComponent<MovePlayButtonEntrance>().GameObject.AddComponent<PlayButton>().GameObject
            .AddComponent<InputTrigger>();
        inputTrigger.TriggerSize = inputTrigger.GameObject.GetComponent<Sprite>().Size / 2;
        result.Start();
        return result;
    }
}
