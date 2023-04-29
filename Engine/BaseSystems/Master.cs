using Microsoft.Xna.Framework;

namespace Engine.BaseSystems;

public delegate void ProgramState();

internal class Master : Microsoft.Xna.Framework.Game
{
    internal GameTime ActualGameTime = new();
    internal readonly GraphicsDeviceManager Graphics;

    internal static event ProgramState OnBeforeUpdate;
    internal static event ProgramState OnAfterUpdate;
    internal static event ProgramState OnBeforeDraw;
    internal static event ProgramState OnAfterDraw;

    internal Master()
    {
        Graphics = new GraphicsDeviceManager(this);
        Graphics.PreparingDeviceSettings += (_, args) =>
        {
            Graphics.PreferMultiSampling = true;
            args.GraphicsDeviceInformation.PresentationParameters.MultiSampleCount = 32;
        };
        Content.RootDirectory = "";
        IsMouseVisible = true;
    }

    protected override void Update(GameTime gameTime)
    {
        ActualGameTime = gameTime;
        OnBeforeUpdate?.Invoke();
        SceneManager.CurrentScene?.Update();
        OnAfterUpdate?.Invoke();
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        OnBeforeDraw?.Invoke();
        ActualGameTime = gameTime;
        GraphicsDevice.Clear(Color.White);
        SceneManager.CurrentScene?.Draw();
        OnAfterDraw?.Invoke();
        base.Draw(gameTime);
    }
}
