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
            Transform.Position += delta;
            GameObject.GetGameObjectByIndex(3).Transform.Position += delta;
            GameObject.GetGameObjectByIndex(4).Transform.Position += delta;
            GameObject.GetGameObjectByIndex(5).Transform.Position += delta;
        }
        else GameObject.DestroyComponent(this);
    }

    private double _startMilliseconds;
}
