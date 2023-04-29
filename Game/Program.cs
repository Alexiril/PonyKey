using Engine.BaseSystems;
using Game.Levels;

using var game = new Master(144, true, screenBackgroundAssetName:"Common/LoadingSceneBackground.svg");
game.SceneManager.RegisterLevels(new()
{
    new MainMenu(),
    new Level0()
});
game.Run();
