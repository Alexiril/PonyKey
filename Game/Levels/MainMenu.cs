using System.IO;
using Game.BaseTypes;
using Game.BuiltInComponents;
using Game.Components;
using Microsoft.Xna.Framework;

namespace Game.Levels;

internal abstract class MainMenu : ILevel
{
    internal static Scene GetScene(InternalGame actualGame)
    {
        var result = new Scene().SetBackgroundColor(Color.SkyBlue);
        result.AddGameObject(new GameObject("Background", actualGame)).AddComponent<Sprite>().SetTexture(
            SvgConverter.TransformSvgToTexture2D(
                actualGame.GraphicsDevice,
                new FileStream($"{actualGame.Content.RootDirectory}/MainMenuBackground.svg", FileMode.Open),
                new(actualGame.GraphicsDevice.Viewport.Width, actualGame.GraphicsDevice.Viewport.Height)
            )).GameObject.Transform.SetPosition(new(
            (float)actualGame.GraphicsDevice.Viewport.Width / 2,
            (float)actualGame.GraphicsDevice.Viewport.Height / 2)
        );
        result.AddGameObject(new GameObject("Logo", actualGame)).AddComponent<Sprite>().SetTexture(SvgConverter
            .TransformSvgToTexture2D(
                actualGame.GraphicsDevice,
                new FileStream($"{actualGame.Content.RootDirectory}/MainMenuLogo.svg", FileMode.Open),
                new(actualGame.GraphicsDevice.Viewport.Width, actualGame.GraphicsDevice.Viewport.Height)
            )).
            GameObject.Transform.SetPosition(new(
            (float)actualGame.GraphicsDevice.Viewport.Width / 2,
            (float)actualGame.GraphicsDevice.Viewport.Height / 2
        )).GameObject.AddComponent<MoveLogoEntrance>();
        result.LoadContent();
        return result;
    }
}
