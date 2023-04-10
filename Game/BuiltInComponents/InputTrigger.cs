using System;
using Game.BaseTypes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game.BuiltInComponents;

internal class InputTrigger : Component
{
    internal event Action<MouseState> OnPointerInput;

    internal Vector2 TriggerSize
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

    internal Vector2 CenterOffset
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

    internal InputTrigger SetTriggerSize(Vector2 size)
    {
        TriggerSize = size;
        return this;
    }

    internal InputTrigger SetCenterOffset(Vector2 offset)
    {
        CenterOffset = offset;
        return this;
    }

    internal override void Update()
    {
        var mouseState = Mouse.GetState(GameObject.ActualGame.Window);
        if (
            (mouseState.LeftButton == ButtonState.Pressed ||
            mouseState.RightButton == ButtonState.Pressed ||
            mouseState.MiddleButton == ButtonState.Pressed ||
            mouseState.XButton1 == ButtonState.Pressed ||
            mouseState.XButton2 == ButtonState.Pressed) &&
            mouseState.X < GameObject.Transform.Position.X + CenterOffset.X + TriggerSize.X &&
            mouseState.X > GameObject.Transform.Position.X + CenterOffset.X - TriggerSize.X &&
            mouseState.Y < GameObject.Transform.Position.Y + CenterOffset.Y + TriggerSize.Y &&
            mouseState.Y > GameObject.Transform.Position.Y + CenterOffset.Y - TriggerSize.Y
        )
            OnPointerInput?.Invoke(mouseState);
    }

#if DEBUG
    internal override void Draw() =>
        GameObject.ActualGame.DrawSpace.Draw(
            _debugTexture,
            GameObject.Transform.Position + CenterOffset,
            null,
            Color.White,
            0,
            new Vector2(_debugTexture.Width / 2f, _debugTexture.Height / 2f),
            1,
            SpriteEffects.None,
            GameObject.Transform.LayerDepth
        );

    private Texture2D _debugTexture;

    private void GenerateDebugTexture() =>
        _debugTexture = RuntimeTextureGenerator.GenerateTexture(
            GameObject.ActualGame.GraphicsDevice,
            (int)TriggerSize.X * 2,
            (int)TriggerSize.Y * 2,
            i =>
            {
                if (i < TriggerSize.X * 2 ||
                    i > TriggerSize.Y * TriggerSize.X * 4 - TriggerSize.X * 2 ||
                    i % (int)(TriggerSize.X * 2) == 0 ||
                    i % (int)(TriggerSize.X * 2) == (int)(TriggerSize.X * 2) - 1)
                    return Color.Green;
                return Color.Transparent;
            }
        );
#endif

    private Vector2 _triggerSize = Vector2.One;
    private Vector2 _centerOffset = Vector2.Zero;

}
