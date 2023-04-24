using System.Collections.Generic;
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

    public Master Master { get; private set; }

    private readonly List<GameObject> _gameObjects = new();

    private readonly List<GameObject> _removingGameObjects = new();

    public Scene(Master master, string name)
    {
        Master = master;
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
        gameObject.Master = Master;
        gameObject.ActualScene = this;
        return gameObject;
    }

    public GameObject Instantiate(GameObject gameObject)
    {
        var newGameObject = new GameObject(gameObject);
        EventSystem.AddOnceTimeEvent(() => true, _ => AddGameObject(newGameObject));
        return newGameObject;
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
        Master.GraphicsDevice.Clear(BackgroundColor);
        foreach (var gameObject in _gameObjects)
            gameObject.Draw();
#if DEBUG
        if (!_onDebug) return;
        Master.DrawSpace.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);
        var posy = 10;
        Master.DrawSpace.DrawString(
            Master.DebugFont,
            $"Total committed memory: {MathF.Round((float)GC.GetGCMemoryInfo().TotalCommittedBytes / 0x100000, 3)} MB",
            new(5, posy),
            Color.White
        );
        posy += 25;
        Master.DrawSpace.DrawString(
            Master.DebugFont,
            $"Scene index: {AssemblyIndex}, scene name: '{Name}'",
            new(5, posy),
            Color.White
        );
        posy += 25;
        Master.DrawSpace.DrawString(
            Master.DebugFont,
            $"Frames per second: {MathF.Round(1 / (float)Master.ActualGameTime.ElapsedGameTime.TotalSeconds, 2)}",
            new(5, posy),
            Color.White
        );
        posy += 25;
        Master.DrawSpace.DrawString(
            Master.DebugFont,
            $"Resolution: {Master.ViewportSize.X}x{Master.ViewportSize.Y}, screen center: {Master.ViewportCenter}",
            new(5, posy),
            Color.White
        );
        posy += 25;
        if (Master.ActualGameTime.TotalGameTime.TotalMilliseconds - _lastPrintLogTime > 3000)
            _logInformation.Clear();
        foreach (var info in _logInformation)
        {
            Master.DrawSpace.DrawString(
                Master.DebugFont,
                info,
                new(5, posy),
                Color.White
            );
            posy += 25;
        }

        foreach (var gameObject in _gameObjects)
        {
            Master.DrawSpace.DrawString(
                Master.DebugFont,
                $"{gameObject.ObjectName}:",
                new(10, posy),
                Color.White
            );
            posy += 25;
            Master.DrawSpace.DrawString(
                Master.DebugFont,
                string.Join(", ", gameObject.GetAllComponents().Select(x => x.GetType().Name)),
                new(30, posy),
                Color.White
            );
            posy += 25;
        }

        Master.DrawSpace.End();

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
        Master = null;
    }

#if DEBUG

    internal void Print(string information)
    {
        _logInformation.Enqueue(information);
        if (_logInformation.Count > 6)
            _logInformation.Dequeue();
        _lastPrintLogTime = Master.ActualGameTime.TotalGameTime.TotalMilliseconds;
    }

    private void OnToggleDebugInformation()
    {
        if (Master?.ActualGameTime == null || Master.ActualGameTime.TotalGameTime.TotalMilliseconds - _lastDebugChange < 500)
            return;
        _onDebug = !_onDebug;
        _lastDebugChange = Master.ActualGameTime.TotalGameTime.TotalMilliseconds;
    }

    private readonly Queue<string> _logInformation = new(7);

    private double _lastPrintLogTime = -1;

    private bool _onDebug = true;
    private double _lastDebugChange;

#endif
}
