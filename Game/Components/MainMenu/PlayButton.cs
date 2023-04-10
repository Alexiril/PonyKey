using Game.BaseTypes;
using Game.BuiltInComponents;
using Microsoft.Xna.Framework;

namespace Game.Components.MainMenu;

internal class PlayButton : Component
{
    internal override void Start()
    {
        GetComponent<InputTrigger>().OnPointerDown += _ =>
            GetComponent<Sprite>().TextureColor = new Color(255, 200, 255, 255);
        GetComponent<InputTrigger>().OnPointerExit += _ =>
            GetComponent<Sprite>().TextureColor = Color.White;
        GetComponent<InputTrigger>().OnPointerUp += _ =>
        {
            _startingNextScene = true;
            GetComponent<InputTrigger>().Active = false;
            _timeFromClick = ActualGame.ActualGameTime.TotalGameTime.TotalMilliseconds;
        };
    }

    internal override void Update()
    {
        if (_startingNextScene)
        {
            if (ActualGame.ActualGameTime.TotalGameTime.TotalMilliseconds - _timeFromClick < 1500)
            {
                GetComponent<Sprite>().TextureColor *= .95f;
                GameObject.GetGameObjectByIndex(0).GetComponent<Sprite>().TextureColor *= .95f;
            }
            else ActualGame.SceneManager.LoadScene(1);
        }
    }

    private bool _startingNextScene;
    private double _timeFromClick;
}
