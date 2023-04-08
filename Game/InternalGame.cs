using Game.BaseTypes;
using Game.Levels;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game;

public class InternalGame : Microsoft.Xna.Framework.Game
{
    public SpriteBatch DrawSpace;
    public GameTime ActualGameTime;
    public SpriteFont DebugFont;

    public Scene GameScene
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

    public InternalGame()
    {
        _graphics = new GraphicsDeviceManager(this);
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
        DrawSpace.Begin();
        GameScene.Draw();
        DrawSpace.End();
        base.Draw(gameTime);
    }
}
