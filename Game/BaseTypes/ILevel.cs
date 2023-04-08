namespace Game.BaseTypes;

public interface ILevel
{
    public static Scene GetScene(InternalGame actualGame)
    {
        return null;
    }

    public static void UnloadAssets(InternalGame actualGame) {}
}
