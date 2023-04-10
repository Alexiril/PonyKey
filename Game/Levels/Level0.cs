using Game.BaseTypes;
using Microsoft.Xna.Framework;

namespace Game.Levels;

internal class Level0 : ILevel
{
    public Scene GetScene(InternalGame actualGame)
    {
        return new Scene(actualGame).SetBackgroundColor(Color.SkyBlue);
    }
}
