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
        _logoSprite = GameObject.GetComponent<Sprite>();
        _startMilliseconds = GameObject.ActualGame.ActualGameTime.TotalGameTime.TotalMilliseconds;
    }

    internal override void Update()
    {
        if (GameObject.ActualGame.ActualGameTime.TotalGameTime.TotalMilliseconds - _startMilliseconds > 2500)
        {
            GameObject.Transform.Position +=
                (float)GameObject.ActualGame.ActualGameTime.ElapsedGameTime.TotalMilliseconds *
                .5f * GameObject.Transform.GlobalUp;
            _logoSprite.TextureColor *= .99f;
        }
        else
        {
            _logoSprite.TextureColor = Color.White *
                MathF.Min(1f, (float)(GameObject.ActualGame.ActualGameTime.TotalGameTime.TotalMilliseconds - _startMilliseconds) / 1000);
        }
        if (GameObject.Transform.Position.Y < -GameObject.GetComponent<Sprite>().Height)
            GameObject.Destroy();
    }
}
