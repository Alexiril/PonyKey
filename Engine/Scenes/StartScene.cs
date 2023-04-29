using Engine.BaseComponents;
using Engine.BaseSystems;
using Engine.BaseTypes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Scenes;

internal class StartScene : ILevel
{
    public Scene GetScene(Master master)
    {
        async void LoadNextSceneAction(int _) => await master.SceneManager.LoadSceneAsync(1);

        EventSystem.AddOnceTimeEvent(() => true, LoadNextSceneAction);
        return new Scene(master, "StartingScene")
            .SetBackgroundColor(Color.DeepSkyBlue)
            .AddGameObject(new("AltText"))
            .Transform.SetPosition(master.ViewportCenter)
            .AddComponent<TextMesh>()
            .SetColor(Color.FloralWhite)
            .SetFont(ArchivedContent.LoadContent<SpriteFont>(master, "defaultLoadingScreenFont"))
            .SetWidth((int)(master.ViewportSize.X  * .75f))
            .SetWordWrap(true)
            .SetText("Welcome here! :)")
            .SetCentralOffset()
            .AddGameObject(new("LoadingScreenBackground"))
            .Transform.SetPosition(master.ViewportCenter)
            .AddComponent<Sprite>()
            .SetTexture(master.LoadingScreenBackground)
            .AddGameObject(new("loadingSpinner"))
            .Transform.SetPosition(master.ViewportCenter)
            .AddComponent<Sprite>()
            .SetTexture(SvgConverter.LoadSvg(master,"loadingSpinner", new(master.ViewportSize.X * .7f)))
            .AddComponent<Spinner>()
            .ActualScene;
    }

    private class Spinner : Component
    {
        public override void Update() => Transform.Rotation += .5f;
    }
}
