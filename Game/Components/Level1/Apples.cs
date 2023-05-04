using System;
using Engine.BaseSystems;
using Engine.BaseTypes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game.Components.Level1;

public class Apples : Component
{
    public Apples()
    {
    }

    public Apples(Apples apples) : base(apples) => _textures = apples._textures;

    public Apples SetTextures(Texture2D[] textures)
    {
        _textures = textures;
        return this;
    }

    public override void Start()
    {
        var random = new Random();
        Sprite.Texture = TextureGenerator.GenerateTexture(
            (int)(400 * Engine.BaseSystems.Game.ResolutionCoefficient),
            (int)(400 * Engine.BaseSystems.Game.ResolutionCoefficient),
            (_, _) => Color.Transparent);
        for(var i = 0; i < 3; i++)
            foreach (var texture in _textures)
                Sprite.AppendTexture(texture, new(
                    random.Next((int)(50 * Engine.BaseSystems.Game.ResolutionCoefficient),
                        (int)(350 * Engine.BaseSystems.Game.ResolutionCoefficient)),
                    random.Next((int)(50 * Engine.BaseSystems.Game.ResolutionCoefficient),
                        (int)(350 * Engine.BaseSystems.Game.ResolutionCoefficient))));
    }

    public override void Update()
    {
        Transform.Position -= Transform.Up * DeltaTime * .7f;
        if (Transform.Position.Y > Engine.BaseSystems.Game.ViewportSize.Y * 1.25f)
            GameObject.Destroy();
    }

    private Texture2D[] _textures;
}
