using Game.BaseTypes;
using Game.BuiltInComponents;

namespace Game.Components.MainMenu;

internal class PlayButton : Component
{
    internal override void Start()
    {
        GameObject.GetComponent<InputTrigger>().OnPointerInput += state =>
            GameObject.ActualGame.SceneManager.LoadScene(1);
    }
}
