using Engine.BaseSystems;

namespace Engine.BaseTypes;

public interface ILevel
{
    public Scene GetScene(ActualGame actualGame);
}
