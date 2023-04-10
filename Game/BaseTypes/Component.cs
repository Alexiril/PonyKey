namespace Game.BaseTypes;

internal class Component
{
    internal GameObject GameObject { get; set; }

    internal virtual void Start() {}

    internal virtual void Update() {}

    internal virtual void Draw() {}

    internal void Print(string information) => GameObject.Print(information);
}
