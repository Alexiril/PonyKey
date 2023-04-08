using System.Collections.Generic;
using System.Linq;
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
#if DEBUG
        var debugColor = BackgroundColor == Color.White ? Color.Black : Color.White;
        var posy = 0;
#endif

        foreach (var gameObject in _gameObjects)
        {
            gameObject.Draw();
        }
#if DEBUG
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
