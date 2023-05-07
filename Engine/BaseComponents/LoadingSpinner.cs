using Engine.BaseSystems;
using Engine.BaseTypes;

namespace Engine.BaseComponents;

public class LoadingSpinner: Component
{
    public float Speed { get; set; } = 1;

    public bool LoadNextSceneOnStart { get; set; }

    public LoadingSpinner SetSpeed(float speed)
    {
        Speed = speed;
        return this;
    }

    public LoadingSpinner SetLoadNextSceneOnStart(bool load)
    {
        LoadNextSceneOnStart = load;
        return this;
    }

    public override void Start()
    {
        if (!LoadNextSceneOnStart) return;
        async void LoadNextSceneAction(int _) => await SceneManager.LoadSceneAsync(1);
        EventSystem.AddOnceTimeEvent(() => true, LoadNextSceneAction);

    }

    public override void Update() => Transform.Rotation += DeltaTime * Speed;
}
