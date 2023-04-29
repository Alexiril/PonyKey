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
            .AddGameObject(new("loadingSpinner"))
            .Transform.SetPosition(EGame.ViewportCenter)
            .AddComponent<Sprite>()
            .SetTexture(SvgConverter.LoadSvg("loadingSpinner", new(EGame.ViewportSize.X * .7f)))
            .AddComponent<LoadingSpinner>()
            .SetSpeed(5)
            .AddGameObject(new GameObject("Background"))
            .AddComponent<Sprite>()
            .SetTexture(SvgConverter.LoadSvg("MainMenu/Background", EGame.ViewportSize))
            .Transform.SetPosition(EGame.ViewportCenter)
            .AddComponent<SoundSource>()
            .SetSound(ArchivedContent.LoadContent<SoundEffect>("MainMenu/BackgroundMusicMenu"))
            .SetIsLooped(true)
            .SetVolume(float.TryParse(PlayerSettings.GetValue("vl"), out var value) ? value : 1)
            .SetPlayAtStart(true)
            .AddGameObject(new GameObject("Logo"))
            .AddComponent<Sprite>()
            .SetTexture(SvgConverter.LoadSvg("MainMenu/Logo", EGame.ViewportSize))
            .Transform.SetPosition(EGame.ViewportCenter)
            .AddComponent<MovingText>()
            .SetShouldDestroy(text => text.Transform.Position.Y < -text.Sprite.Height)
            .AddGameObject(new GameObject("PlayButton"))
            .AddComponent<Sprite>()
            .SetTexture(SvgConverter.LoadSvg("MainMenu/PlayButton",
                new Vector2(190, 190) * EGame.ResolutionCoefficient))
            .Transform.SetPosition(new(EGame.ViewportCenter.X, EGame.ViewportSize.Y * 1.5f))
            .AddComponent<MoveMainMenuButtonsEntrance>()
            .AddComponent<PlayButton>()
            .AddGameObject(new GameObject("ExitButton"))
            .AddComponent<Sprite>()
            .SetTexture(SvgConverter.LoadSvg("MainMenu/ExitButton",
                new Vector2(125, 125) * EGame.ResolutionCoefficient))
            .Transform.SetPosition(new(EGame.ViewportCenter.X + 225 * EGame.ResolutionCoefficient,
                                    EGame.ViewportSize.Y * 1.55f))
            .AddComponent<SpriteButton>()
            .SetOnPointerUp(_ => EGame.Exit())
            .ActualScene
            .AddGameObject(new GameObject("DisplayButton"))
            .AddComponent<Sprite>()
            .SetTexture(SvgConverter.LoadSvg("MainMenu/DisplayButton",
                new Vector2(125, 125) * EGame.ResolutionCoefficient))
            .Transform.SetPosition(new(EGame.ViewportCenter.X - 225 * EGame.ResolutionCoefficient,
                                    EGame.ViewportSize.Y * 1.55f))
            .AddComponent<SpriteButton>()
            .SetOnPointerUp(_ => EGame.ChangeVideoMode())
            .AddGameObject(new GameObject("SoundButton"))
            .AddComponent<Sprite>()
            .SetTexture(SvgConverter.LoadSvg(
                !float.TryParse(PlayerSettings.GetValue("vl"), out var soundValue) ? "MainMenu/SoundButton" :
                    soundValue > 0 ? "MainMenu/SoundButton" : "MainMenu/SoundOffButton",
                new Vector2(95, 95) * EGame.ResolutionCoefficient))
            .Transform.SetPosition(new(EGame.ViewportCenter.X - 380 * EGame.ResolutionCoefficient,
                                EGame.ViewportSize.Y * 1.57f))
            .AddComponent<SoundButton>()
            .SetStandardButtonTexture(SvgConverter.LoadSvg("MainMenu/SoundButton",
                new Vector2(95, 95) * EGame.ResolutionCoefficient))
            .SetTurnedOffButtonTexture(SvgConverter.LoadSvg("MainMenu/SoundOffButton",
                new Vector2(95, 95) * EGame.ResolutionCoefficient))
            .AddGameObject(new GameObject("InfoButton"))
            .AddComponent<Sprite>()
            .SetTexture(SvgConverter.LoadSvg("MainMenu/InfoButton",
                new Vector2(95, 95) * EGame.ResolutionCoefficient))
            .Transform.SetPosition(new(EGame.ViewportCenter.X + 380 * EGame.ResolutionCoefficient,
                                        EGame.ViewportSize.Y * 1.57f))
            .AddComponent<SpriteButton>()
            .SetOnPointerUp(_ => Process.Start(new ProcessStartInfo { FileName = "https://github.com/Alexiril/PonyKey", UseShellExecute = true }))
            .ActualScene;
}
