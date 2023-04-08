using Game.BaseTypes;

namespace Game.Levels;

public class Level0 : ILevel
{
    public static Scene GetScene()
    {
        return new Scene();
    }
}
