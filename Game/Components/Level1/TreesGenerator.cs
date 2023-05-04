using System;
using System.Linq;
using System.Threading.Tasks;
using Engine.BaseComponents;
using Engine.BaseSystems;
using Engine.BaseTypes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using EGame = Engine.BaseSystems.Game;

namespace Game.Components.Level1;

internal class TreesGenerator : Component
{
    public TreesGenerator()
    {
        _random = new Random();
        _gameTree = new GameObject("GameTree")
            .AddComponent<GameTree>()
            .SetTreesTextures(
                new()
                {
                    EngineContent.LoadSvg("Level1/Appletree1", new Vector2(512) * EGame.ResolutionCoefficient),
                    EngineContent.LoadSvg("Level1/Appletree2", new Vector2(512) * EGame.ResolutionCoefficient)
                },
                new()
                {
                    EngineContent.LoadSvg("Level1/Tree1", new Vector2(512) * EGame.ResolutionCoefficient),
                    EngineContent.LoadSvg("Level1/Tree2", new Vector2(512) * EGame.ResolutionCoefficient),
                    EngineContent.LoadSvg("Level1/Bushes", new Vector2(512) * EGame.ResolutionCoefficient),
                }
            )
            .SetButtons(
                EngineContent.GetFilesNames("Buttons")
                    .Select(x => (GetKey(x), EngineContent
                        .LoadSvg(x.Replace(".svg", ""), new Vector2(100) * EGame.ResolutionCoefficient))).ToList())
            .SetApples(new GameObject("Apples").AddComponent<Sprite>().AddComponent<Apples>().SetTextures(new[]
                {
                    EngineContent.LoadSvg("Level1/Apple1", new(50 * Engine.BaseSystems.Game.ResolutionCoefficient)),
                    EngineContent.LoadSvg("Level1/Apple2", new(50 * Engine.BaseSystems.Game.ResolutionCoefficient)),
                    EngineContent.LoadSvg("Level1/Apple3", new(50 * Engine.BaseSystems.Game.ResolutionCoefficient))
                })
                .GameObject)
            .AddComponent<Sprite>()
            .GameObject;
    }

    public override void Start() => _gameTree.Transform.SetPosition(Transform.Position);


    public override void Update()
    {
        if (_currentTimeFromPreviousTree < MinTimeOffsetBetweenTrees)
        {
            _currentTimeFromPreviousTree += DeltaTime;
            return;
        }

        if (_random.Next(0, 100) != 2) return;
        _currentTimeFromPreviousTree = 0;

        async Task MakeInstance() => await Task.Run(() => Instantiate(_gameTree, GameObject.GetIndexInScene()));
        MakeInstance().ConfigureAwait(false);
    }

    private static Keys GetKey(string key)
    {
        key = key.Split("/").Last().Replace(".svg", "");
        if (Enum.TryParse(typeof(Keys), key, true, out var result) && result != null)
            return (Keys)result;
        throw new Exception("Not correct key");
    }

    private float _currentTimeFromPreviousTree;
    private readonly GameObject _gameTree;
    private readonly Random _random;
    private const float MinTimeOffsetBetweenTrees = 1000;
}
