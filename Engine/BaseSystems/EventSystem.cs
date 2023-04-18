using System;
using System.Collections.Generic;
using System.Linq;
using Engine.BaseTypes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Engine.BaseSystems;

public static class EventSystem
{
    internal static void Update()
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            OnExit?.Invoke();
#if DEBUG
        if (Keyboard.GetState().IsKeyDown(Keys.F1))
            OnToggleDebugInformation?.Invoke();
        if (Keyboard.GetState().IsKeyDown(Keys.F2))
            OnToggleDebugBoxes?.Invoke();
#endif
        OnUpdate?.Invoke();
        var index = 0;
        Events
            .Where(action => action.Value.Item1.Invoke())
            .ToList()
            .ForEach(e =>
            {
                e.Value.Item2?.Invoke(index);
                index++;
            });
        OnceTimeEvents.ForEach(i =>
        {
            Events.Remove(i);
            RegisteredGameObjects.Remove(i);
        });
        OnceTimeEvents.Clear();
        RemovingEvents.ForEach(i =>
        {
            Events.Remove(i);
            RegisteredGameObjects.Remove(i);
        });
        RemovingEvents.Clear();
    }

    public static event Action OnExit;

#if DEBUG
    internal static event Action OnToggleDebugInformation;

    internal static event Action OnToggleDebugBoxes;
#endif

    public static event Action OnUpdate;

    public static int AddEvent(Func<bool> request, Action<int> action, GameObject gameObject = null)
    {
        Events[_eventCode + 1] = (request, action);
        if (gameObject != null) RegisteredGameObjects[_eventCode] = gameObject;
        _eventCode += 1;
        return _eventCode;
    }

    public static int AddOnceTimeEvent(Func<bool> request, Action<int> action, GameObject gameObject = null)
    {
        var newEventCode = AddEvent(request, action);
        if (gameObject != null) RegisteredGameObjects[newEventCode] = gameObject;
        OnceTimeEvents.Add(newEventCode);
        return newEventCode;
    }

    public static void RemoveEvent(int code) => RemovingEvents.Add(code);

    public static void RemoveEventByGameObject(GameObject gameObject)
    {
        if (!RegisteredGameObjects.ContainsValue(gameObject)) return;
        foreach (var value in RegisteredGameObjects.Where(value => gameObject == value.Value))
            RemoveEvent(value.Key);
    }

    private static readonly Dictionary<int, (Func<bool>, Action<int>)> Events = new();

    private static readonly Dictionary<int, GameObject> RegisteredGameObjects = new();

    private static readonly List<int> RemovingEvents = new();

    private static readonly List<int> OnceTimeEvents = new();

    private static int _eventCode;
}
