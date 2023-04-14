using Game.BaseTypes;
using Game.BuiltInComponents;
using Microsoft.Xna.Framework;

namespace Game.Components.MainMenu;

internal class DisplayButton : Component
{
    internal override void Start()
    {
        GetComponent<InputTrigger>().OnPointerDown += _ =>
            Sprite.TextureColor = new Color(255, 200, 255, 255);
        GetComponent<InputTrigger>().OnPointerHover += _ =>
            Sprite.TextureColor = new Color(255, 230, 255, 255);
        GetComponent<InputTrigger>().OnPointerExit += _ =>
            Sprite.TextureColor = Color.White;
        GetComponent<InputTrigger>().OnPointerUp += _ => ActualGame.ChangeVideoMode();
    }
}
