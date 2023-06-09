﻿using System;
using Engine.BaseTypes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Game = Engine.BaseSystems.Game;
#if DEBUG
using Engine.BaseSystems;
using Microsoft.Xna.Framework.Graphics;
#endif

namespace Engine.BaseComponents;

public class InputTrigger : Component
{
    public InputTrigger() {}

    public InputTrigger(InputTrigger trigger) : base(trigger)
    {
        OnPointerDown = (Action<MouseState>)trigger.OnPointerDown.Clone();
        OnPointerUp = (Action<MouseState>)trigger.OnPointerUp.Clone();
        OnPointerHolds = (Action<MouseState>)trigger.OnPointerHolds.Clone();
        OnPointerOver = (Action<MouseState>)trigger.OnPointerOver.Clone();
        OnPointerExit = (Action<MouseState>)trigger.OnPointerExit.Clone();
        TriggerSize = trigger.TriggerSize;
        CenterOffset = trigger.CenterOffset;
    }

    public event Action<MouseState> OnPointerDown;
    public event Action<MouseState> OnPointerUp;
    public event Action<MouseState> OnPointerHolds;
    public event Action<MouseState> OnPointerOver;
    public event Action<MouseState> OnPointerExit;

    public Vector2 TriggerSize
    {
        get => _triggerSize;
        set
        {
            _triggerSize = new((int)value.X, (int)value.Y);
#if DEBUG
            GenerateDebugTexture();
#endif
        }
    }

    public Vector2 CenterOffset
    {
        get => _centerOffset;
        set
        {
            _centerOffset = value;
#if DEBUG
            GenerateDebugTexture();
#endif
        }
    }

    public InputTrigger SetTriggerSize(Vector2 size)
    {
        TriggerSize = size;
        return this;
    }

    public InputTrigger SetCenterOffset(Vector2 offset)
    {
        CenterOffset = offset;
        return this;
    }

    public InputTrigger SetTriggerSizeFromSprite()
    {
        TriggerSize = Sprite.Size / 2;
        return this;
    }

    public InputTrigger SetPointerUpAction(Action<MouseState> action)
    {
        OnPointerUp += action;
        return this;
    }

    public override void Update()
    {
        var mouseState = Mouse.GetState(Game.Window);
        if (CheckAnyMouseKeyPressed(mouseState) && CheckPointerOverTrigger(mouseState))
        {
            _pointerWasOverObject = true;
            if (!_pointerWasDown)
            {
                OnPointerDown?.Invoke(mouseState);
                _pointerWasDown = true;
            }
            OnPointerHolds?.Invoke(mouseState);
            _triggerClicked = true;
        }
        else if (_triggerClicked)
        {
            _triggerClicked = _pointerWasDown = false;
            OnPointerExit?.Invoke(mouseState);
            if (!CheckAnyMouseKeyPressed(mouseState))
                OnPointerUp?.Invoke(mouseState);
        }
        else if (CheckPointerOverTrigger(mouseState))
        {
            _pointerWasOverObject = true;
            OnPointerOver?.Invoke(mouseState);
        }
        else if (_pointerWasOverObject)
        {
            OnPointerExit?.Invoke(mouseState);
        }
    }

#if DEBUG
    public override void Draw()
    {
        if (_debugTexture == null || !Game.DebugBoxesOn) return;
        Game.DrawSpace.Begin();
        Game.DrawSpace.Draw(
            _debugTexture,
            Transform.Position + CenterOffset,
            null,
            Color.White,
            0,
            new Vector2(_debugTexture.Width / 2f, _debugTexture.Height / 2f),
            1,
            SpriteEffects.None,
            Transform.LayerDepth
        );
        Game.DrawSpace.End();
    }

    private Texture2D _debugTexture;

    private void GenerateDebugTexture() =>
        _debugTexture = TextureGenerator.GenerateTexture(
            (int)TriggerSize.X * 2,
            (int)TriggerSize.Y * 2,
            (x, y) =>
            {
                if (y == 0 ||
                    y == (int)TriggerSize.Y * 2 - 1 ||
                    x == 1 ||
                    x == (int)TriggerSize.X * 2 - 1)
                    return Color.Green;
                return Color.Transparent;
            }
        );
#endif

    private Vector2 _triggerSize = Vector2.One;
    private Vector2 _centerOffset = Vector2.Zero;
    private bool _triggerClicked;
    private bool _pointerWasDown;
    private bool _pointerWasOverObject;

    private static bool CheckAnyMouseKeyPressed(MouseState mouseState) =>
        mouseState.LeftButton == ButtonState.Pressed ||
        mouseState.RightButton == ButtonState.Pressed ||
        mouseState.MiddleButton == ButtonState.Pressed ||
        mouseState.XButton1 == ButtonState.Pressed ||
        mouseState.XButton2 == ButtonState.Pressed;

    private bool CheckPointerOverTrigger(MouseState mouseState) =>
        mouseState.X < Transform.Position.X + CenterOffset.X + TriggerSize.X &&
        mouseState.X > Transform.Position.X + CenterOffset.X - TriggerSize.X &&
        mouseState.Y < Transform.Position.Y + CenterOffset.Y + TriggerSize.Y &&
        mouseState.Y > Transform.Position.Y + CenterOffset.Y - TriggerSize.Y;

}
