namespace Game.BaseTypes;

public class Component
{
    public GameObject GameObject
    {
        get => _actualGameObject;
        init
        {
            _actualGameObject = value;
            _actualGameObject.AddComponent(this);
        }
    }

    private readonly GameObject _actualGameObject;

    public virtual void LoadContent() {}

    public virtual void Update() {}

    public virtual void Draw() {}
}
