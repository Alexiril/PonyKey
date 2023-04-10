using System;
using System.Collections.Generic;
using Game.BaseTypes;
using Game.Levels;

namespace Game;

internal class SceneManager
{
    internal SceneManager(InternalGame actualGame)
    {
        actualGame.OnAfterUpdate += () =>
        {
            if (_requestedLoad != -1)
            {
                DestroyScene(CurrentScene);
                CurrentScene = _levels[_requestedLoad].GetScene(actualGame);
                CurrentSceneIndex = _requestedLoad;
                _requestedLoad = -1;
            }
        };
    }

    internal Scene CurrentScene { get; private set; }

    internal int CurrentSceneIndex { get; private set; } = -1;

    internal void LoadScene(int index)
    {
        _requestedLoad = index;
    }

    private int _requestedLoad = -1;

    private readonly List<ILevel> _levels = new()
    {
        new MainMenu(),
        new Level0()
    };

    private void DestroyScene(Scene scene)
    {
        if (scene == null)
            return;
        var tempRef = new WeakReference(scene);
        scene.Remove();
        GC.Collect(GC.GetGeneration(tempRef));
    }
}
