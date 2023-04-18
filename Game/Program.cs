using Game.Levels;

using var game = new Engine.ActualGame();
game.SceneManager.RegisterLevels(new()
{
    new MainMenu(),
    new Level0()
} );
game.SceneManager.LoadScene(0);
game.Run();
