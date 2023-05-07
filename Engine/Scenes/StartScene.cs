using System.Text;
using Engine.BaseComponents;
using Engine.BaseSystems;
using Engine.BaseTypes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Game = Engine.BaseSystems.Game;

namespace Engine.Scenes;

internal class StartScene : ILevel
{
    public Scene GetScene() =>
        SceneBuilder.BuildScene(Encoding.UTF8.GetString(EngineContent.LoadFile("StartScene.json").GetBuffer()));
        // new Scene("StartingScene")
        //     .SetBackgroundColor(Color.DeepSkyBlue)
        //     .AddGameObject(AltText)
        //     .AddGameObject(LoadingScreenBackground)
        //     .AddGameObject(LoadingSpinner)
        //     .ActualScene;

    private static GameObject AltText =>
        new GameObject("AltText")
            .Transform.SetPosition(Game.ViewportCenter)
            .AddComponent<TextMesh>()
            .SetColor(Color.FloralWhite)
            .SetFont(EngineContent.LoadContent<SpriteFont>("defaultLoadingScreenFont"))
            .SetWidth((int)(Game.ViewportSize.X * .75f))
            .SetWordWrap(true)
            .SetText("Welcome here! :)")
            .SetCentralOffset()
            .GameObject;

    private static GameObject LoadingScreenBackground =>
        new GameObject("LoadingScreenBackground")
            .Transform.SetPosition(Game.ViewportCenter)
            .AddComponent<Sprite>()
            .SetTexture(Game.LoadingScreenBackground)
            .GameObject;

    private static GameObject LoadingSpinner =>
        new GameObject("LoadingSpinner").Transform.SetPosition(Game.ViewportCenter)
            .AddComponent<Sprite>()
            .SetTexture(EngineContent.LoadSvgTexture("loadingSpinner", new(Game.ViewportSize.X * .7f)))
            .AddComponent<LoadingSpinner>()
            .SetSpeed(5)
            .GameObject;
}
