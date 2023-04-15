using Game.BaseTypes;
using Game.BuiltInComponents;
using Microsoft.Xna.Framework;

namespace Game.Levels;

internal class Level0 : ILevel
{
    public Scene GetScene(InternalGame actualGame)
    {
        return new Scene(actualGame)
            .SetBackgroundColor(Color.SkyBlue)
            .AddGameObject(new GameObject("Background"))
            .AddComponent<Sprite>()
            .SetTexture(actualGame.LoadSvg("Level0/Background0", actualGame.ViewportSize))
            .Transform.SetPosition(actualGame.ViewportCenter)
            .ActualScene
            .AddGameObject(new GameObject("Background1"))
            .AddComponent<Sprite>()
            .SetTexture(actualGame.LoadSvg("Level0/Background1",
                new Vector2(2560, 720) * actualGame.ResolutionCoefficient))
            .Transform.SetPosition(new(actualGame.ViewportSize.X, actualGame.ViewportCenter.Y))
            .ActualScene
            .AddGameObject(new GameObject("Background2"))
            .AddComponent<Sprite>()
            .SetTexture(actualGame.LoadSvg("Level0/Background2",
                new Vector2(2560, 720) * actualGame.ResolutionCoefficient))
            .Transform.SetPosition(new(actualGame.ViewportSize.X, actualGame.ViewportCenter.Y))
            .ActualScene;
    }
}
