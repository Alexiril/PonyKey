using System.IO;
using AForge.Imaging.Filters;
using Game.BaseTypes;
using Game.BuiltInComponents;
using Microsoft.Xna.Framework;

namespace Game.Levels;

public abstract class MainMenu : ILevel
{
    public static Scene GetScene(InternalGame actualGame)
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
                new(actualGame.GraphicsDevice.Viewport.Width, actualGame.GraphicsDevice.Viewport.Height),
                new GaussianSharpen(1, 4)
            )).
            GameObject.Transform.SetPosition(new(
            (float)actualGame.GraphicsDevice.Viewport.Width / 2,
            (float)actualGame.GraphicsDevice.Viewport.Height / 2
        ));
        return result;
    }
}
