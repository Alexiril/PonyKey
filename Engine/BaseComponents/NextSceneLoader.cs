using Engine.BaseSystems;
using Engine.BaseTypes;

namespace Engine.BaseComponents;

public class NextSceneLoader: Component
{
    public override void Start()
    {
        async void LoadNextSceneAction(int _) => await SceneManager.LoadSceneAsync(ActualScene.AssemblyIndex + 1);
        EventSystem.AddOnceTimeEvent(() => true, LoadNextSceneAction);
    }
}
