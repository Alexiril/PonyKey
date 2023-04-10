using Game.BaseTypes;
using Game.BuiltInComponents;

namespace Game.Components.MainMenu;

internal class MovePlayButtonEntrance : Component
{
    private double _startMilliseconds;

    internal override void Start()
    {
        _startMilliseconds = GameObject.ActualGame.ActualGameTime.TotalGameTime.TotalMilliseconds;
    }

    internal override void Update()
    {
        if (GameObject.ActualGame.ActualGameTime.TotalGameTime.TotalMilliseconds - _startMilliseconds > 2500)
        {
            if (GameObject.Transform.Position.Y >
                GameObject.ActualGame.ViewportCenter.Y + GameObject.GetComponent<Sprite>().Height)
            {
                GameObject.Transform.Position +=
                                (float)GameObject.ActualGame.ActualGameTime.ElapsedGameTime.TotalMilliseconds *
                                .5f * GameObject.Transform.GlobalUp;
            }
            else GameObject.DestroyComponent(this);
        }
    }
}
