using Engine.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.BaseSystems;

public delegate void ProgramState();

public class ActualGame : Game
{
    public GameTime ActualGameTime = new();
    public readonly SceneManager SceneManager;
    public event ProgramState OnBeforeUpdate;
    public event ProgramState OnAfterUpdate;
    public event ProgramState OnBeforeDraw;
    public event ProgramState OnAfterDraw;

    public ActualGame()
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
        SceneManager.RegisterLevels(new() { new StartScene() });
    }

    public void SetLoadingScreenBackground(string assetName) => _loadingScreenBackgroundAssetName = assetName;

    public Texture2D LoadSvg(string assetName, Vector2 size) =>
        SvgConverter.LoadSvg(this, assetName, size);

    public float ResolutionCoefficient => .5f * (ViewportSize.X / 1280) + .5f * (ViewportSize.Y / 720);

    public Vector2 ViewportSize => new(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

    public Vector2 ViewportCenter => ViewportSize / 2;

    public void ChangeVideoMode()
    {
        _needChangeVideoMode = true;
        SceneManager.LoadSceneAsync(0).ConfigureAwait(false);
    }

    internal SpriteFont DebugFont;
    internal SpriteBatch DrawSpace;
    internal Texture2D LoadingScreenBackground { get; private set; }

    protected override void Initialize()
    {
        _graphics.IsFullScreen = false;
        _graphics.PreferredBackBufferWidth = 1280;
        _graphics.PreferredBackBufferHeight = 720;
        _graphics.ApplyChanges();
        EventSystem.OnExit += Exit;
        OnAfterUpdate += EventSystem.Update;
        base.Initialize();
    }

    protected override void LoadContent()
    {
        DrawSpace = new SpriteBatch(GraphicsDevice);
        DebugFont = Content.Load<SpriteFont>("DebugFont");
        if (!string.IsNullOrEmpty(_loadingScreenBackgroundAssetName))
            LoadingScreenBackground = Content.Load<Texture2D>(_loadingScreenBackgroundAssetName);
        SceneManager.LoadScene(0);
    }

    protected override void Update(GameTime gameTime)
    {
        ActualGameTime = gameTime;
        OnBeforeUpdate?.Invoke();
        SceneManager.CurrentScene?.Update();
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
        SceneManager.CurrentScene?.Draw();
        OnAfterDraw?.Invoke();
        base.Draw(gameTime);
    }

    private readonly GraphicsDeviceManager _graphics;
    private bool _needChangeVideoMode;
    private string _loadingScreenBackgroundAssetName;
}
