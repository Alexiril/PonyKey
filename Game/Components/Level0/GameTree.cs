using System;
using System.Collections.Generic;
using Engine.BaseComponents;
using Engine.BaseTypes;
using Microsoft.Xna.Framework.Graphics;

namespace Game.Components.Level0;

internal class GameTree : Component
{
    public GameTree() {}

    public GameTree(GameTree tree) : base(tree)
    {
        _appleTrees = tree._appleTrees;
        _backTrees = tree._backTrees;
    }

    public GameTree SetTreesTextures(List<Texture2D> appleTrees, List<Texture2D> backTrees)
    {
        _appleTrees = appleTrees;
        _backTrees = backTrees;
        return this;
    }

    public override void Start()
    {
        var random = new Random();
        _appleTree = random.Next(0, 2) == 1;
        Sprite.SetTexture(_appleTree
            ? _appleTrees[random.Next(0, _appleTrees.Count)]
            : _backTrees[random.Next(0, _backTrees.Count)]);
    }

    public override void Update()
    {
        Transform.Position += -Transform.Right * DeltaTime;
        if (Transform.Position.X < -Master.ViewportSize.X * .2f)
            GameObject.Destroy();
    }

    protected override List<Type> Requirements => new() { typeof(Sprite) };

    private bool _appleTree;
    private List<Texture2D> _appleTrees;
    private List<Texture2D> _backTrees;
}
