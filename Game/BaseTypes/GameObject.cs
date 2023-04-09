using System;
using System.Collections.Generic;
using Game.BuiltInComponents;

namespace Game.BaseTypes;

internal class GameObject
{
    internal static void Destroy(GameObject gameObject)
    {
        gameObject.Destroy();
    }

    internal string ObjectName { get; set; }

    internal bool Active { get; private set; }

    internal InternalGame ActualGame { get; private set; }

    internal Scene ActualScene { get; set; }

    internal Transform Transform { get; private set; }

    private readonly List<Component> _components;

    internal GameObject SetActive(bool active)
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

    internal void LoadContent()
    {
        if (Active)
            foreach (var component in _components)
                component.LoadContent();
    }

    internal void Update()
    {
        if (Active)
            foreach (var component in _components)
                component.Update();
    }

    internal void Draw()
    {
        if (Active)
            foreach (var component in _components)
                component.Draw();
    }

    internal void Destroy()
    {
        ActualScene.DestroyGameObject(this);
    }

    internal void Remove()
    {
        foreach (var component in _components)
            component.Destroy();
        _components.Clear();
        Transform = null;
        ActualGame = null;
        ActualScene = null;
    }
}
