using System.Collections.Generic;
using Game.BaseTypes;
using Game.BuiltInComponents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game.Levels;

public abstract class MainMenu : ILevel
{
    public static Scene GetScene(InternalGame actualGame)
    {
        var result = new Scene
        {
            BackgroundColor = Color.White
        };
        var button = result.AddGameObject(new GameObject("Button", actualGame));
        button.AddComponent<Sprite>().Texture = actualGame.Content.Load<Texture2D>("SnakeTexture");
        button.Transform.Position = new Vector2(520, 200);
        button.Transform.Rotation = 20;
        result.AddGameObject(new GameObject("Button1", actualGame)).AddComponent<Sprite>();
        return result;
    }

    public static void UnloadAssets(InternalGame actualGame)
    {
        actualGame.Content.UnloadAssets(new List<string> {"SnakeTexture"});
    }
}
