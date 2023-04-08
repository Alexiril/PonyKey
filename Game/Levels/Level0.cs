using Game.BaseTypes;

namespace Game.Levels;

public abstract class Level0 : ILevel
{
    public static Scene GetScene(InternalGame actualGame)
    {
        return new Scene();
    }
}
