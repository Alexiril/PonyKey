using System;
using System.Collections.Generic;
using Game.BuiltInComponents;
using Microsoft.Xna.Framework;

namespace Game.BaseTypes;

internal class Component
{
    internal GameObject GameObject
    {
        get => _gameObject;
        set
        {
            _gameObject = value;
            if (_gameObject == null) return;
            Requirements.ForEach(t => { if (!_gameObject.HasComponent(t)) _gameObject.AddComponent(t); });
            Initiate();
        }
    }

    internal bool Active { get; set; } = true;

    internal virtual void Start() {}

    internal virtual void Update() {}

    internal virtual void Draw() {}

    internal void Print(string information) => GameObject.Print(information);

    internal T GetComponent<T>() where T : Component => GameObject.GetComponent<T>();

    internal Transform Transform => GameObject.Transform;

    internal Sprite Sprite => GameObject.Sprite;

    internal Scene ActualScene => GameObject.ActualScene;

    internal InternalGame ActualGame => GameObject.ActualGame;

    internal GameTime ActualGameTime => ActualGame.ActualGameTime;

    internal SceneManager SceneManager => ActualGame.SceneManager;

    internal T AddComponent<T>() where T : Component, new() => GameObject.AddComponent<T>();

    protected virtual List<Type> Requirements => new();

    protected virtual void Initiate() {}

    private GameObject _gameObject;
}
