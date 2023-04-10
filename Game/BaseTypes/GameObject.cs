using System.Collections.Generic;
using System.Linq;
using Game.BuiltInComponents;

namespace Game.BaseTypes;

internal class GameObject
{
    internal string ObjectName { get; set; }

    internal bool Active { get; private set; } = true;

    internal InternalGame ActualGame { get; set; }

    internal Scene ActualScene { get; set; }

    internal Transform Transform { get; private set; }

    internal GameObject SetActive(bool active)
    {
        Active = active;
        return this;
    }

    internal GameObject(string objectName)
    {
        ObjectName = objectName;
        Transform = AddComponent<Transform>();
    }

    internal T AddComponent<T>() where T : Component, new() => (T)AddComponent(new T { GameObject = this });

    internal T GetComponent<T>() where T : Component
    {
        foreach (var component in _components)
            if (component.GetType() == typeof(T))
                return (T)component;
        return null;
    }

    internal void DestroyComponent(Component component) => _removingComponents.Add(component);

    internal Component[] GetAllComponents() => _components.ToArray();

    internal void Start()
    {
        if (Active)
            foreach (var component in _components)
                component.Start();
    }

    internal void Update()
    {
        if (Active)
            foreach (var component in _components)
                component.Update();
        foreach (var component in _removingComponents)
        {
            component.GameObject = null;
            _components.Remove(component);
        }
    }

    internal void Draw()
    {
        if (Active)
            foreach (var component in _components)
                component.Draw();
    }

    internal void Destroy() => ActualScene.DestroyGameObject(this);

    internal void Remove()
    {
        foreach (var component in _components.ToList())
        {
            component.GameObject = null;
            _components.Remove(component);
        }
        _components.Clear();
        Transform = null;
        ActualGame = null;
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

    private Component AddComponent(Component component)
    {
        if (component != null && !_components.Exists(x => x == component))
            _components.Add(component);
        return component;
    }
}
