using System;
using System.Collections.Generic;
using Engine.BaseComponents;
using Microsoft.Xna.Framework;
using Game = Engine.BaseSystems.Game;

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

    public static GameTime GameTime => Game.GameTime;

    public T AddComponent<T>() where T : Component, new() => GameObject.AddComponent<T>();

    public GameObject AddGameObject(GameObject gameObject) => ActualScene.AddGameObject(gameObject);

    public GameObject Instantiate(GameObject gameObject, int index = -1) =>
        ActualScene.Instantiate(gameObject, index);

    public object Clone() => new Component(this);

    public static float DeltaTime => (float)GameTime.ElapsedGameTime.TotalMilliseconds;

    protected virtual List<Type> Requirements => new();

    protected virtual void Initiate() {}

    private GameObject _gameObject;

}
