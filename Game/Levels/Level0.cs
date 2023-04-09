using Game.BaseTypes;

namespace Game.Levels;

internal abstract class Level0 : ILevel
{
    internal static Scene GetScene(InternalGame actualGame)
    {
        return new Scene();
    }
}
