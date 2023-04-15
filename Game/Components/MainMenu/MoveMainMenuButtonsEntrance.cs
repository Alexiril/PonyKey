using Game.BaseTypes;

namespace Game.Components.MainMenu;

internal class MoveMainMenuButtonsEntrance : Component
{
    internal override void Start() => _startMilliseconds = ActualGameTime.TotalGameTime.TotalMilliseconds;

    internal override void Update()
    {
        if (!(ActualGameTime.TotalGameTime.TotalMilliseconds - _startMilliseconds > 2500)) return;
        if (Transform.Position.Y > ActualGame.ViewportCenter.Y + Sprite.Height)
        {
            var delta = (float)ActualGameTime.ElapsedGameTime.TotalMilliseconds * .5f * Transform.GlobalUp;
            for (var i = 2; i < 7; i++) GameObject.GetGameObjectByIndex(i).Transform.Position += delta;
        }
        else GameObject.DestroyComponent(this);
    }

    private double _startMilliseconds;
}
