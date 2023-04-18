using Engine.BaseSystems;
using Game.Levels;

using var game = new ActualGame();
game.SceneManager.RegisterLevels(new()
{
    new MainMenu(),
    new Level0()
} );

// var fileName = System.Diagnostics.Process.GetCurrentProcess().MainModule?.FileName ?? string.Empty;
// var handle = System.Diagnostics.Process.GetCurrentProcess().MainWindowHandle;
// var actualHandle = game.Window.Handle;
// ((Form)Control.FromHandle(game.Window.Handle)).Icon = Icon.ExtractAssociatedIcon(fileName);
game.SceneManager.LoadScene(0);
game.Run();
