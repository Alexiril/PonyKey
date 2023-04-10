using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace Game.BaseTypes;

internal class Scene
{
    internal Color BackgroundColor { get; set; } = Color.CornflowerBlue;

    internal InternalGame ActualGame { get; private set; }

    private readonly List<GameObject> _gameObjects = new();

    private readonly List<GameObject> _removingGameObjects = new();

    public Scene(InternalGame actualGame)
    {
        ActualGame = actualGame;
    }

    internal Scene SetBackgroundColor(Color color)
    {
        BackgroundColor = color;
        return this;
    }

    internal GameObject AddGameObject(GameObject gameObject)
    {
        _gameObjects.Add(gameObject);
        gameObject.ActualGame = ActualGame;
        gameObject.ActualScene = this;
        return gameObject;
    }

    internal void DestroyGameObject(GameObject gameObject) => _removingGameObjects.Add(gameObject);

    internal int RemoveGameObjectsByName(string name) => _gameObjects.RemoveAll(x => x.ObjectName == name);

    internal List<GameObject> FindGameObjects(string name) => _gameObjects.FindAll(x => x.ObjectName == name);

    internal GameObject GetGameObject(int index) => _gameObjects[index];

    internal void Start()
    {
        foreach (var gameObject in _gameObjects)
            gameObject.Start();
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
            $"Total committed memory: {MathF.Round((float)GC.GetGCMemoryInfo().TotalCommittedBytes / 0x100000, 3)} MB",
            new(5, posy),
            debugColor
        );
        posy += 25;
        ActualGame.DrawSpace.DrawString(
            ActualGame.DebugFont,
            $"Scene index: {ActualGame.SceneManager.CurrentSceneIndex}",
            new(5, posy),
            debugColor
        );
        posy += 25;
        ActualGame.DrawSpace.DrawString(
            ActualGame.DebugFont,
            $"Frames per second: {MathF.Round(1 / (float)ActualGame.ActualGameTime.ElapsedGameTime.TotalSeconds, 2)}",
            new(5, posy),
            debugColor
        );
        posy += 25;
        if (ActualGame.ActualGameTime.TotalGameTime.TotalMilliseconds - _lastPrintLogTime > 3000)
            ClearLog();
        foreach (var info in _logInformation)
        {
            ActualGame.DrawSpace.DrawString(
                ActualGame.DebugFont,
                info,
                new(5, posy),
                debugColor
            );
            posy += 25;
        }
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

    internal void Remove()
    {
        foreach (var gameObject in _gameObjects.ToList())
        {
            gameObject.SetActive(false);
            _gameObjects.Remove(gameObject);
            gameObject.Remove();
        }

        _gameObjects.Clear();
        _removingGameObjects.Clear();
        ActualGame = null;
    }

#if DEBUG

    internal void Print(string information)
    {
        _logInformation.Enqueue(information);
        if (_logInformation.Count > 6)
            _logInformation.Dequeue();
        _lastPrintLogTime = ActualGame.ActualGameTime.TotalGameTime.TotalMilliseconds;
    }

    internal void ClearLog() => _logInformation.Clear();

    private readonly Queue<string> _logInformation = new(7);

    private double _lastPrintLogTime = -1;

#endif
}
