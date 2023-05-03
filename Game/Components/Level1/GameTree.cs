using System;
using System.Collections.Generic;
using Engine.BaseComponents;
using Engine.BaseTypes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game.Components.Level1;

internal class GameTree : Component
{
    public GameTree() {}

    public GameTree(GameTree tree) : base(tree)
    {
        _appleTrees = tree._appleTrees;
        _backTrees = tree._backTrees;
        _buttons = tree._buttons;
    }

    public GameTree SetTreesTextures(List<Texture2D> appleTrees, List<Texture2D> backTrees)
    {
        _appleTrees = appleTrees;
        _backTrees = backTrees;
        return this;
    }

    public GameTree SetButtons(List<(string, Texture2D)> buttons)
    {
        _buttons = buttons;
        return this;
    }

    public override void Start()
    {
        var random = new Random();
        _appleTree = random.Next(0, 2) == 1;
        Sprite.SetTexture(_appleTree
            ? _appleTrees[random.Next(0, _appleTrees.Count)]
            : _backTrees[random.Next(0, _backTrees.Count)]);
        if (!_appleTree) return;
        _currentButton = random.Next(0, _buttons.Count - 1);
        Sprite.AppendTexture(_buttons[_currentButton].Item2, Sprite.Center + new Vector2(0, -Sprite.Center.Y));
    }

    public override void Update()
    {
        Transform.Position += -Transform.Right * DeltaTime;
        if (Transform.Position.X < -Engine.BaseSystems.Game.ViewportSize.X * .2f)
            gameObject.Destroy();
    }

    protected override List<Type> Requirements => new() { typeof(Sprite) };

    private bool _appleTree;
    private List<Texture2D> _appleTrees;
    private List<Texture2D> _backTrees;
    private List<(string, Texture2D)> _buttons;
    private int _currentButton = -1;
}
