using System;
using System.Linq;
using Engine.BaseComponents;
using Engine.BaseSystems;
using Engine.BaseTypes;
using Microsoft.Xna.Framework;
using EGame = Engine.BaseSystems.Game;

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
                    SvgConverter.LoadSvg( "Level0/Appletree1", new Vector2(512) * EGame.ResolutionCoefficient),
                    SvgConverter.LoadSvg( "Level0/Appletree2", new Vector2(512) * EGame.ResolutionCoefficient)
                },
                new()
                {
                    SvgConverter.LoadSvg( "Level0/Tree1", new Vector2(512) * EGame.ResolutionCoefficient),
                    SvgConverter.LoadSvg( "Level0/Tree2", new Vector2(512) * EGame.ResolutionCoefficient),
                    SvgConverter.LoadSvg( "Level0/Bushes", new Vector2(512) * EGame.ResolutionCoefficient),
                }
            )
            .SetButtons(
                ArchivedContent.GetFilesNames("Buttons")
                    .Select(x => (x, SvgConverter
                        .LoadSvg( x.Replace(".svg", ""), new Vector2(100) * EGame.ResolutionCoefficient))).ToList())
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
