using System.Windows.Forms;
using static Engine.BaseSystems.Game;

Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);

Init(144, true, "Common/LoadingSceneBackground.svg");
//RegisterLevels(Game.BuildTime.BuildingScenes.Scenes);
Run();
