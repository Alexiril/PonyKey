using System;
using System.Collections.Generic;
using Engine.BaseComponents;
using Microsoft.Xna.Framework;
using Game = Engine.BaseSystems.Game;

namespace Engine.BaseTypes;

public class Component : ICloneable
{
    public Component() {}

    public Component(Component component) => gameObject = component.gameObject;

    // ReSharper disable once InconsistentNaming
    public GameObject gameObject
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

    public void Print(string information) => gameObject.Print(information);

    public T GetComponent<T>() where T : Component => gameObject.GetComponent<T>();

    public Transform Transform => gameObject.Transform;

    public Sprite Sprite => gameObject.Sprite;

    public Scene ActualScene => gameObject.ActualScene;

    public static GameTime GameTime => Game.GameTime;

    public T AddComponent<T>() where T : Component, new() => gameObject.AddComponent<T>();

    public GameObject AddGameObject(GameObject obj) => ActualScene.AddGameObject(obj);

    public GameObject Instantiate(GameObject obj, int index = -1) =>
        ActualScene.Instantiate(obj, index);

    public object Clone() => new Component(this);

    public float DeltaTime => (float)GameTime.ElapsedGameTime.TotalMilliseconds;

    protected virtual List<Type> Requirements => new();

    protected virtual void Initiate() {}

    private GameObject _gameObject;

}
