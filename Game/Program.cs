using Game.Levels;
using static Engine.BaseSystems.Game;
using static Engine.BaseSystems.SceneManager;

Init(144, true, "Common/LoadingSceneBackground.svg");
RegisterLevels(new() { new MainMenu(), new Level0() });
Run();
