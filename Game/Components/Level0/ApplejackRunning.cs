using Engine.BaseTypes;

namespace Game.Components.Level0;

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
        if (Transform.Position.X < Master.ViewportSize.X * .2f)
            Transform.Position += Transform.Right * (float)ActualGameTime.ElapsedGameTime.TotalMilliseconds;
        else
        {
            if (_ground.Transform.Position.X > 0)
                _ground.Transform.Position +=
                    -Transform.Right * (float)ActualGameTime.ElapsedGameTime.TotalMilliseconds;
            else
                _ground.Transform.Position = new(Master.ViewportSize.X, Master.ViewportCenter.Y);
            if (_grassForeground.Transform.Position.X > -Master.ViewportSize.X * .2f)
                _grassForeground.Transform.Position +=
                    -Transform.Right * 1.5f * (float)ActualGameTime.ElapsedGameTime.TotalMilliseconds;
            else
                _grassForeground.Transform.Position = new(Master.ViewportSize.X * 2, Master.ViewportCenter.Y);
        }
    }

    private GameObject _ground;
    private GameObject _grassForeground;
}
