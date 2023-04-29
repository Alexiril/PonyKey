using Engine.BaseComponents;
using Engine.BaseSystems;
using Engine.BaseTypes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Game = Engine.BaseSystems.Game;

namespace Engine.Scenes;

internal class StartScene : ILevel
{
    public Scene GetScene()
    {
        async void LoadNextSceneAction(int _) => await SceneManager.LoadSceneAsync(1);

        EventSystem.AddOnceTimeEvent(() => true, LoadNextSceneAction);
        return new Scene("StartingScene")
            .SetBackgroundColor(Color.DeepSkyBlue)
            .AddGameObject(new("AltText"))
            .Transform.SetPosition(Game.ViewportCenter)
            .AddComponent<TextMesh>()
            .SetColor(Color.FloralWhite)
            .SetFont(ArchivedContent.LoadContent<SpriteFont>( "defaultLoadingScreenFont"))
            .SetWidth((int)(Game.ViewportSize.X  * .75f))
            .SetWordWrap(true)
            .SetText("Welcome here! :)")
            .SetCentralOffset()
            .AddGameObject(new("LoadingScreenBackground"))
            .Transform.SetPosition(Game.ViewportCenter)
            .AddComponent<Sprite>()
            .SetTexture(Game.LoadingScreenBackground)
            .AddGameObject(new("loadingSpinner"))
            .Transform.SetPosition(Game.ViewportCenter)
            .AddComponent<Sprite>()
            .SetTexture(SvgConverter.LoadSvg("loadingSpinner", new(Game.ViewportSize.X * .7f)))
            .AddComponent<LoadingSpinner>()
            .SetSpeed(5)
            .ActualScene;
    }
}
