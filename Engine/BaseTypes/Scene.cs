﻿using System.Collections.Generic;
using System.Linq;
using Engine.BaseSystems;
using Microsoft.Xna.Framework;
#if DEBUG
using System;
using Microsoft.Xna.Framework.Graphics;
#endif

namespace Engine.BaseTypes;

public class Scene
{
    public readonly string Name;

    public int AssemblyIndex { get; internal set; }

    public Color BackgroundColor { get; set; } = Color.CornflowerBlue;

    public ActualGame ActualGame { get; private set; }

    private readonly List<GameObject> _gameObjects = new();

    private readonly List<GameObject> _removingGameObjects = new();

    public Scene(ActualGame actualGame, string name)
    {
        ActualGame = actualGame;
        Name = name;
    }

    public Scene SetBackgroundColor(Color color)
    {
        BackgroundColor = color;
        return this;
    }

    public GameObject AddGameObject(GameObject gameObject)
    {
        _gameObjects.Add(gameObject);
        gameObject.ActualGame = ActualGame;
        gameObject.ActualScene = this;
        return gameObject;
    }

    public void DestroyGameObject(GameObject gameObject) => _removingGameObjects.Add(gameObject);

    public int RemoveGameObjectsByName(string name) => _gameObjects.RemoveAll(x => x.ObjectName == name);

    public int GameObjectsCount => _gameObjects.Count;

    internal List<GameObject> FindGameObjects(string name) => _gameObjects.FindAll(x => x.ObjectName == name);

    internal GameObject GetGameObject(int index) => _gameObjects[index];

    internal void Start()
    {
#if DEBUG
        EventSystem.OnToggleDebugInformation += OnToggleDebugInformation;

#endif
        foreach (var gameObject in _gameObjects)
            gameObject.Start();
    }

    internal void Update()
    {
        foreach (var gameObject in _gameObjects)
            gameObject.Update();
        if (_removingGameObjects.Count <= 0) return;
        foreach (var gameObject in _removingGameObjects)
        {
            gameObject.SetActive(false);
            _gameObjects.Remove(gameObject);
            gameObject.Remove();
        }
        _removingGameObjects.Clear();
    }

    internal void Draw()
    {
        ActualGame.GraphicsDevice.Clear(BackgroundColor);
        foreach (var gameObject in _gameObjects)
            gameObject.Draw();
#if DEBUG
        if (!_onDebug) return;
        ActualGame.DrawSpace.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);
        var posy = 10;
        ActualGame.DrawSpace.DrawString(
            ActualGame.DebugFont,
            $"Total committed memory: {MathF.Round((float)GC.GetGCMemoryInfo().TotalCommittedBytes / 0x100000, 3)} MB",
            new(5, posy),
            Color.White
        );
        posy += 25;
        ActualGame.DrawSpace.DrawString(
            ActualGame.DebugFont,
            $"Scene index: {AssemblyIndex}, scene name: '{Name}'",
            new(5, posy),
            Color.White
        );
        posy += 25;
        ActualGame.DrawSpace.DrawString(
            ActualGame.DebugFont,
            $"Frames per second: {MathF.Round(1 / (float)ActualGame.ActualGameTime.ElapsedGameTime.TotalSeconds, 2)}",
            new(5, posy),
            Color.White
        );
        posy += 25;
        if (ActualGame.ActualGameTime.TotalGameTime.TotalMilliseconds - _lastPrintLogTime > 3000)
            _logInformation.Clear();
        foreach (var info in _logInformation)
        {
            ActualGame.DrawSpace.DrawString(
                ActualGame.DebugFont,
                info,
                new(5, posy),
                Color.White
            );
            posy += 25;
        }

        foreach (var gameObject in _gameObjects)
        {
            ActualGame.DrawSpace.DrawString(
                ActualGame.DebugFont,
                $"{gameObject.ObjectName}:",
                new(10, posy),
                Color.White
            );
            posy += 25;
            ActualGame.DrawSpace.DrawString(
                ActualGame.DebugFont,
                string.Join(", ", gameObject.GetAllComponents().Select(x => x.GetType().Name)),
                new(30, posy),
                Color.White
            );
            posy += 25;
        }

        ActualGame.DrawSpace.End();

#endif
    }

    internal void Unload()
    {
#if DEBUG
        EventSystem.OnToggleDebugInformation -= OnToggleDebugInformation;
#endif
        foreach (var gameObject in _gameObjects)
        {
            EventSystem.RemoveEventByGameObject(gameObject);
            gameObject.Unload();
        }
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

    private void OnToggleDebugInformation()
    {
        if (ActualGame?.ActualGameTime == null || ActualGame.ActualGameTime.TotalGameTime.TotalMilliseconds - _lastDebugChange < 500)
            return;
        _onDebug = !_onDebug;
        _lastDebugChange = ActualGame.ActualGameTime.TotalGameTime.TotalMilliseconds;
    }

    private readonly Queue<string> _logInformation = new(7);

    private double _lastPrintLogTime = -1;

    private bool _onDebug = true;
    private double _lastDebugChange;

#endif
}
