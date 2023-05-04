using Engine.BaseTypes;
using EGame = Engine.BaseSystems.Game;

namespace Game.Components.Level1;

internal class ApplejackRunning : Component
{
    public bool StartPlaying;

    public override void Start()
    {
        _ground = GameObject.Find("Background1")[0];
        _grassForeground = GameObject.Find("Background2")[0];
    }

    public override void Update()
    {
        if (!StartPlaying) return;
        if (Transform.Position.X < EGame.ViewportSize.X * .2f)
            Transform.Position += Transform.Right * DeltaTime;
        else
        {
            if (_ground.Transform.Position.X > 0)
                _ground.Transform.Position +=
                    -Transform.Right * DeltaTime;
            else
                _ground.Transform.Position = new(EGame.ViewportSize.X, EGame.ViewportCenter.Y);
            if (_grassForeground.Transform.Position.X > -EGame.ViewportSize.X * .2f)
                _grassForeground.Transform.Position +=
                    -Transform.Right * 1.5f * DeltaTime;
            else
                _grassForeground.Transform.Position = new(EGame.ViewportSize.X * 2, EGame.ViewportCenter.Y);
        }
    }

    private GameObject _ground;
    private GameObject _grassForeground;
}
