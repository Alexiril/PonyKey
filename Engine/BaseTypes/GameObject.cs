using System;
using System.Collections.Generic;
using System.Linq;
using Engine.BaseComponents;
using Engine.BaseSystems;

namespace Engine.BaseTypes;

public class GameObject
{
    public string ObjectName { get; set; }

    public bool Active
    {
        get => _active;
        private set
        {
            _active = value;
            if (!_started)
                Start();
        }
    }

    public Scene ActualScene { get; set; }

    public Transform Transform => GetComponent<Transform>();

    public GameObject SetActive(bool active)
    {
        Active = active;
        return this;
    }

    public GameObject(string objectName) => ObjectName = objectName;

    public GameObject(GameObject gameObject)
    {
        ObjectName = gameObject.ObjectName;
        foreach (var component in gameObject._components)
            AddComponent((Component)Activator.CreateInstance(component.GetType(), component));
    }

    // ReSharper disable once InconsistentNaming
    public GameObject gameObject => this;

    public GameObject AddGameObject(GameObject obj) => ActualScene.AddGameObject(obj);

    public int GetIndexInScene() => ActualScene.GetGameObjectIndex(this);

    public T AddComponent<T>() where T : Component, new() => (T)AddComponent(new T());

    public Component AddComponent(Type T) => AddComponent((Component)Activator.CreateInstance(T));

    public T GetComponent<T>() where T : Component
    {
        foreach (var component in _components)
            if (component.GetType() == typeof(T))
                return (T)component;
        return null;
    }

    public Sprite Sprite => GetComponent<Sprite>();

    public void DestroyComponent(Component component) => _removingComponents.Add(component);

    public bool HasComponent<T>() => _components.Exists(x => x.GetType() == typeof(T));

    public bool HasComponent(Type T) => _components.Exists(x => x.GetType() == T);

    internal Component[] GetAllComponents() => _components.ToArray();

    public List<GameObject> Find(string name) => ActualScene.FindGameObjects(name);

    public GameObject GetGameObjectByIndex(int index) => ActualScene.GetGameObject(index);

    internal void Start()
    {
        if (!Active) return;
        foreach (var component in _components)
            component.Start();
        _started = true;
    }

    internal void Update()
    {
        if (Active)
            foreach (var component in _components.Where(component => component.Active))
                component.Update();
        foreach (var component in _removingComponents)
        {
            component.gameObject = null;
            _components.Remove(component);
        }
    }

    internal void Draw()
    {
        if (!Active) return;
        foreach (var component in _components.Where(component => component.Active))
            component.Draw();
    }

    internal void Unload()
    {
        foreach (var component in _components)
            component.Unload();
    }

    public void Destroy() => ActualScene.DestroyGameObject(this);

    internal void Remove()
    {
        foreach (var component in _components.ToList())
        {
            component.gameObject = null;
            _components.Remove(component);
        }
        _components.Clear();
        ActualScene = null;
    }

    internal void Print(string information)
    {
#if DEBUG
        ActualScene.Print(information);
#endif
    }

    private readonly List<Component> _components = new();

    private readonly List<Component> _removingComponents = new();

    private bool _started;
    private bool _active = true;

    private Component AddComponent(Component component)
    {
        if (component == null)
            throw new EngineException("The component was null.");
        if (_components.Exists(x => x == component)) return component;
        component.gameObject = this;
        _components.Add(component);
        return component;
    }
}
