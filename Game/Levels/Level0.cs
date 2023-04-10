using Game.BaseTypes;

namespace Game.Levels;

internal class Level0 : ILevel
{
    public Scene GetScene(InternalGame actualGame)
    {
        return new Scene(actualGame);
    }
}
