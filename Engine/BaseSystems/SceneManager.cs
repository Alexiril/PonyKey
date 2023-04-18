using System;
using System.Collections.Generic;
using Engine.BaseTypes;

namespace Engine.BaseSystems;

public class SceneManager
{
    public void RegisterLevels(List<ILevel> levels) => _levels.AddRange(levels);

    internal SceneManager(ActualGame actualGame)
    {
        actualGame.OnAfterUpdate += () =>
        {
            if (_requestedLoad == -1) return;
            CurrentScene?.Unload();
            DestroyScene(CurrentScene);
            CurrentScene = _levels[_requestedLoad].GetScene(actualGame);
            _noDestroyObjects.ForEach(o => CurrentScene.AddGameObject(o));
            CurrentScene.Start();
            CurrentSceneIndex = _requestedLoad;
            _requestedLoad = -1;
        };
    }

    public Scene CurrentScene { get; private set; }

    public int CurrentSceneIndex { get; private set; } = -1;

    public void LoadScene(int index)
    {
        _requestedLoad = index;
    }

    public void DontDestroyOnLoad(GameObject gameObject)
    {
        if (!_noDestroyObjects.Contains(gameObject))
            _noDestroyObjects.Add(gameObject);
    }

    public void DestroyOnLoad(GameObject gameObject) => _noDestroyObjects.Remove(gameObject);

    private int _requestedLoad = -1;

    private readonly List<GameObject> _noDestroyObjects = new();

    private readonly List<ILevel> _levels = new();

    private static void DestroyScene(Scene scene)
    {
        if (scene == null)
            return;
        var tempRef = new WeakReference(scene);
        scene.Remove();
        GC.Collect(GC.GetGeneration(tempRef));
    }
}
