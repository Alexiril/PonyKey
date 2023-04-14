using System;
using Game.BaseTypes;
using Game.BuiltInComponents;
using Microsoft.Xna.Framework;

namespace Game.Components.MainMenu;

internal class MoveLogoEntrance : Component
{
    internal override void Start()
    {
        _logoSprite = Sprite;
        _startMilliseconds = ActualGameTime.TotalGameTime.TotalMilliseconds;
    }

    internal override void Update()
    {
        if (ActualGameTime.TotalGameTime.TotalMilliseconds - _startMilliseconds > 2500)
        {
            Transform.Position += (float)ActualGameTime.ElapsedGameTime.TotalMilliseconds * .5f * Transform.GlobalUp;
            _logoSprite.TextureColor *= .99f;
        }
        else _logoSprite.TextureColor =
            Color.White * MathF.Min(1f, (float)(ActualGameTime.TotalGameTime.TotalMilliseconds - _startMilliseconds) / 1000);
        if (Transform.Position.Y < -Sprite.Height) GameObject.Destroy();
    }

    private Sprite _logoSprite;

    private double _startMilliseconds;
}
