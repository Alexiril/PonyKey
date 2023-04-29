using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Engine.BaseTypes;

namespace Engine.BaseSystems;

public static class SceneManager
{
    public static void RegisterLevels(List<ILevel> levels) => Levels.AddRange(levels);

    internal static void Update()
    {
        if (_preLoadedScene == null) return;
        CurrentScene?.Unload();
        DestroyScene(CurrentScene);
        CurrentScene = _preLoadedScene;
        CurrentScene.Start();
        _preLoadedScene = null;
    }

    public static Scene CurrentScene { get; private set; }

    public static void LoadScene(int index) => ActualSceneLoad(index);

    public static async Task LoadSceneAsync(int index) => await Task.Run(() => ActualSceneLoad(index));

    public static void DontDestroyOnLoad(GameObject gameObject)
    {
        if (!NoDestroyObjects.Contains(gameObject))
            NoDestroyObjects.Add(gameObject);
    }

    public static void DestroyOnLoad(GameObject gameObject) => NoDestroyObjects.Remove(gameObject);

    private static readonly List<GameObject> NoDestroyObjects = new();

    private static readonly List<ILevel> Levels = new();

    private static Scene _preLoadedScene;

    private static void DestroyScene(Scene scene)
    {
        if (scene == null)
            return;
        var tempRef = new WeakReference(scene);
        scene.Remove();
        GC.Collect(GC.GetGeneration(tempRef));
    }

    private static void ActualSceneLoad(int index)
    {
        _preLoadedScene = Levels[index].GetScene();
        _preLoadedScene.AssemblyIndex = index;
        NoDestroyObjects.ForEach(o => _preLoadedScene.AddGameObject(o));
    }
}
