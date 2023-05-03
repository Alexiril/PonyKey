﻿using Engine.BaseComponents;
using Engine.BaseSystems;

namespace Game.Components.MainMenu;

internal class PlayButton : SpriteButton
{
    public override void Start()
    {
        base.Start();
        SetOnPointerUp(_ =>
            {
                _startingNextScene = true;
                GetComponent<InputTrigger>().Active = false;
                _timeFromClick = GameTime.TotalGameTime.TotalMilliseconds;
            }
        );
    }

    public override void Update()
    {
        if (!_startingNextScene) return;
        if (GameTime.TotalGameTime.TotalMilliseconds - _timeFromClick < 1500)
        {
            for (var i = 1; i < ActualScene.GameObjectsCount; i++)
            {
                var obj = gameObject.GetGameObjectByIndex(i);
                var sprite = obj.Sprite;
                if (sprite != null) sprite.TextureColor *= .95f;
                if (obj.HasComponent<InputTrigger>())
                    obj.DestroyComponent(obj.GetComponent<InputTrigger>());
            }
        }
        else
        {
            SceneManager.LoadSceneAsync(2).ConfigureAwait(false);
            _startingNextScene = false;
        }
    }

    private bool _startingNextScene;
    private double _timeFromClick;
}
