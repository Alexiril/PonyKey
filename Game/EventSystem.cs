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
        Events
            .Where(action => action.Value.Item1.Invoke())
            .ToList()
            .ForEach(e => e.Value.Item2?.Invoke());
    }

    internal static event Action OnExit;

    internal static event Action OnUpdate;

    internal static int AddEvent(Func<bool> request, Action action)
    {
        Events[_eventCode + 1] = (request, action);
        _eventCode += 1;
        return _eventCode;
    }

    internal static bool RemoveEvent(int code) => Events.Remove(code);

    private static readonly Dictionary<int, (Func<bool>, Action)> Events = new();

    private static int _eventCode;
}
