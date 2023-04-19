﻿using Engine.BaseComponents;

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
                _timeFromClick = ActualGameTime.TotalGameTime.TotalMilliseconds;
            }
        );
    }

    public override void Update()
    {
        if (!_startingNextScene) return;
        if (ActualGameTime.TotalGameTime.TotalMilliseconds - _timeFromClick < 1500)
        {
            for (var i = 0; i < ActualScene.GameObjectsCount; i++)
            {
                var gameObject = GameObject.GetGameObjectByIndex(i);
                var sprite = gameObject.Sprite;
                if (sprite != null) sprite.TextureColor *= .95f;
                if (gameObject.HasComponent<InputTrigger>())
                    gameObject.DestroyComponent(gameObject.GetComponent<InputTrigger>());
            }
        }
        else SceneManager.LoadScene(1);
    }

    private bool _startingNextScene;
    private double _timeFromClick;
}