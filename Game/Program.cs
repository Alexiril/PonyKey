#if !DEBUG
using System;
using System.Windows;
#endif
using static Engine.BaseSystems.Game;
//using static Engine.BaseSystems.SceneManager;

#if !DEBUG
try
{
#endif
    Init(framerate: 144, fixedTimeStep: true, screenBackgroundAssetName: "Common/LoadingSceneBackground.svg");
    //RegisterLevels(new() { new MainMenu(), new Level1() });
    Run();
#if !DEBUG
}
catch(Exception e)
{
    MessageBox.Show(
        $"Sorry, unexpected error has happened: \n\n{e}\n\n Please, contact developers to fix it.",
        "Something has happened",
        MessageBoxButton.OK,
        MessageBoxImage.Warning,
        MessageBoxResult.OK
    );
}
#endif
