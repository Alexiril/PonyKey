using Game.BuiltInComponents;
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

    internal override void Start()
    {
        base.Start();
        var soundSource = GameObject.GetGameObjectByIndex(0).GetComponent<SoundSource>();
        SetOnPointerUp(_ =>
            {
                if (soundSource.Sound.Volume < float.Epsilon)
                {
                    soundSource.SetVolume(_initialVolume);
                    Sprite.Texture = _standardButton;
                }
                else
                {
                    _initialVolume = soundSource.Sound.Volume;
                    soundSource.SetVolume(0);
                    Sprite.Texture = _turnedOffButton;
                }
            }
        );
    }

    private Texture2D _standardButton;

    private Texture2D _turnedOffButton;

    private float _initialVolume;
}
