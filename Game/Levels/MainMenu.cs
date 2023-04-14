using Game.BaseTypes;
using Game.BuiltInComponents;
using Game.Components.MainMenu;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace Game.Levels;

internal class MainMenu : ILevel
{
    public Scene GetScene(InternalGame actualGame)
    {
        // Challenge: make a scene in one function chain - done :)
        return new Scene(actualGame).SetBackgroundColor(Color.SkyBlue)
            .AddGameObject(new GameObject("Background"))
            .AddComponent<Sprite>()
            .SetTexture(actualGame.LoadSvg("MainMenuBackground", actualGame.ViewportSize))
            .Transform
            .SetPosition(actualGame.ViewportCenter)
            .AddComponent<SoundSource>()
            .SetSound(actualGame.Content.Load<SoundEffect>("MainMenuBackgroundMusic"))
            .SetIsLooped(true)
            .SetVolume(1)
            .SetPlayAtStart(true)
            .ActualScene
            .AddGameObject(new GameObject("Logo"))
            .AddComponent<Sprite>()
            .SetTexture(actualGame.LoadSvg("MainMenuLogo", actualGame.ViewportSize))
            .Transform
            .SetPosition(actualGame.ViewportCenter)
            .AddComponent<MoveLogoEntrance>()
            .ActualScene
            .AddGameObject(new GameObject("PlayButton"))
            .AddComponent<Sprite>()
            .SetTexture(actualGame.LoadSvg("PlayButtonMainMenu",
                new Vector2(190, 190) * actualGame.ResolutionCoefficient))
            .Transform
            .SetPosition(new(actualGame.ViewportCenter.X, actualGame.ViewportSize.Y * 1.5f))
            .AddComponent<MoveMainMenuButtonsEntrance>()
            .AddComponent<PlayButton>()
            .GameObject
            .AddComponent<InputTrigger>()
            .SetTriggerSizeFromSprite()
            .ActualScene
            .AddGameObject(new GameObject("ExitButton"))
            .AddComponent<Sprite>()
            .SetTexture(actualGame.LoadSvg("MainMenuExitButton",
                new Vector2(125, 125) * actualGame.ResolutionCoefficient))
            .Transform
            .SetPosition(new(actualGame.ViewportCenter.X + 225 * actualGame.ResolutionCoefficient,
                actualGame.ViewportSize.Y * 1.55f))
            .AddComponent<InputTrigger>()
            .SetTriggerSizeFromSprite()
            .AddComponent<ExitButton>()
            .ActualScene
            .AddGameObject(new GameObject("DisplayButton"))
            .AddComponent<Sprite>()
            .SetTexture(actualGame.LoadSvg("MainMenuDisplayButton",
                new Vector2(125, 125) * actualGame.ResolutionCoefficient))
            .Transform
            .SetPosition(new(actualGame.ViewportCenter.X - 225 * actualGame.ResolutionCoefficient,
                actualGame.ViewportSize.Y * 1.55f))
            .AddComponent<InputTrigger>()
            .SetTriggerSizeFromSprite()
            .AddComponent<DisplayButton>()
            .ActualScene
            .AddGameObject(new GameObject("SoundButton"))
            .AddComponent<Sprite>()
            .SetTexture(actualGame.LoadSvg("MainMenuSoundButton",
                new Vector2(95, 95) * actualGame.ResolutionCoefficient))
            .Transform
            .SetPosition(new(actualGame.ViewportCenter.X - 380 * actualGame.ResolutionCoefficient,
                actualGame.ViewportSize.Y * 1.57f))
            .AddComponent<InputTrigger>()
            .SetTriggerSizeFromSprite()
            .AddComponent<SoundButton>()
            .SetStandardButtonTexture(actualGame.LoadSvg("MainMenuSoundButton",
                new Vector2(95, 95) * actualGame.ResolutionCoefficient))
            .SetTurnedOffButtonTexture(actualGame.LoadSvg("MainMenuSoundOffButton",
                new Vector2(95, 95) * actualGame.ResolutionCoefficient))
            .ActualScene;
    }
}
