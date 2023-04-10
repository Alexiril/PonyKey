using System;
using Game.BaseTypes;
using Game.BuiltInComponents;
using Microsoft.Xna.Framework;

namespace Game.Components.MainMenu;

internal class MoveLogoEntrance : Component
{
    private Sprite _logoSprite;

    private double _startMilliseconds;

    internal override void Start()
    {
        _logoSprite = GetComponent<Sprite>();
        _startMilliseconds = ActualGame.ActualGameTime.TotalGameTime.TotalMilliseconds;
    }

    internal override void Update()
    {
        if (ActualGame.ActualGameTime.TotalGameTime.TotalMilliseconds - _startMilliseconds > 2500)
        {
            Transform.Position +=
                (float)ActualGame.ActualGameTime.ElapsedGameTime.TotalMilliseconds *
                .5f * Transform.GlobalUp;
            _logoSprite.TextureColor *= .99f;
        }
        else
        {
            _logoSprite.TextureColor = Color.White *
                MathF.Min(1f, (float)(ActualGame.ActualGameTime.TotalGameTime.TotalMilliseconds - _startMilliseconds) / 1000);
        }
        if (Transform.Position.Y < -GetComponent<Sprite>().Height)
            GameObject.Destroy();
    }
}
