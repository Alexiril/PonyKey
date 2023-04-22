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
    public Scene GetScene(Master master) =>
        new Scene(master, "MainMenu")
            .SetBackgroundColor(Color.DeepSkyBlue)
            .AddGameObject(new GameObject("Background"))
            .AddComponent<Sprite>()
            .SetTexture(SvgConverter.LoadSvg(master,"MainMenu/Background", master.ViewportSize))
            .Transform.SetPosition(master.ViewportCenter)
            .AddComponent<SoundSource>()
            .SetSound(master.LoadContent<SoundEffect>("MainMenu/BackgroundMusic"))
            .SetIsLooped(true)
            .SetVolume(float.TryParse(PlayerSettings.GetValue("vl"), out var value) ? value : 1)
            .SetPlayAtStart(true)
            .AddGameObject(new GameObject("Logo"))
            .AddComponent<Sprite>()
            .SetTexture(SvgConverter.LoadSvg(master,"MainMenu/Logo", master.ViewportSize))
            .Transform.SetPosition(master.ViewportCenter)
            .AddComponent<MovingText>()
            .SetShouldDestroy(text => text.Transform.Position.Y < -text.Sprite.Height)
            .AddGameObject(new GameObject("PlayButton"))
            .AddComponent<Sprite>()
            .SetTexture(SvgConverter.LoadSvg(master,"MainMenu/PlayButton",
                new Vector2(190, 190) * master.ResolutionCoefficient))
            .Transform.SetPosition(new(master.ViewportCenter.X, master.ViewportSize.Y * 1.5f))
            .AddComponent<MoveMainMenuButtonsEntrance>()
            .AddComponent<PlayButton>()
            .AddGameObject(new GameObject("ExitButton"))
            .AddComponent<Sprite>()
            .SetTexture(SvgConverter.LoadSvg(master,"MainMenu/ExitButton",
                new Vector2(125, 125) * master.ResolutionCoefficient))
            .Transform.SetPosition(new(master.ViewportCenter.X + 225 * master.ResolutionCoefficient,
                                    master.ViewportSize.Y * 1.55f))
            .AddComponent<SpriteButton>()
            .SetOnPointerUp(_ => master.Exit())
            .ActualScene
            .AddGameObject(new GameObject("DisplayButton"))
            .AddComponent<Sprite>()
            .SetTexture(SvgConverter.LoadSvg(master,"MainMenu/DisplayButton",
                new Vector2(125, 125) * master.ResolutionCoefficient))
            .Transform.SetPosition(new(master.ViewportCenter.X - 225 * master.ResolutionCoefficient,
                                    master.ViewportSize.Y * 1.55f))
            .AddComponent<SpriteButton>()
            .SetOnPointerUp(_ => master.ChangeVideoMode())
            .AddGameObject(new GameObject("SoundButton"))
            .AddComponent<Sprite>()
            .SetTexture(SvgConverter.LoadSvg(master,"MainMenu/SoundButton",
                new Vector2(95, 95) * master.ResolutionCoefficient))
            .Transform.SetPosition(new(master.ViewportCenter.X - 380 * master.ResolutionCoefficient,
                                master.ViewportSize.Y * 1.57f))
            .AddComponent<SoundButton>()
            .SetStandardButtonTexture(SvgConverter.LoadSvg(master,"MainMenu/SoundButton",
                new Vector2(95, 95) * master.ResolutionCoefficient))
            .SetTurnedOffButtonTexture(SvgConverter.LoadSvg(master,"MainMenu/SoundOffButton",
                new Vector2(95, 95) * master.ResolutionCoefficient))
            .AddGameObject(new GameObject("InfoButton"))
            .AddComponent<Sprite>()
            .SetTexture(SvgConverter.LoadSvg(master,"MainMenu/InfoButton",
                new Vector2(95, 95) * master.ResolutionCoefficient))
            .Transform.SetPosition(new(master.ViewportCenter.X + 380 * master.ResolutionCoefficient,
                                        master.ViewportSize.Y * 1.57f))
            .AddComponent<SpriteButton>()
            .SetOnPointerUp(_ => Process.Start(new ProcessStartInfo { FileName = "https://github.com/Alexiril/PonyKey", UseShellExecute = true }))
            .ActualScene;
}
