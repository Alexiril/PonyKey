using Engine.BaseTypes;

namespace Engine.StartSceneComponents;

public class StartSceneInit : Component
{
    public override void Start()
    {
        gameObject.GetGameObjectByIndex(0);
    }
}
