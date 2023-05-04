using System.Collections.Generic;
using System.Linq;
using Engine.BaseSystems;
using Microsoft.Xna.Framework;
using Game = Engine.BaseSystems.Game;
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

    private readonly List<GameObject> _gameObjects = new();

    private readonly List<GameObject> _removingGameObjects = new();

    public Scene(string name) => Name = name;

    public Scene SetBackgroundColor(Color color)
    {
        BackgroundColor = color;
        return this;
    }

    public GameObject AddGameObject(GameObject gameObject)
    {
        _gameObjects.Add(gameObject);
        gameObject.ActualScene = this;
        return gameObject;
    }

    public int GetGameObjectIndex(GameObject gameObject)
    {
        if (_gameObjects.Contains(gameObject))
            return _gameObjects.IndexOf(gameObject);
        return -1;
    }

    public GameObject Instantiate(GameObject gameObject, int index = -1)
    {
        gameObject = new GameObject(gameObject);
        gameObject.ObjectName += "(Clone)";
        EventSystem.AddOnceTimeEvent(() => true, _ =>
        {
            if (index != -1) _gameObjects.Insert(index, gameObject);
            else _gameObjects.Add(gameObject);
            gameObject.ActualScene = this;
            gameObject.Start();
        });
        return gameObject;
    }

    public void DestroyGameObject(GameObject gameObject) => _removingGameObjects.Add(gameObject);

    public int RemoveGameObjectsByName(string name) => _gameObjects.RemoveAll(x => x.ObjectName == name);

    public int GameObjectsCount => _gameObjects.Count;

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
        Game.GraphicsDevice.Clear(BackgroundColor);
        foreach (var gameObject in _gameObjects)
            gameObject.Draw();
#if DEBUG
        if (!Game.DebugInformationOn) return;
        Game.DrawSpace.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied);
        var posy = 10;
        Game.DrawSpace.DrawString(
            Game.DebugFont,
            $"Total committed memory: {MathF.Round((float)GC.GetGCMemoryInfo().TotalCommittedBytes / 0x100000, 3)} MB",
            new(5, posy),
            Color.White
        );
        posy += 25;
        Game.DrawSpace.DrawString(
            Game.DebugFont,
            $"Scene index: {AssemblyIndex}, scene name: '{Name}'",
            new(5, posy),
            Color.White
        );
        posy += 25;
        Game.DrawSpace.DrawString(
            Game.DebugFont,
            $"Frames per second: {MathF.Round(1 / (float)Game.GameTime.ElapsedGameTime.TotalSeconds, 2)}",
            new(5, posy),
            Color.White
        );
        posy += 25;
        Game.DrawSpace.DrawString(
            Game.DebugFont,
            $"Resolution: {Game.ViewportSize.X}x{Game.ViewportSize.Y}, screen center: {Game.ViewportCenter}",
            new(5, posy),
            Color.White
        );
        posy += 25;
        if (Game.GameTime.TotalGameTime.TotalMilliseconds - _lastPrintLogTime > 3000)
            _logInformation.Clear();
        foreach (var info in _logInformation)
        {
            Game.DrawSpace.DrawString(
                Game.DebugFont,
                info,
                new(5, posy),
                Color.White
            );
            posy += 25;
        }

        foreach (var gameObject in _gameObjects)
        {
            Game.DrawSpace.DrawString(
                Game.DebugFont,
                $"{gameObject.ObjectName}{(gameObject.Active ? "" : " (Inactive)")}:",
                new(10, posy),
                Color.White
            );
            posy += 25;
            Game.DrawSpace.DrawString(
                Game.DebugFont,
                string.Join(", ", gameObject.GetAllComponents().Select(x => x.GetType().Name)),
                new(30, posy),
                Color.White
            );
            posy += 25;
        }

        Game.DrawSpace.End();

#endif
    }

    internal void Unload()
    {
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
    }

#if DEBUG

    internal void Print(string information)
    {
        _logInformation.Enqueue(information);
        if (_logInformation.Count > 6)
            _logInformation.Dequeue();
        _lastPrintLogTime = Game.GameTime.TotalGameTime.TotalMilliseconds;
    }

    private readonly Queue<string> _logInformation = new(7);

    private double _lastPrintLogTime = -1;

#endif
}
