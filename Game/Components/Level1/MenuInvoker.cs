using Engine.BaseTypes;
using Microsoft.Xna.Framework.Input;

namespace Game.Components.Level1;

public class MenuInvoker : Component
{
    public override void Start() => _menu = GameObject.Find("Menu")[0];

    public override void Update()
    {
        if (_previousState.IsKeyUp(Keys.Escape) && Keyboard.GetState().IsKeyDown(Keys.Escape) && !_menu.Active)
            _menu.Active = true;
        _previousState = Keyboard.GetState();
    }

    private GameObject _menu;
    private KeyboardState _previousState = Keyboard.GetState();
}
