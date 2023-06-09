﻿using System;
using System.Collections.Generic;
using Engine.BaseComponents;
using Engine.BaseTypes;
using Microsoft.Xna.Framework;

namespace Game.Components.Common;

internal class MovingText : Component
{
    public float WaitingTime { get; set; } = 2500;

    public float ShowingTime { get; set; } = 1000;

    public float MovingSpeed { get; set; } = .5f;

    public float ColorChangeSpeed { get; set; } = .99f;

    public Vector2 MovingDirection { get; set; } = -Vector2.UnitY;

    public Func<MovingText, bool> ShouldDestroy { get; set; } = _ => false;

    public Func<MovingText, bool> ShouldStop { get; set; } = _ => false;

    public MovingText SetWaitingTime(float time)
    {
        WaitingTime = time;
        return this;
    }

    public MovingText SetShowingTime(float time)
    {
        ShowingTime = time;
        return this;
    }

    public MovingText SetMovingSpeed(float speed)
    {
        MovingSpeed = speed;
        return this;
    }

    public MovingText SetColorChangeSpeed(float speed)
    {
        ColorChangeSpeed = speed;
        return this;
    }

    public MovingText SetMovingDirection(Vector2 direction)
    {
        MovingDirection = direction;
        return this;
    }

    public MovingText SetShouldDestroy(Func<MovingText, bool> shouldDestroy)
    {
        ShouldDestroy = shouldDestroy;
        return this;
    }

    public MovingText SetShouldStop(Func<MovingText, bool> shouldStop)
    {
        ShouldStop = shouldStop;
        return this;
    }

    public override void Start()
    {
        _actualSprite = Sprite;
        _transparency = (float)_actualSprite.TextureColor.A / 256;
        _startMilliseconds = GameTime.TotalGameTime.TotalMilliseconds;
    }

    public override void Update()
    {
        if (GameTime.TotalGameTime.TotalMilliseconds - _startMilliseconds > WaitingTime && _work)
        {
            Transform.Position += DeltaTime * MovingSpeed * MovingDirection;
            _actualSprite.TextureColor *= ColorChangeSpeed;
        }
        else
            _actualSprite.TextureColor =
                Color.White * MathF.Min(_transparency, (float)(GameTime.TotalGameTime.TotalMilliseconds -
                _startMilliseconds) / ShowingTime);
        if (ShouldDestroy.Invoke(this)) GameObject.Destroy();
        if (ShouldStop.Invoke(this)) _work = false;
    }

    protected override List<Type> Requirements => new() { typeof(Sprite) };

    private Sprite _actualSprite;
    private float _transparency;
    private double _startMilliseconds;
    private bool _work = true;
}
