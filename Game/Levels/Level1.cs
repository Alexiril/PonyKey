using Engine.BaseComponents;
using Engine.BaseSystems;
using Engine.BaseTypes;
using Game.Components.Common;
using Game.Components.Level1;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using EGame = Engine.BaseSystems.Game;

namespace Game.Levels;

internal class Level1 : ILevel
{
    public Scene GetScene() =>
        new Scene("Level1")
            .SetBackgroundColor(Color.DeepSkyBlue)
            .AddGameObject(PersonScore)
            .AddGameObject(Background)
            .AddGameObject(ScorePanel)
            .AddGameObject(TreesGenerator)
            .AddGameObject(Background1)
            .AddGameObject(AJRunning)
            .AddGameObject(Background2)
            .AddGameObject(PonyTalking)
            .AddGameObject(SpeechCloud)
            .AddGameObject(HelperText)
            .ActualScene;

    private static GameObject PersonScore =>
        new GameObject("PersonScore")
            .AddComponent<Score>()
            .GameObject;

    private static GameObject Background =>
        new GameObject("Background")
            .AddComponent<Sprite>()
            .SetTexture(EngineContent.LoadSvg("Level1/Background0", EGame.ViewportSize))
            .Transform.SetPosition(EGame.ViewportCenter)
            .AddComponent<SoundSource>()
            .SetSound(EngineContent.LoadContent<SoundEffect>("Level1/BackgroundMusicLevel1"))
            .SetIsLooped(true)
            .SetVolume(float.TryParse(PlayerSettings.GetValue("vl"), out var value) ? value : 1)
            .SetPlayAtStart(true)
            .GameObject;

    private static GameObject ScorePanel =>
        new GameObject("ScorePanel")
            .SetActive(false)
            .Transform.SetPosition(new(EGame.ViewportCenter.X, EGame.ViewportSize.Y * .025f))
            .AddComponent<TextMesh>()
            .SetColor(Color.White)
            .SetWordWrap(true)
            .SetFont(EngineContent.LoadContent<SpriteFont>("Common/TwilightSpeechFont21"))
            .SetText("Score: 0")
            .SetWidth(200)
            .SetOffset(new(-50, 0))
            .GameObject;

    private static GameObject TreesGenerator =>
        new GameObject("TreesGenerator")
            .SetActive(false)
            .Transform.SetPosition(new(EGame.ViewportSize.X * 1.5f, EGame.ViewportSize.Y * .7f))
            .AddComponent<TreesGenerator>()
            .GameObject;

    private static GameObject Background1 =>
        new GameObject("Background1")
            .AddComponent<Sprite>()
            .SetTexture(EngineContent.LoadSvg(
                "Level1/Background1",
                new Vector2(2560, 720) * EGame.ResolutionCoefficient)
            )
            .Transform.SetPosition(new(EGame.ViewportSize.X, EGame.ViewportCenter.Y))
            .GameObject;

    // ReSharper disable once InconsistentNaming
    private static GameObject AJRunning =>
        new GameObject("AJRunning")
            .AddComponent<Animator>()
            .SetAnimationInformation(EngineContent.LoadSvgAnimation(
                "Level1/ajAnimation",
                new Vector2(512) * EGame.ResolutionCoefficient)
            )
            .SetPlaying(true)
            .SetLoop(true)
            .Transform.SetPosition(new(-EGame.ViewportSize.X * .2f, EGame.ViewportSize.Y * .75f))
            .AddComponent<ApplejackRunning>()
            .GameObject;

    private static GameObject Background2 =>
        new GameObject("Background2")
            .AddComponent<Sprite>()
            .SetTexture(EngineContent.LoadSvg(
                "Level1/Background2",
                new Vector2(2560, 720) * EGame.ResolutionCoefficient)
            )
            .Transform.SetPosition(new(EGame.ViewportSize.X, EGame.ViewportCenter.Y))
            .GameObject;

    private static GameObject PonyTalking =>
        new GameObject("PonyTalking")
            .AddComponent<Sprite>()
            .SetTexture(EngineContent.LoadSvg(
                "Common/TwilightUnhappy",
                new Vector2(600, 600) * EGame.ResolutionCoefficient)
            )
            .Transform.SetPosition(new(EGame.ViewportSize.X * .66f, EGame.ViewportCenter.Y))
            .GameObject;

    private static GameObject SpeechCloud =>
        new GameObject("SpeechCloud")
            .Transform.SetPosition(new(EGame.ViewportSize.X * .3f, EGame.ViewportSize.Y * .3f))
            .AddComponent<TextMesh>()
            .SetColor(Color.Violet)
            .SetWordWrap(true)
            .SetFont(EngineContent.LoadContent<SpriteFont>("Common/TwilightSpeechFont21"))
            .SetText("Hi there! We do need some help here, can you help us?")
            .SetWidth(200)
            .SetOffset(new(-100, -110))
            .AddComponent<InputTrigger>()
            .SetTriggerSize(EGame.ViewportCenter)
            .SetCenterOffset(EGame.ViewportCenter - EGame.ViewportSize * .3f)
            .AddComponent<PoniesTalking>()
            .AddComponent<Sprite>()
            .SetTexture(EngineContent.LoadSvg(
                "Common/SpeechCloud",
                new Vector2(365, 365) * EGame.ResolutionCoefficient)
            )
            .GameObject;

    private static GameObject HelperText =>
        new GameObject("HelperText")
            .SetActive(false)
            .AddComponent<Sprite>()
            .SetTexture(EngineContent.LoadSvg("Level1/TextHelper",
                new Vector2(687, 160) * EGame.ResolutionCoefficient))
            .SetTextureColor(Color.White * .8f)
            .Transform.SetPosition(EGame.ViewportCenter + new Vector2(0, -200))
            .AddComponent<MovingText>()
            .SetShouldDestroy(text => text.Transform.Position.Y < -text.Sprite.Height)
            .SetColorChangeSpeed(.95f)
            .SetMovingSpeed(.2f)
            .GameObject;
}
