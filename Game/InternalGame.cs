using Game.BaseTypes;
using Game.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game;

internal class InternalGame : Microsoft.Xna.Framework.Game
{
    internal SpriteBatch DrawSpace;
    internal GameTime ActualGameTime = new();
    internal SpriteFont DebugFont;

    internal Scene GameScene
    {
        get => _gameScene;
        set
        {
            _gameScene = value;
            _gameScene.ActualGame = this;
        }
    }

    private readonly GraphicsDeviceManager _graphics;
    private Scene _gameScene;

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
        GameScene = MainMenu.GetScene(this);
    }

    protected override void Update(GameTime gameTime)
    {
        ActualGameTime = gameTime;
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
        GameScene.Update();
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        ActualGameTime = gameTime;
        GraphicsDevice.Clear(Color.White);
        DrawSpace.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
        GameScene.Draw();
        DrawSpace.End();
        base.Draw(gameTime);
    }
}
