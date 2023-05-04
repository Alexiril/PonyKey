using Game.Levels;
using static Engine.BaseSystems.Game;
using static Engine.BaseSystems.SceneManager;

Init(framerate: 144, fixedTimeStep: true, screenBackgroundAssetName: "Common/LoadingSceneBackground.svg");
RegisterLevels(new() { new MainMenu(), new Level1() });
Run();
