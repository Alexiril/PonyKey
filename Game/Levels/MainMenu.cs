using Game.BaseTypes;
using Microsoft.Xna.Framework;

namespace Game.Levels;

public abstract class MainMenu : ILevel
{
    public static Scene GetScene(InternalGame actualGame)
    {
        var result = new Scene
        {
            BackgroundColor = Color.White
        };
        return result;
    }

    public static void UnloadAssets(InternalGame actualGame)
    {

    }
}
