using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Engine.BaseTypes;

namespace Engine.BaseSystems;

public class SceneManager
{
    public void RegisterLevels(List<ILevel> levels) => _levels.AddRange(levels);

    internal SceneManager(Master master)
    {
        _master = master;
        master.OnAfterUpdate += () =>
        {
            if (_preLoadedScene == null) return;
            CurrentScene?.Unload();
            DestroyScene(CurrentScene);
            CurrentScene = _preLoadedScene;
            CurrentScene.Start();
            _preLoadedScene = null;
        };
    }

    public Scene CurrentScene { get; private set; }

    public void LoadScene(int index) => ActualSceneLoad(index);

    public async Task LoadSceneAsync(int index) => await Task.Run(() => ActualSceneLoad(index));

    public void DontDestroyOnLoad(GameObject gameObject)
    {
        if (!_noDestroyObjects.Contains(gameObject))
            _noDestroyObjects.Add(gameObject);
    }

    public void DestroyOnLoad(GameObject gameObject) => _noDestroyObjects.Remove(gameObject);

    private readonly List<GameObject> _noDestroyObjects = new();

    private readonly List<ILevel> _levels = new();

    private Scene _preLoadedScene;

    private readonly Master _master;

    private static void DestroyScene(Scene scene)
    {
        if (scene == null)
            return;
        var tempRef = new WeakReference(scene);
        scene.Remove();
        GC.Collect(GC.GetGeneration(tempRef));
    }

    private void ActualSceneLoad(int index)
    {
        _preLoadedScene = _levels[index].GetScene(_master);
        _preLoadedScene.AssemblyIndex = index;
        _noDestroyObjects.ForEach(o => _preLoadedScene.AddGameObject(o));
    }
}
