using System.Collections.Generic;
using System.IO;
using AForge.Imaging.Filters;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game;

internal delegate void ProgramState();

internal class InternalGame : Microsoft.Xna.Framework.Game
{
    internal SpriteBatch DrawSpace;
    internal GameTime ActualGameTime = new();
    internal SpriteFont DebugFont;
    internal readonly SceneManager SceneManager;
    internal event ProgramState OnBeforeUpdate;
    internal event ProgramState OnAfterUpdate;
    internal event ProgramState OnBeforeDraw;
    internal event ProgramState OnAfterDraw;

    internal InternalGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        _graphics.PreparingDeviceSettings += (_, args) =>
        {
            _graphics.PreferMultiSampling = true;
            args.GraphicsDeviceInformation.PresentationParameters.MultiSampleCount = 32;
        };
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        SceneManager = new(this);
    }

    internal float ResolutionCoefficient => .5f * (ViewportSize.X / 1280) + .5f * (ViewportSize.Y / 1280);

    internal Vector2 ViewportSize => new(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

    internal Vector2 ViewportCenter => ViewportSize / 2;

    internal Texture2D LoadSvg(string assetName, Vector2 size, List<IFilter> filters = null)
    {
        return SvgConverter.TransformSvgToTexture2D(
            GraphicsDevice,
            new FileStream($"{Content.RootDirectory}/{assetName}.svg", FileMode.Open),
            size,
            filters
        );
    }

    internal void ChangeVideoMode()
    {
        _needChangeVideoMode = true;
        SceneManager.LoadScene(SceneManager.CurrentSceneIndex);
    }

    protected override void Initialize()
    {
        _graphics.IsFullScreen = false;
        _graphics.PreferredBackBufferWidth = 1280;
        _graphics.PreferredBackBufferHeight = 720;
        _graphics.ApplyChanges();
        base.Initialize();
    }

    protected override void LoadContent()
    {
        DrawSpace = new SpriteBatch(GraphicsDevice);
        DebugFont = Content.Load<SpriteFont>("DebugFont");
        SceneManager.LoadScene(0);
    }

    protected override void Update(GameTime gameTime)
    {
        OnBeforeUpdate?.Invoke();
        ActualGameTime = gameTime;
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
        if (SceneManager.CurrentScene != null)
            SceneManager.CurrentScene.Update();
        if (_needChangeVideoMode)
        {
            if (_graphics.IsFullScreen)
            {
                _graphics.IsFullScreen = false;
                _graphics.PreferredBackBufferWidth = 1280;
                _graphics.PreferredBackBufferHeight = 720;
            }
            else
            {
                _graphics.IsFullScreen = true;
                _graphics.PreferredBackBufferWidth = _graphics.GraphicsDevice.DisplayMode.Width;
                _graphics.PreferredBackBufferHeight = _graphics.GraphicsDevice.DisplayMode.Height;
            }
            _graphics.ApplyChanges();
            _needChangeVideoMode = false;
        }
        OnAfterUpdate?.Invoke();
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        OnBeforeDraw?.Invoke();
        ActualGameTime = gameTime;
        GraphicsDevice.Clear(Color.White);
        DrawSpace.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
        if (SceneManager.CurrentScene != null)
            SceneManager.CurrentScene.Draw();
        DrawSpace.End();
        OnAfterDraw?.Invoke();
        base.Draw(gameTime);
    }

    private readonly GraphicsDeviceManager _graphics;
    private bool _needChangeVideoMode;
}
