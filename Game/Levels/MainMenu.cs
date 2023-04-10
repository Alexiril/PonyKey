using Game.BaseTypes;
using Game.BuiltInComponents;
using Game.Components.MainMenu;
using Microsoft.Xna.Framework;

namespace Game.Levels;

internal class MainMenu : ILevel
{
    public Scene GetScene(InternalGame actualGame)
    {
        // Challenge: make a scene in one function chain - done :)
        var result = new Scene(actualGame);
        return result.SetBackgroundColor(Color.SkyBlue)
            .AddGameObject(new GameObject("Background"))
            .AddComponent<Sprite>()
            .SetTexture(actualGame.LoadSvg("MainMenuBackground", actualGame.ViewportSize))
            .Transform
            .SetPosition(actualGame.ViewportCenter)
            .ActualScene
            .AddGameObject(new GameObject("Logo"))
            .AddComponent<Sprite>()
            .SetTexture(actualGame.LoadSvg("MainMenuLogo", actualGame.ViewportSize))
            .Transform
            .SetPosition(actualGame.ViewportCenter)
            .SetRotation(25)
            .AddComponent<MoveLogoEntrance>()
            .ActualScene
            .AddGameObject(new GameObject("PlayButton"))
            .AddComponent<Sprite>()
            .SetTexture(actualGame.LoadSvg("PlayButtonMainMenu",
                new Vector2(220, 220) * actualGame.ResolutionCoefficient))
            .Transform
            .SetPosition(new(actualGame.ViewportCenter.X, actualGame.ViewportSize.Y * 1.5f))
            .AddComponent<MoveMainMenuButtonsEntrance>()
            .AddComponent<PlayButton>()
            .GameObject
            .AddComponent<InputTrigger>()
            .SetTriggerSize(result.GetGameObject(2).GetComponent<Sprite>().Size / 2)
            .ActualScene
            .AddGameObject(new GameObject("ExitButton"))
            .AddComponent<Sprite>()
            .SetTexture(actualGame.LoadSvg("MainMenuExitButton",
                new Vector2(155, 155) * actualGame.ResolutionCoefficient))
            .Transform
            .SetPosition(new(actualGame.ViewportCenter.X + 225 * actualGame.ResolutionCoefficient,
                actualGame.ViewportSize.Y * 1.55f))
            .AddComponent<InputTrigger>()
            .SetTriggerSize(result.GetGameObject(3).GetComponent<Sprite>().Size / 2)
            .AddComponent<ExitButton>()
            .ActualScene
            .AddGameObject(new GameObject("DisplayButton"))
            .AddComponent<Sprite>()
            .SetTexture(actualGame.LoadSvg("MainMenuDisplayButton",
                new Vector2(155, 155) * actualGame.ResolutionCoefficient))
            .Transform
            .SetPosition(new(actualGame.ViewportCenter.X - 225 * actualGame.ResolutionCoefficient,
                actualGame.ViewportSize.Y * 1.55f))
            .AddComponent<InputTrigger>()
            .SetTriggerSize(result.GetGameObject(4).GetComponent<Sprite>().Size / 2)
            .AddComponent<DisplayButton>()
            .ActualScene;
    }
}
