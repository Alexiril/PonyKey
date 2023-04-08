using System.Collections.Generic;

namespace Game.BaseTypes;

public class Scene
{
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
        foreach (var gameObject in _gameObjects)
            gameObject.Draw();
    }
}
