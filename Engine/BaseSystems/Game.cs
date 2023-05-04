using System;
using System.Diagnostics;
using System.Linq;
using Engine.Scenes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.BaseSystems;

public static class Game
{
    public static event ProgramState OnBeforeUpdate = () => {};
    public static event ProgramState OnAfterUpdate = () => {};
    public static event ProgramState OnBeforeDraw = () => {};
    public static event ProgramState OnAfterDraw = () => {};

    public static GameTime GameTime => Master.ActualGameTime;

    public static void Init(int framerate = 60, bool fixedTimeStep = false, string screenBackgroundAssetName = null)
    {
        Master.IsFixedTimeStep = fixedTimeStep;
        Master.TargetElapsedTime = TimeSpan.FromSeconds(1d / framerate);
        _loadingScreenBackgroundAssetName = screenBackgroundAssetName;
        SceneManager.RegisterLevels(new() { new StartScene() });
        EventSystem.OnExit += Exit;
        PlayerSettings.ForceUpdate();
        Graphics.IsFullScreen = bool.TryParse(PlayerSettings.GetValue("fS"), out var fS) && fS;
        Graphics.PreferredBackBufferWidth = int.TryParse(PlayerSettings.GetValue("bW"), out var bW) ? bW : 1280;
        Graphics.PreferredBackBufferHeight = int.TryParse(PlayerSettings.GetValue("bH"), out var bH) ? bH : 720;
        Graphics.ApplyChanges();
        DrawSpace = new SpriteBatch(GraphicsDevice);
        DebugFont = EngineContent.LoadContent<SpriteFont>( "DebugFont");
        if (!string.IsNullOrEmpty(_loadingScreenBackgroundAssetName))
            LoadingScreenBackground = _loadingScreenBackgroundAssetName.Split(".").Last() == "svg"
                ? EngineContent.LoadSvg(_loadingScreenBackgroundAssetName.Split(".")[0], ViewportSize)
                : EngineContent.LoadContent<Texture2D>( _loadingScreenBackgroundAssetName);
        Master.OnBeforeUpdate += OnBeforeUpdate.Invoke;
        Master.OnBeforeDraw += OnBeforeDraw.Invoke;
        Master.OnAfterUpdate += OnAfterUpdate.Invoke;
        Master.OnAfterDraw += OnAfterDraw.Invoke;
        Master.OnAfterUpdate += EventSystem.Update;
        Master.OnAfterDraw += SceneManager.Update;
#if DEBUG
        EventSystem.OnToggleDebugInformation += () => DebugInformationOn = !DebugInformationOn;
        EventSystem.OnToggleDebugBoxes += () => DebugBoxesOn = !DebugBoxesOn;
        EventSystem.OnToggleDebugPoints += () => DebugPointsOn = !DebugPointsOn;
#endif
        SceneManager.LoadScene(0);
    }

    public static void Run() => Master.Run();

    public static float ResolutionCoefficient => .5f * (ViewportSize.X / 1280) + .5f * (ViewportSize.Y / 720);

    public static Vector2 ViewportSize => new(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

    public static Vector2 ViewportCenter => ViewportSize / 2;

    public static void Exit() => Master.Exit();

    public static GameWindow Window => Master.Window;

    public static void ChangeVideoMode()
    {
        if (Graphics.IsFullScreen)
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

    internal static SpriteFont DebugFont;
    internal static SpriteBatch DrawSpace;

    internal static ContentManager Content => Master.Content;

    internal static GraphicsDevice GraphicsDevice => Master.GraphicsDevice;

    internal static GraphicsDeviceManager Graphics => Master.Graphics;

    internal static Texture2D LoadingScreenBackground { get; private set; }

#if DEBUG

    internal static bool DebugInformationOn { get; private set; } = true;
    internal static bool DebugBoxesOn { get; private set; } = true;
    internal static bool DebugPointsOn { get; private set; } = true;

#endif

    private static readonly Master Master = new();
    private static string _loadingScreenBackgroundAssetName;
}
