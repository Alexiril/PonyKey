﻿using System.Globalization;
using Engine.BaseComponents;
using Engine.BaseSystems;
using Microsoft.Xna.Framework.Graphics;

namespace Game.Components.MainMenu;

internal class SoundButton : SpriteButton
{
    public SoundButton SetStandardButtonTexture(Texture2D texture)
    {
        _standardButton = texture;
        return this;
    }

    public SoundButton SetTurnedOffButtonTexture(Texture2D texture)
    {
        _turnedOffButton = texture;
        return this;
    }

    public override void Start()
    {
        SetOnPointerUp(_ => ChangeVolume());
        SetCorrectTexture();
        base.Start();
    }

    private void ChangeVolume()
    {
        var soundSource = GameObject.GetGameObjectByIndex(1).GetComponent<SoundSource>();
        if (soundSource.SoundInstance.Volume < float.Epsilon)
        {
            soundSource.SetVolume(_initialVolume);
            PlayerSettings.SetValue("vl", _initialVolume.ToString(CultureInfo.InvariantCulture));
        }
        else
        {
            _initialVolume = soundSource.SoundInstance.Volume > 0 ? soundSource.SoundInstance.Volume : 1;
            soundSource.SetVolume(0);
            PlayerSettings.SetValue("vl", "0");
        }

        SetCorrectTexture();
    }

    private void SetCorrectTexture()
    {
        var soundSource = GameObject.GetGameObjectByIndex(1).GetComponent<SoundSource>();
        Sprite.Texture = soundSource.SoundInstance.Volume > float.Epsilon ? _standardButton : _turnedOffButton;
    }

    private Texture2D _standardButton;

    private Texture2D _turnedOffButton;

    private float _initialVolume = 1;
}
