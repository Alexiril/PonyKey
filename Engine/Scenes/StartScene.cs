using System.Threading.Tasks;
using Engine.BaseComponents;
using Engine.BaseSystems;
using Engine.BaseTypes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Scenes;

internal class StartScene : ILevel
{
    public Scene GetScene(ActualGame actualGame)
    {
        async void LoadNextSceneAction(int _)
        {
            await Task.Delay(1500);
            await actualGame.SceneManager.LoadSceneAsync(1);
        }

        EventSystem.AddOnceTimeEvent(() => true, LoadNextSceneAction);
        return new Scene(actualGame, "StartingScene")
            .SetBackgroundColor(Color.DeepSkyBlue)
            .AddGameObject(new("AltText"))
            .Transform.SetPosition(actualGame.ViewportCenter)
            .AddComponent<TextMesh>()
            .SetColor(Color.FloralWhite)
            .SetFont(actualGame.Content.Load<SpriteFont>("defaultLoadingScreenFont"))
            .SetWidth((int)(actualGame.ViewportSize.X  * .75f))
            .SetWordWrap(true)
            .SetText("Welcome here! :)")
            .SetCentralOffset()
            .AddGameObject(new("LoadingScreenBackground"))
            .Transform.SetPosition(actualGame.ViewportCenter)
            .AddComponent<Sprite>()
            .SetTexture(actualGame.LoadingScreenBackground)
            .AddGameObject(new("loadingSpinner"))
            .Transform.SetPosition(actualGame.ViewportCenter)
            .AddComponent<Sprite>()
            .SetTexture(actualGame.LoadSvg("loadingSpinner", new(actualGame.ViewportSize.X * .7f)))
            .AddComponent<Spinner>()
            .ActualScene;
    }

    private class Spinner : Component
    {
        public override void Update() => Transform.Rotation += .5f;
    }
}
