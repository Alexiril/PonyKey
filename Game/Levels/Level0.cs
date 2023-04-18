using Engine.BaseComponents;
using Engine.BaseSystems;
using Engine.BaseTypes;
using Game.Components.Common;
using Game.Components.Level0;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace Game.Levels;

internal class Level0 : ILevel
{
    public Scene GetScene(ActualGame actualGame) =>
        new Scene(actualGame)
            .SetBackgroundColor(Color.SkyBlue)
            .AddGameObject(new("Background"))
            .AddComponent<Sprite>()
            .SetTexture(actualGame.LoadSvg("Level0/Background0", actualGame.ViewportSize))
            .Transform.SetPosition(actualGame.ViewportCenter)
            .AddComponent<SoundSource>()
            .SetSound(actualGame.Content.Load<SoundEffect>("Level0/BackgroundMusic"))
            .SetIsLooped(true)
            .SetVolume(1)
            .SetPlayAtStart(true)
            .ActualScene
            .AddGameObject(new("Background1"))
            .AddComponent<Sprite>()
            .SetTexture(actualGame.LoadSvg("Level0/Background1",
                new Vector2(2560, 720) * actualGame.ResolutionCoefficient))
            .Transform.SetPosition(new(actualGame.ViewportSize.X, actualGame.ViewportCenter.Y))
            .ActualScene
            .AddGameObject(new("Background2"))
            .AddComponent<Sprite>()
            .SetTexture(actualGame.LoadSvg("Level0/Background2",
                new Vector2(2560, 720) * actualGame.ResolutionCoefficient))
            .Transform.SetPosition(new(actualGame.ViewportSize.X, actualGame.ViewportCenter.Y))
            .ActualScene
            .AddGameObject(new("PonyTalking"))
            .AddComponent<Sprite>()
            .SetTexture(actualGame.LoadSvg("Common/TwilightUnhappy",
                new Vector2(600, 600) * actualGame.ResolutionCoefficient))
            .Transform.SetPosition(new(actualGame.ViewportSize.X * .66f, actualGame.ViewportCenter.Y))
            .ActualScene
            .AddGameObject(new("SpeechCloud"))
            .AddComponent<Sprite>()
            .SetTexture(actualGame.LoadSvg("Common/SpeechCloud",
                new Vector2(365, 365) * actualGame.ResolutionCoefficient))
            .Transform.SetPosition(new(actualGame.ViewportSize.X * .3f, actualGame.ViewportSize.Y * .3f))
            .AddComponent<TextMesh>()
            .SetColor(Color.Violet)
            .SetWordWrap(true)
            .SetFont(actualGame.Content.Load<SpriteFont>("Common/TwilightSpeechFont21"))
            .SetText("Hi there! We do need some help here, can you help us?")
            .SetWidth(200)
            .SetOffset(new(-100, -110))
            .AddComponent<InputTrigger>()
            .SetTriggerSize(actualGame.ViewportCenter)
            .SetCenterOffset(actualGame.ViewportCenter -
                             new Vector2(actualGame.ViewportSize.X * .3f, actualGame.ViewportSize.Y * .3f))
            .AddComponent<PoniesTalking>()
            .ActualScene
            .AddGameObject(new("HelperText"))
            .SetActive(false)
            .AddComponent<Sprite>()
            .SetTexture(actualGame.LoadSvg("Level0/TextHelper",
                new Vector2(687, 160) * actualGame.ResolutionCoefficient))
            .SetTextureColor(Color.White * .8f)
            .Transform.SetPosition(actualGame.ViewportCenter + new Vector2(0, -200))
            .AddComponent<MovingText>()
            .SetShouldDestroy(text => text.Transform.Position.Y < -text.Sprite.Height)
            .SetColorChangeSpeed(.95f)
            .SetMovingSpeed(.2f)
            .ActualScene;
}
