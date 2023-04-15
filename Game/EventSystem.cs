using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Game;

internal static class EventSystem
{
    internal static void Update()
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            OnExit?.Invoke();
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
        OnceTimeEvents.ForEach(i => Events.Remove(i));
        OnceTimeEvents.Clear();
        RemovingEvents.ForEach(i => Events.Remove(i));
        RemovingEvents.Clear();
    }

    internal static event Action OnExit;

    internal static event Action OnUpdate;

    internal static int AddEvent(Func<bool> request, Action<int> action)
    {
        Events[_eventCode + 1] = (request, action);
        _eventCode += 1;
        return _eventCode;
    }

    internal static int AddOnceTimeEvent(Func<bool> request, Action<int> action)
    {
        var newEventCode = AddEvent(request, action);
        OnceTimeEvents.Add(newEventCode);
        return newEventCode;
    }

    internal static void RemoveEvent(int code) => RemovingEvents.Add(code);

    private static readonly Dictionary<int, (Func<bool>, Action<int>)> Events = new();

    private static readonly List<int> RemovingEvents = new();

    private static readonly List<int> OnceTimeEvents = new();

    private static int _eventCode;
}
