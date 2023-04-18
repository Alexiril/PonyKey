using System.Diagnostics;
using Engine.BaseComponents;
using Engine.BaseSystems;
using Engine.BaseTypes;
using Game.Components.Common;
using Game.Components.MainMenu;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace Game.Levels;

internal class MainMenu : ILevel
{
    public Scene GetScene(ActualGame actualGame) =>
        new Scene(actualGame).SetBackgroundColor(Color.SkyBlue)
            .AddGameObject(new GameObject("Background"))
            .AddComponent<Sprite>()
            .SetTexture(actualGame.LoadSvg("MainMenu/Background", actualGame.ViewportSize))
            .Transform
            .SetPosition(actualGame.ViewportCenter)
            .AddComponent<SoundSource>()
            .SetSound(actualGame.Content.Load<SoundEffect>("MainMenu/BackgroundMusic"))
            .SetIsLooped(true)
            .SetVolume(1)
            .SetPlayAtStart(true)
            .ActualScene
            .AddGameObject(new GameObject("Logo"))
            .AddComponent<Sprite>()
            .SetTexture(actualGame.LoadSvg("MainMenu/Logo", actualGame.ViewportSize))
            .Transform
            .SetPosition(actualGame.ViewportCenter)
            .AddComponent<MovingText>()
            .SetShouldDestroy(text => text.Transform.Position.Y < -text.Sprite.Height)
            .ActualScene
            .AddGameObject(new GameObject("PlayButton"))
            .AddComponent<Sprite>()
            .SetTexture(actualGame.LoadSvg("MainMenu/PlayButton",
                new Vector2(190, 190) * actualGame.ResolutionCoefficient))
            .Transform
            .SetPosition(new(actualGame.ViewportCenter.X, actualGame.ViewportSize.Y * 1.5f))
            .AddComponent<MoveMainMenuButtonsEntrance>()
            .AddComponent<PlayButton>()
            .ActualScene
            .AddGameObject(new GameObject("ExitButton"))
            .AddComponent<Sprite>()
            .SetTexture(actualGame.LoadSvg("MainMenu/ExitButton",
                new Vector2(125, 125) * actualGame.ResolutionCoefficient))
            .Transform
            .SetPosition(new(actualGame.ViewportCenter.X + 225 * actualGame.ResolutionCoefficient,
                actualGame.ViewportSize.Y * 1.55f))
            .AddComponent<SpriteButton>()
            .SetOnPointerUp(_ => actualGame.Exit())
            .ActualScene
            .AddGameObject(new GameObject("DisplayButton"))
            .AddComponent<Sprite>()
            .SetTexture(actualGame.LoadSvg("MainMenu/DisplayButton",
                new Vector2(125, 125) * actualGame.ResolutionCoefficient))
            .Transform
            .SetPosition(new(actualGame.ViewportCenter.X - 225 * actualGame.ResolutionCoefficient,
                actualGame.ViewportSize.Y * 1.55f))
            .AddComponent<SpriteButton>()
            .SetOnPointerUp(_ => actualGame.ChangeVideoMode())
            .ActualScene
            .AddGameObject(new GameObject("SoundButton"))
            .AddComponent<Sprite>()
            .SetTexture(actualGame.LoadSvg("MainMenu/SoundButton",
                new Vector2(95, 95) * actualGame.ResolutionCoefficient))
            .Transform
            .SetPosition(new(actualGame.ViewportCenter.X - 380 * actualGame.ResolutionCoefficient,
                actualGame.ViewportSize.Y * 1.57f))
            .AddComponent<SoundButton>()
            .SetStandardButtonTexture(actualGame.LoadSvg("MainMenu/SoundButton",
                new Vector2(95, 95) * actualGame.ResolutionCoefficient))
            .SetTurnedOffButtonTexture(actualGame.LoadSvg("MainMenu/SoundOffButton",
                new Vector2(95, 95) * actualGame.ResolutionCoefficient))
            .ActualScene
            .AddGameObject(new GameObject("InfoButton"))
            .AddComponent<Sprite>()
            .SetTexture(actualGame.LoadSvg("MainMenu/InfoButton",
                new Vector2(95, 95) * actualGame.ResolutionCoefficient))
            .Transform
            .SetPosition(new(actualGame.ViewportCenter.X + 380 * actualGame.ResolutionCoefficient,
                actualGame.ViewportSize.Y * 1.57f))
            .AddComponent<SpriteButton>()
            .SetOnPointerUp(_ => Process.Start(new ProcessStartInfo { FileName = "https://github.com/Alexiril/PonyKey", UseShellExecute = true }))
            .ActualScene;
}
