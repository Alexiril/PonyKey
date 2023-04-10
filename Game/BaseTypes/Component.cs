using Game.BuiltInComponents;

namespace Game.BaseTypes;

internal class Component
{
    internal GameObject GameObject { get; set; }

    internal bool Active { get; set; } = true;

    internal virtual void Start() {}

    internal virtual void Update() {}

    internal virtual void Draw() {}

    internal void Print(string information) => GameObject.Print(information);

    internal T GetComponent<T>() where T : Component => GameObject.GetComponent<T>();

    internal Transform Transform => GameObject.Transform;

    internal Scene ActualScene => GameObject.ActualScene;

    internal InternalGame ActualGame => GameObject.ActualGame;
}
