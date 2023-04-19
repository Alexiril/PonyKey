using System.Diagnostics;
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
        EventSystem.OnExit += Exit;
        OnAfterUpdate += EventSystem.Update;
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
        if (_graphics.IsFullScreen)
            PlayerSettings.SetValues(new()
            {
                { "fS", "false" },
                { "bW", "1280" },
                { "bH", "720" }
            });
        else
            PlayerSettings.SetValues(new()
            {
                {"fS", "true"},
                {"bW", GraphicsDevice.DisplayMode.Width.ToString()},
                {"bH", GraphicsDevice.DisplayMode.Height.ToString()}
            });
        var p = new Process();
        p.StartInfo.FileName = Process.GetCurrentProcess().MainModule?.FileName ?? string.Empty;
        p.Start();
        Exit();
    }

    internal SpriteFont DebugFont;
    internal SpriteBatch DrawSpace;
    internal Texture2D LoadingScreenBackground { get; private set; }

    protected override void Initialize()
    {
        PlayerSettings.ForceUpdate();
        _graphics.IsFullScreen = bool.TryParse(PlayerSettings.GetValue("fS"), out var fS) && fS;
        _graphics.PreferredBackBufferWidth = int.TryParse(PlayerSettings.GetValue("bW"), out var bW) ? bW : 1280;
        _graphics.PreferredBackBufferHeight = int.TryParse(PlayerSettings.GetValue("bH"), out var bH) ? bH : 720;
        _graphics.ApplyChanges();
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
    private string _loadingScreenBackgroundAssetName;
}
