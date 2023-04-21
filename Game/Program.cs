using Engine.BaseSystems;
using Game.Levels;

using var game = new Master();
game.SceneManager.RegisterLevels(new()
{
    new MainMenu(),
    new Level0()
});
game.SetLoadingScreenBackground("Common/LoadingSceneBackground.svg");
game.Run();
