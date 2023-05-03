using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Engine.BaseTypes;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Engine.BaseSystems;

public static class SceneManager
{
    public static void RegisterLevels(IEnumerable<string> levels) => Levels.AddRange(levels);

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

    private static readonly List<string> Levels = new();

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
        _preLoadedScene = ConstructScene(Levels[index]);
        _preLoadedScene.AssemblyIndex = index;
        NoDestroyObjects.ForEach(o => _preLoadedScene.AddGameObject(o));
    }

    private static Scene ConstructScene(string yaml)
    {
        Type GetAssetType(Dictionary<string, string> asset)
        {
            return asset["type"] switch
            {
                "texture-svg" => typeof(TextureSvg),
                "animation-svg" => typeof(AnimationSvg),
                _ => Assembly.GetCallingAssembly().GetTypes().First(x => x.Name == asset["type"])
            };
        }

        T GetAsset<T>(Dictionary<string, string> asset)
        {
            if (typeof(T) == typeof(TextureSvg))
            {

            }
            else if (typeof(T) == typeof(AnimationSvg))
            {
                return SvgConverter.LoadSvgAnimation(asset["assetName"], asset)
            }

            return ArchivedContent.LoadContent<T>(asset["filename"]);
        }

        var yamlScene = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build()
            .Deserialize<YamlScene>(new string(yaml
                .Where(x => (!char.IsControl(x) || x == '\n') && char.IsAscii(x)).ToArray()));
        var result = new Scene(yamlScene.Name);
        foreach (var objectName in yamlScene.Hierarchy)
            result.AddGameObject(new GameObject(objectName));
        return result;
    }

    // ReSharper disable once ClassNeverInstantiated.Local
    private class YamlScene
    {
        public string Name { get; set; }
        public string BackgroundColor { get; set; }
        public List<string> Hierarchy { get; set; }
        public Dictionary<string, Dictionary<string, string>> Assets { get; set; }
        public Dictionary<string, Dictionary<string, Dictionary<string, string>>> GameObjects { get; set; }
    }

    private class TextureSvg {}

    private class AnimationSvg {}
}
