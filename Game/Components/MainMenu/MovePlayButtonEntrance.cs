using Game.BaseTypes;
using Game.BuiltInComponents;

namespace Game.Components.MainMenu;

internal class MovePlayButtonEntrance : Component
{
    private double _startMilliseconds;

    internal override void Start()
    {
        _startMilliseconds = ActualGame.ActualGameTime.TotalGameTime.TotalMilliseconds;
    }

    internal override void Update()
    {
        if (ActualGame.ActualGameTime.TotalGameTime.TotalMilliseconds - _startMilliseconds > 2500)
        {
            if (Transform.Position.Y >
                ActualGame.ViewportCenter.Y + GetComponent<Sprite>().Height)
            {
                Transform.Position +=
                                (float)ActualGame.ActualGameTime.ElapsedGameTime.TotalMilliseconds *
                                .5f * Transform.GlobalUp;
            }
            else GameObject.DestroyComponent(this);
        }
    }
}
