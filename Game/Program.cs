using Engine.BaseSystems;
using Game.Levels;

using var game = new ActualGame();
game.SceneManager.RegisterLevels(new()
{
    new MainMenu(),
    new Level0()
});
game.SetLoadingScreenBackground("Common/LoadingSceneBackground");
game.Run();
