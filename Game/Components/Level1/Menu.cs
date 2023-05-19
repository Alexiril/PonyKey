using Engine.BaseComponents;
using Engine.BaseTypes;
using Microsoft.Xna.Framework.Input;

namespace Game.Components.Level1;

public class Menu : Component
{
    public override void Start()
    {
        GameObject.OnActivate += StopEmAll;
        GameObject.OnDeactivate += NoSorryRunEmAll;
        GameObject.GetComponent<SpriteButton>().PointerUp = _ => Engine.BaseSystems.Game.Exit();
    }

    public override void Update()
    {
        if (Keyboard.GetState().IsKeyDown(Keys.Escape) && _previousState.IsKeyUp(Keys.Escape))
            GameObject.SetActive(false);

        _previousState = Keyboard.GetState();
    }

    private void StopEmAll(GameObject _)
    {
        for (var i = 0; i < ActualScene.GameObjectsCount; i++)
        {
            var obj = GameObject.GetGameObjectByIndex(i);
            if (obj != GameObject) obj.SetActive(false);
        }
    }

    private void NoSorryRunEmAll(GameObject _)
    {
        for (var i = 0; i < ActualScene.GameObjectsCount; i++)
        {
            var obj = GameObject.GetGameObjectByIndex(i);
            if (obj != GameObject) obj.SetActive(true);
        }
    }

    private KeyboardState _previousState = Keyboard.GetState();
}
