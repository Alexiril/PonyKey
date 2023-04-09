using System;
using System.Collections.Generic;
using Game.BuiltInComponents;

namespace Game.BaseTypes;

public class GameObject
{
    public string ObjectName { get; set; }

    public bool Active { get; private set; }

    public readonly InternalGame ActualGame;

    public Transform Transform { get; }

    private readonly List<Component> _components;

    public GameObject SetActive(bool active)
    {
        Active = active;
        return this;
    }

    internal GameObject(string objectName, InternalGame actualGame)
    {
        ObjectName = objectName;
        ActualGame = actualGame;
        Active = true;
        _components = new();
        Transform = AddComponent<Transform>();
    }

    internal T AddComponent<T>() where T : Component, new() => new T { GameObject = this };

    internal void AddComponent(Component component)
    {
        if (component != null)
        {
            if (!_components.Exists(x => x == component))
                _components.Add(component);
            else
                throw new NullReferenceException("Component is in the game object already.");
        }
        else
            throw new NullReferenceException("Component is null.");
    }

    internal T GetComponents<T>() where T : Component
    {
        foreach (var component in _components)
            if (component.GetType() == typeof(T))
                return (T)component;
        return null;
    }

    internal bool RemoveComponent(Component component) => _components.Remove(component);

    internal Component[] GetAllComponents() => _components.ToArray();

    public void LoadContent()
    {
        if (Active)
            foreach (var component in _components)
                component.LoadContent();
    }

    public void Update()
    {
        if (Active)
            foreach (var component in _components)
                component.Update();
    }

    public void Draw()
    {
        if (Active)
            foreach (var component in _components)
                component.Draw();
    }
}
