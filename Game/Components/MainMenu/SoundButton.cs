using System.Globalization;
using Engine.BaseComponents;
using Engine.BaseSystems;
using Microsoft.Xna.Framework.Graphics;

namespace Game.Components.MainMenu;

internal class SoundButton : SpriteButton
{
    internal SoundButton SetStandardButtonTexture(Texture2D texture)
    {
        _standardButton = texture;
        return this;
    }

    internal SoundButton SetTurnedOffButtonTexture(Texture2D texture)
    {
        _turnedOffButton = texture;
        return this;
    }

    public override void Start()
    {
        base.Start();
        var soundSource = GameObject.GetGameObjectByIndex(1).GetComponent<SoundSource>();
        SetOnPointerUp(_ =>
            {
                if (soundSource.Sound.Volume < float.Epsilon)
                {
                    soundSource.SetVolume(_initialVolume);
                    PlayerSettings.SetValue("vl",_initialVolume.ToString(CultureInfo.InvariantCulture));
                    Sprite.Texture = _standardButton;
                }
                else
                {
                    _initialVolume = soundSource.Sound.Volume > 0 ? soundSource.Sound.Volume : 1;
                    soundSource.SetVolume(0);
                    PlayerSettings.SetValue("vl", "0");
                    Sprite.Texture = _turnedOffButton;
                }
            }
        );
    }

    private Texture2D _standardButton;

    private Texture2D _turnedOffButton;

    private float _initialVolume = 1;
}
