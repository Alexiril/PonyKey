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
    public Scene GetScene(Master master) =>
        new Scene(master, "Level0")
            .SetBackgroundColor(Color.DeepSkyBlue)
            .AddGameObject(new("Background"))
            .AddComponent<Sprite>()
            .SetTexture(master.LoadSvg("Level0/Background0", master.ViewportSize))
            .Transform.SetPosition(master.ViewportCenter)
            .AddComponent<SoundSource>()
            .SetSound(master.LoadContent<SoundEffect>("Level0/BackgroundMusic"))
            .SetIsLooped(true)
            .SetVolume(1)
            .SetPlayAtStart(true)
            .AddGameObject(new("Background1"))
            .AddComponent<Sprite>()
            .SetTexture(master.LoadSvg("Level0/Background1",
                new Vector2(2560, 720) * master.ResolutionCoefficient))
            .Transform.SetPosition(new(master.ViewportSize.X, master.ViewportCenter.Y))
            .AddGameObject(new("Background2"))
            .AddComponent<Sprite>()
            .SetTexture(master.LoadSvg("Level0/Background2",
                new Vector2(2560, 720) * master.ResolutionCoefficient))
            .Transform.SetPosition(new(master.ViewportSize.X, master.ViewportCenter.Y))
            .AddGameObject(new("PonyTalking"))
            .AddComponent<Sprite>()
            .SetTexture(master.LoadSvg("Common/TwilightUnhappy",
                new Vector2(600, 600) * master.ResolutionCoefficient))
            .Transform.SetPosition(new(master.ViewportSize.X * .66f, master.ViewportCenter.Y))
            .AddGameObject(new("SpeechCloud"))
            .AddComponent<Sprite>()
            .SetTexture(master.LoadSvg("Common/SpeechCloud",
                new Vector2(365, 365) * master.ResolutionCoefficient))
            .Transform.SetPosition(new(master.ViewportSize.X * .3f, master.ViewportSize.Y * .3f))
            .AddComponent<TextMesh>()
            .SetColor(Color.Violet)
            .SetWordWrap(true)
            .SetFont(master.LoadContent<SpriteFont>("Common/TwilightSpeechFont21"))
            .SetText("Hi there! We do need some help here, can you help us?")
            .SetWidth(200)
            .SetOffset(new(-100, -110))
            .AddComponent<InputTrigger>()
            .SetTriggerSize(master.ViewportCenter)
            .SetCenterOffset(master.ViewportCenter -
                             new Vector2(master.ViewportSize.X * .3f, master.ViewportSize.Y * .3f))
            .AddComponent<PoniesTalking>()
            .AddGameObject(new("HelperText"))
            .SetActive(false)
            .AddComponent<Sprite>()
            .SetTexture(master.LoadSvg("Level0/TextHelper",
                new Vector2(687, 160) * master.ResolutionCoefficient))
            .SetTextureColor(Color.White * .8f)
            .Transform.SetPosition(master.ViewportCenter + new Vector2(0, -200))
            .AddComponent<MovingText>()
            .SetShouldDestroy(text => text.Transform.Position.Y < -text.Sprite.Height)
            .SetColorChangeSpeed(.95f)
            .SetMovingSpeed(.2f)
            .ActualScene;
}
