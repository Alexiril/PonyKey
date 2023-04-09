using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Game.BaseTypes;

internal class Scene
{
    internal Color BackgroundColor { get; private set; } = Color.CornflowerBlue;

    internal InternalGame ActualGame { get; set; }

    private readonly List<GameObject> _gameObjects = new();

    private readonly List<GameObject> _removingGameObjects = new();

    internal Scene SetBackgroundColor(Color color)
    {
        BackgroundColor = color;
        return this;
    }

    internal GameObject AddGameObject(GameObject gameObject)
    {
        _gameObjects.Add(gameObject);
        gameObject.ActualScene = this;
        return gameObject;
    }

    internal void DestroyGameObject(GameObject gameObject) => _removingGameObjects.Add(gameObject);

    internal int RemoveGameObjectsByName(string name) => _gameObjects.RemoveAll(x => x.ObjectName == name);

    internal List<GameObject> FindGameObjects(string name) => _gameObjects.FindAll(x => x.ObjectName == name);
    internal GameObject GetGameObject(int index) => _gameObjects[index];

    internal void LoadContent()
    {
        foreach (var gameObject in _gameObjects)
            gameObject.LoadContent();
    }

    internal void Update()
    {
        foreach (var gameObject in _gameObjects)
            gameObject.Update();
        if (_removingGameObjects.Count > 0)
        {
            foreach (var gameObject in _removingGameObjects)
            {
                gameObject.SetActive(false);
                _gameObjects.Remove(gameObject);
                gameObject.Remove();
            }
            _removingGameObjects.Clear();
            GC.Collect();
        }
    }

    internal void Draw()
    {
        ActualGame.GraphicsDevice.Clear(BackgroundColor);
        foreach (var gameObject in _gameObjects)
        {
            gameObject.Draw();
        }
#if DEBUG
        var debugColor = BackgroundColor == Color.White ? Color.Black : Color.White;
        var posy = 10;

        ActualGame.DrawSpace.DrawString(
            ActualGame.DebugFont,
            $"Total committed bytes: {GC.GetGCMemoryInfo().TotalCommittedBytes}",
            new(5, posy),
            debugColor
        );
        posy += 25;
        foreach (var gameObject in _gameObjects)
        {
            ActualGame.DrawSpace.DrawString(
                ActualGame.DebugFont,
                $"{gameObject.ObjectName}:",
                new(10, posy),
                debugColor
            );
            posy += 25;
            ActualGame.DrawSpace.DrawString(
                ActualGame.DebugFont,
                string.Join(", ", gameObject.GetAllComponents().Select(x => x.GetType().Name)),
                new(30, posy),
                debugColor
            );
            posy += 25;
        }
#endif
    }
}
