using System;
using System.Collections.Generic;
using Engine.BaseComponents;
using Engine.BaseTypes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game.Components.Level0;

internal class PoniesTalking : Component
{
    public override void Start()
    {
        GetComponent<InputTrigger>().OnPointerUp += _ =>
        {
            switch (_state)
            {
                case 0:
                    GetComponent<TextMesh>()
                        .SetText("Applejack wants many apples to prepare for a rodeo. Can you help her?")
                        .SetFont(ActualGame.Content.Load<SpriteFont>("Common/TwilightSpeechFont18"))
                        .SetOffset(new(-115, -103));
                    _state++;
                    break;
                case 1:
                    GameObject.Find("PonyTalking")[0]
                        .GetComponent<Sprite>()
                        .SetTexture(ActualGame.LoadSvg("Common/ApplejackAsking",
                            new Vector2(600, 600) * ActualGame.ResolutionCoefficient));
                    GetComponent<TextMesh>()
                        .SetText("Yeah, I'd not mind helping. Yee-haw!")
                        .SetFont(ActualGame.Content.Load<SpriteFont>("Common/TwilightSpeechFont21"))
                        .SetOffset(new(-115, -90))
                        .SetWidth(270)
                        .SetColor(Color.Orange);
                    _state++;
                    break;
                case 2:
                    GameObject.Find("HelperText")[0].SetActive(true);
                    ActualScene.DestroyGameObject(GameObject.Find("PonyTalking")[0]);
                    GameObject.Destroy();
                    break;
            }
        };
    }

    protected override List<Type> Requirements => new() { typeof(InputTrigger), typeof(TextMesh) };

    private int _state;
}
