using Game.BaseTypes;
using Microsoft.Xna.Framework;

namespace Game.Levels;

public class MainMenu : ILevel
{
    public static Scene GetScene()
    {
        var result = new Scene();
        result.BackgroundColor = Color.White;
        return result;
    }
}
