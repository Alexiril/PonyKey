using Game.BaseTypes;
using Game.BuiltInComponents;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game;

public class InternalGame : Microsoft.Xna.Framework.Game
{
    public SpriteBatch DrawSpace;
    public GameTime ActualGameTime;
    public readonly Scene GameScene;

    private GraphicsDeviceManager _graphics;

    public InternalGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        GameScene = new();
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        var snake = GameScene.AddGameObject(new GameObject("PlayerSnake", this));
        snake.AddComponent<Sprite>().LoadingTextureName = "SnakeTexture";
        base.Initialize();
    }

    protected override void LoadContent()
    {
        DrawSpace = new SpriteBatch(GraphicsDevice);
        GameScene.LoadContent();
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
        GraphicsDevice.Clear(Color.CornflowerBlue);
        DrawSpace.Begin();
        GameScene.Draw();
        DrawSpace.End();
        base.Draw(gameTime);
    }
}
