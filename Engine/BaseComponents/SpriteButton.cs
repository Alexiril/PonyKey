using System;
using System.Collections.Generic;
using Engine.BaseSystems;
using Engine.BaseTypes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Engine.BaseComponents;

public class SpriteButton : Component
{
    public Action<MouseState> OnPointerUp
    {
        get => _onPointerUp;
        set
        {
            _onPointerUp = value;
            EventSystem.AddOnceTimeEvent(() => true, _ => GetComponent<InputTrigger>().OnPointerUp += _onPointerUp);
        }
    }

    public SpriteButton SetOnPointerUp(Action<MouseState> action)
    {
        OnPointerUp = action;
        return this;
    }

    public override void Start()
    {
        if (!GameObject.HasComponent<InputTrigger>())
            throw new ArgumentException($"Input trigger was not found in the game object '{GameObject.ObjectName}'");
        if (!GameObject.HasComponent<Sprite>()) return;
        GetComponent<InputTrigger>().OnPointerDown += _ => Sprite.TextureColor = new Color(255, 200, 255, 255);
        GetComponent<InputTrigger>().OnPointerOver += _ => Sprite.TextureColor = new Color(255, 230, 255, 255);
        GetComponent<InputTrigger>().OnPointerExit += _ => Sprite.TextureColor = Color.White;
    }

    private Action<MouseState> _onPointerUp;

    protected override List<Type> Requirements => new() { typeof(InputTrigger), typeof(Sprite) };

    protected override void Initiate()
    {
        GetComponent<InputTrigger>().SetTriggerSizeFromSprite();
    }
}
