using System;
using System.Collections.Generic;
using Game.BuiltInComponents;

namespace Game.BaseTypes;

public class GameObject
{
    public string ObjectName;

    public bool Active { get; set; }

    public readonly InternalGame ActualGame;

    public Transform Transform { get; }

    private List<Component> Components { get; }

    internal GameObject(string objectName, InternalGame actualGame)
    {
        ObjectName = objectName;
        ActualGame = actualGame;
        Active = true;
        Components = new();
        Transform = AddComponent<Transform>();
    }

    internal T AddComponent<T>() where T : Component, new() => new T { GameObject = this };

    internal void AddComponent(Component component)
    {
        if (component != null)
        {
            if (!Components.Exists(x => x == component))
                Components.Add(component);
            else
                throw new NullReferenceException("Component is in the game object already.");
        }
        else
            throw new NullReferenceException("Component is null.");
    }

    internal T GetComponents<T>() where T : Component
    {
        foreach (var component in Components)
            if (component.GetType() == typeof(T))
                return (T)component;
        return null;
    }

    internal bool RemoveComponent(Component component) => Components.Remove(component);

    internal Component[] GetAllComponents() => Components.ToArray();

    public void LoadContent()
    {
        if (Active)
            foreach (var component in Components)
                component.LoadContent();
    }

    public void Update()
    {
        if (Active)
            foreach (var component in Components)
                component.Update();
    }

    public void Draw()
    {
        if (Active)
            foreach (var component in Components)
                component.Draw();
    }
}
