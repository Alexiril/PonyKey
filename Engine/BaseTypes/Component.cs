using System;
using System.Collections.Generic;
using Engine.BaseComponents;
using Engine.BaseSystems;
using Microsoft.Xna.Framework;

namespace Engine.BaseTypes;

public class Component : ICloneable
{
    public Component() {}

    public Component(Component component) => GameObject = component.GameObject;

    public GameObject GameObject
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

    public bool Active { get; set; } = true;

    public virtual void Start() {}

    public virtual void Update() {}

    public virtual void Draw() {}

    public virtual void Unload() {}

    public void Print(string information) => GameObject.Print(information);

    public T GetComponent<T>() where T : Component => GameObject.GetComponent<T>();

    public Transform Transform => GameObject.Transform;

    public Sprite Sprite => GameObject.Sprite;

    public Scene ActualScene => GameObject.ActualScene;

    public Master Master => GameObject.Master;

    public GameTime ActualGameTime => Master.ActualGameTime;

    public SceneManager SceneManager => Master.SceneManager;

    public T AddComponent<T>() where T : Component, new() => GameObject.AddComponent<T>();

    public GameObject AddGameObject(GameObject gameObject) => ActualScene.AddGameObject(gameObject);

    protected virtual List<Type> Requirements => new();

    protected virtual void Initiate() {}

    private GameObject _gameObject;
    public object Clone() => new Component(this);
}
