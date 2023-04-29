using System;
using System.Linq;
using Engine.BaseComponents;
using Engine.BaseSystems;
using Engine.BaseTypes;
using Microsoft.Xna.Framework;

namespace Game.Components.Level0;

internal class TreesGenerator : Component
{
    public TreesGenerator() => _random = new Random();

    public override void Start()
    {
        _gameTree = new GameObject("GameTree")
            .Transform.SetPosition(Transform.Position)
            .AddComponent<GameTree>()
            .SetTreesTextures(
                new()
                {
                    SvgConverter.LoadSvg(Master, "Level0/Appletree1", new Vector2(512) * Master.ResolutionCoefficient),
                    SvgConverter.LoadSvg(Master, "Level0/Appletree2", new Vector2(512) * Master.ResolutionCoefficient)
                },
                new()
                {
                    SvgConverter.LoadSvg(Master, "Level0/Tree1", new Vector2(512) * Master.ResolutionCoefficient),
                    SvgConverter.LoadSvg(Master, "Level0/Tree2", new Vector2(512) * Master.ResolutionCoefficient),
                    SvgConverter.LoadSvg(Master, "Level0/Bushes", new Vector2(512) * Master.ResolutionCoefficient),
                }
            )
            .SetButtons(
                ArchivedContent.GetFilesNames("Buttons")
                    .Select(x => (x, SvgConverter
                        .LoadSvg(Master, x.Replace(".svg", ""), new Vector2(100) * Master.ResolutionCoefficient))).ToList())
            .AddComponent<Sprite>()
            .GameObject;
    }

    public override void Update()
    {
        if (_currentTimeFromPreviousTree < MinTimeOffsetBetweenTrees)
        {
            _currentTimeFromPreviousTree += DeltaTime;
            return;
        }

        if (_random.Next(0, 100) != 2) return;
        _currentTimeFromPreviousTree = 0;
        Instantiate(_gameTree, GameObject.GetIndexInScene());
    }

    private float _currentTimeFromPreviousTree;
    private GameObject _gameTree;
    private readonly Random _random;
    private const float MinTimeOffsetBetweenTrees = 1000;
}
