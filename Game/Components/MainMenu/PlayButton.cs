using System.Threading.Tasks;
using Engine.BaseComponents;
using Engine.BaseSystems;

namespace Game.Components.MainMenu;

internal class PlayButton : SpriteButton
{
    public override void Start()
    {
        base.Start();
        SetOnPointerUp(_ =>
            {
                _startingNextScene = true;
                GetComponent<InputTrigger>().Active = false;
                _timeFromClick = GameTime.TotalGameTime.TotalMilliseconds;
            }
        );
    }

    public override void Update()
    {
        if (_loadingScene?.Exception != null)
            throw _loadingScene.Exception;
        if (!_startingNextScene) return;
        if (GameTime.TotalGameTime.TotalMilliseconds - _timeFromClick < 1500)
        {
            for (var i = 1; i < ActualScene.GameObjectsCount; i++)
            {
                var gameObject = GameObject.GetGameObjectByIndex(i);
                var sprite = gameObject.Sprite;
                if (sprite != null) sprite.TextureColor *= .95f;
                if (gameObject.HasComponent<InputTrigger>())
                    gameObject.DestroyComponent(gameObject.GetComponent<InputTrigger>());
            }
        }
        else
        {
            for (var i = 1; i < ActualScene.GameObjectsCount; i++) GameObject.GetGameObjectByIndex(i).Destroy();
            _loadingScene = SceneManager.LoadSceneAsync(ActualScene.AssemblyIndex+1);
            _startingNextScene = false;
        }
    }

    private bool _startingNextScene;
    private double _timeFromClick;
    private Task _loadingScene;
}
