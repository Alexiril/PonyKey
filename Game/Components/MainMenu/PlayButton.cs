using Game.BaseTypes;
using Game.BuiltInComponents;
using Microsoft.Xna.Framework;

namespace Game.Components.MainMenu;

internal class PlayButton : Component
{
    internal override void Start()
    {
        GetComponent<InputTrigger>().OnPointerDown += _ =>
            Sprite.TextureColor = new Color(255, 200, 255, 255);
        GetComponent<InputTrigger>().OnPointerHover += _ =>
            Sprite.TextureColor = new Color(255, 230, 255, 255);
        GetComponent<InputTrigger>().OnPointerExit += _ =>
            Sprite.TextureColor = Color.White;
        GetComponent<InputTrigger>().OnPointerUp += delegate
        {
            _startingNextScene = true;
            GetComponent<InputTrigger>().Active = false;
            _timeFromClick = ActualGameTime.TotalGameTime.TotalMilliseconds;
        };
    }

    internal override void Update()
    {
        if (!_startingNextScene) return;
        if (ActualGameTime.TotalGameTime.TotalMilliseconds - _timeFromClick < 1500)
        {
            for (int i = 0; i < ActualScene.GameObjectsCount; i++)
            {
                var gameObject = GameObject.GetGameObjectByIndex(i);
                var sprite = gameObject.GetComponent<Sprite>();
                if (sprite != null)
                    sprite.TextureColor *= .95f;
                if (gameObject.HaveComponent<InputTrigger>())
                    gameObject.DestroyComponent(gameObject.GetComponent<InputTrigger>());
            }
        }
        else SceneManager.LoadScene(1);
    }

    private bool _startingNextScene;
    private double _timeFromClick;
}
