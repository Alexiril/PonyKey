using Engine.BaseComponents;
using Engine.BaseSystems;
using Engine.BaseTypes;
using Game.Components.Common;
using Game.Components.Level0;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using EGame = Engine.BaseSystems.Game;

namespace Game.Levels;

internal class Level0 : ILevel
{
    public Scene GetScene() =>
        new Scene("Level0")
            .SetBackgroundColor(Color.DeepSkyBlue)
            .AddGameObject(new("Background"))
            .AddComponent<Sprite>()
            .SetTexture(SvgConverter.LoadSvg("Level0/Background0", EGame.ViewportSize))
            .Transform.SetPosition(EGame.ViewportCenter)
            .AddComponent<SoundSource>()
            .SetSound(ArchivedContent.LoadContent<SoundEffect>("Level0/BackgroundMusicLevel0"))
            .SetIsLooped(true)
            .SetVolume(float.TryParse(PlayerSettings.GetValue("vl"), out var value) ? value : 1)
            .SetPlayAtStart(true)
            .AddGameObject(new("TreesGenerator"))
            .SetActive(false)
            .Transform.SetPosition(new(EGame.ViewportSize.X * 1.5f, EGame.ViewportSize.Y*.7f))
            .AddComponent<TreesGenerator>()
            .AddGameObject(new("Background1"))
            .AddComponent<Sprite>()
            .SetTexture(SvgConverter.LoadSvg("Level0/Background1",
                new Vector2(2560, 720) * EGame.ResolutionCoefficient))
            .Transform.SetPosition(new(EGame.ViewportSize.X, EGame.ViewportCenter.Y))
            .AddGameObject(new GameObject("AJRunning"))
            .AddComponent<Animator>()
            .SetAnimationInformation(SvgConverter.LoadSvgAnimation( "Level0/ajAnimation",
                new Vector2(512) * EGame.ResolutionCoefficient))
            .SetPlaying(true)
            .SetLoop(true)
            .Transform.SetPosition(new(-EGame.ViewportSize.X * .2f, EGame.ViewportSize.Y * .75f))
            .AddComponent<ApplejackRunning>()
            .AddGameObject(new("Background2"))
            .AddComponent<Sprite>()
            .SetTexture(SvgConverter.LoadSvg("Level0/Background2",
                new Vector2(2560, 720) * EGame.ResolutionCoefficient))
            .Transform.SetPosition(new(EGame.ViewportSize.X, EGame.ViewportCenter.Y))
            .AddGameObject(new("PonyTalking"))
            .AddComponent<Sprite>()
            .SetTexture(SvgConverter.LoadSvg("Common/TwilightUnhappy",
                new Vector2(600, 600) * EGame.ResolutionCoefficient))
            .Transform.SetPosition(new(EGame.ViewportSize.X * .66f, EGame.ViewportCenter.Y))
            .AddGameObject(new("SpeechCloud"))
            .AddComponent<Sprite>()
            .SetTexture(SvgConverter.LoadSvg("Common/SpeechCloud",
                new Vector2(365, 365) * EGame.ResolutionCoefficient))
            .Transform.SetPosition(new(EGame.ViewportSize.X * .3f, EGame.ViewportSize.Y * .3f))
            .AddComponent<TextMesh>()
            .SetColor(Color.Violet)
            .SetWordWrap(true)
            .SetFont(ArchivedContent.LoadContent<SpriteFont>( "Common/TwilightSpeechFont21"))
            .SetText("Hi there! We do need some help here, can you help us?")
            .SetWidth(200)
            .SetOffset(new(-100, -110))
            .AddComponent<InputTrigger>()
            .SetTriggerSize(EGame.ViewportCenter)
            .SetCenterOffset(EGame.ViewportCenter -
                             new Vector2(EGame.ViewportSize.X * .3f, EGame.ViewportSize.Y * .3f))
            .AddComponent<PoniesTalking>()
            .AddGameObject(new("HelperText"))
            .SetActive(false)
            .AddComponent<Sprite>()
            .SetTexture(SvgConverter.LoadSvg("Level0/TextHelper",
                new Vector2(687, 160) * EGame.ResolutionCoefficient))
            .SetTextureColor(Color.White * .8f)
            .Transform.SetPosition(EGame.ViewportCenter + new Vector2(0, -200))
            .AddComponent<MovingText>()
            .SetShouldDestroy(text => text.Transform.Position.Y < -text.Sprite.Height)
            .SetColorChangeSpeed(.95f)
            .SetMovingSpeed(.2f)
            .ActualScene;
}
