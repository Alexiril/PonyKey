using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Game.BaseTypes;

public class Scene
{
    public string LevelName;

    public Color BackgroundColor { get; set; } = Color.CornflowerBlue;

    public InternalGame ActualGame { get; set; }

    private readonly List<GameObject> _gameObjects = new();

    public GameObject AddGameObject(GameObject gameObject)
    {
        _gameObjects.Add(gameObject);
        return gameObject;
    }

    public int RemoveGameObjectsByName(string name) => _gameObjects.RemoveAll(x => x.ObjectName == name);
    public List<GameObject> FindGameObjects(string name) => _gameObjects.FindAll(x => x.ObjectName == name);
    public GameObject GetGameObject(int index) => _gameObjects[index];

    public void LoadContent()
    {
        foreach (var gameObject in _gameObjects)
            gameObject.LoadContent();
    }

    public void Update()
    {
        foreach (var gameObject in _gameObjects)
            gameObject.Update();
    }

    public void Draw()
    {
        ActualGame.GraphicsDevice.Clear(BackgroundColor);
        foreach (var gameObject in _gameObjects)
            gameObject.Draw();
    }
}
