namespace Game.BaseTypes;

internal class Component
{
    internal GameObject GameObject
    {
        get => _actualGameObject;
        init
        {
            _actualGameObject = value;
            _actualGameObject.AddComponent(this);
        }
    }

    private GameObject _actualGameObject;

    internal virtual void LoadContent() {}

    internal virtual void Update() {}

    internal virtual void Draw() {}

    internal void Destroy()
    {
        _actualGameObject = null;
    }
}
