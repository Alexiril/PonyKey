using System.Diagnostics;
using Engine.BaseComponents;
using Engine.BaseSystems;
using Engine.BaseTypes;
using Game.Components.Common;
using Game.Components.MainMenu;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using EGame = Engine.BaseSystems.Game;

namespace Game.Levels;

internal class MainMenu : ILevel
{
    public Scene GetScene() =>
        new Scene("MainMenu")
            .SetBackgroundColor(Color.DeepSkyBlue)
            .AddGameObject(LoadingSpinner)
            .AddGameObject(Background)
            .AddGameObject(Logo)
            .AddGameObject(PlayButton)
            .AddGameObject(ExitButton)
            .AddGameObject(DisplayButton)
            .AddGameObject(SoundButton)
            .AddGameObject(InfoButton)
            .ActualScene;

    private static GameObject LoadingSpinner =>
        new GameObject("LoadingSpinner")
            .Transform.SetPosition(EGame.ViewportCenter)
            .AddComponent<Sprite>()
            .SetTexture(SvgConverter.LoadSvg("loadingSpinner", new(EGame.ViewportSize.X * .7f)))
            .AddComponent<LoadingSpinner>()
            .SetSpeed(5)
            .GameObject;

    private static GameObject Background =>
        new GameObject("Background")
            .AddComponent<Sprite>()
            .SetTexture(SvgConverter.LoadSvg("MainMenu/Background", EGame.ViewportSize))
            .Transform.SetPosition(EGame.ViewportCenter)
            .AddComponent<SoundSource>()
            .SetSound(ArchivedContent.LoadContent<SoundEffect>("MainMenu/BackgroundMusicMenu"))
            .SetIsLooped(true)
            .SetVolume(float.TryParse(PlayerSettings.GetValue("vl"), out var value) ? value : 1)
            .SetPlayAtStart(true)
            .GameObject;

    private static GameObject Logo =>
        new GameObject("Logo")
            .AddComponent<Sprite>()
            .SetTexture(SvgConverter.LoadSvg("MainMenu/Logo", EGame.ViewportSize))
            .Transform.SetPosition(EGame.ViewportCenter)
            .AddComponent<MovingText>()
            .SetShouldDestroy(text => text.Transform.Position.Y < -text.Sprite.Height)
            .GameObject;

    private static GameObject GenerateButton(string name, string assetName, Vector2 size, Vector2 position) =>
        new GameObject(name)
            .AddComponent<Sprite>()
            .SetTexture(SvgConverter.LoadSvg(assetName, size))
            .Transform.SetPosition(position)
            .GameObject;

    private static GameObject PlayButton =>
        GenerateButton(
                "PlayButton",
                "MainMenu/PlayButton",
                new Vector2(190, 190) * EGame.ResolutionCoefficient,
                new(EGame.ViewportCenter.X, EGame.ViewportSize.Y * 1.5f)
                )
            .AddComponent<MoveMainMenuButtonsEntrance>()
            .AddComponent<PlayButton>()
            .GameObject;

    private static GameObject ExitButton =>
        GenerateButton(
                "ExitButton",
                "MainMenu/ExitButton",
                new Vector2(125, 125) * EGame.ResolutionCoefficient,
                new(
                    EGame.ViewportCenter.X + 225 * EGame.ResolutionCoefficient,
                    EGame.ViewportSize.Y * 1.55f
                    )
                )
            .AddComponent<SpriteButton>()
            .SetOnPointerUp(_ => EGame.Exit())
            .GameObject;

    private static GameObject DisplayButton =>
        GenerateButton(
                "DisplayButton",
                "MainMenu/DisplayButton",
                new Vector2(125, 125) * EGame.ResolutionCoefficient,
                new(
                    EGame.ViewportCenter.X - 225 * EGame.ResolutionCoefficient,
                    EGame.ViewportSize.Y * 1.55f
                    )
                )
            .AddComponent<SpriteButton>()
            .SetOnPointerUp(_ => EGame.ChangeVideoMode())
            .GameObject;

    private static GameObject SoundButton =>
        GenerateButton(
                "SoundButton",
                !float.TryParse(PlayerSettings.GetValue("vl"), out var soundValue) ?
                    "MainMenu/SoundButton" :
                    soundValue > 0 ?
                        "MainMenu/SoundButton" :
                        "MainMenu/SoundOffButton",
                new Vector2(95, 95) * EGame.ResolutionCoefficient,
                new(
                    EGame.ViewportCenter.X - 380 * EGame.ResolutionCoefficient,
                    EGame.ViewportSize.Y * 1.57f
                    )
                )
            .AddComponent<SoundButton>()
            .SetStandardButtonTexture(SvgConverter.LoadSvg(
                "MainMenu/SoundButton", new Vector2(95, 95) * EGame.ResolutionCoefficient))
            .SetTurnedOffButtonTexture(SvgConverter.LoadSvg(
                "MainMenu/SoundOffButton", new Vector2(95, 95) * EGame.ResolutionCoefficient))
            .GameObject;

    private static GameObject InfoButton =>
        GenerateButton(
                "InfoButton",
                "MainMenu/InfoButton",
                new Vector2(95, 95) * EGame.ResolutionCoefficient,
                new(
                    EGame.ViewportCenter.X + 380 * EGame.ResolutionCoefficient,
                    EGame.ViewportSize.Y * 1.57f
                    )
                )
            .AddComponent<SpriteButton>()
            .SetOnPointerUp(_ => Process.Start(new ProcessStartInfo
                { FileName = "https://github.com/Alexiril/PonyKey", UseShellExecute = true }))
            .GameObject;
}
