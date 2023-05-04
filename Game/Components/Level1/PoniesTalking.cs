using System;
using System.Collections.Generic;
using Engine.BaseComponents;
using Engine.BaseSystems;
using Engine.BaseTypes;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game.Components.Level1;

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
                        .SetFont(EngineContent.LoadContent<SpriteFont>("Common/TwilightSpeechFont18"))
                        .SetOffset(new(-115, -120));
                    _state++;
                    break;
                case 1:
                    GameObject.Find("PonyTalking")[0]
                        .GetComponent<Sprite>()
                        .SetTexture(EngineContent.LoadSvg("Common/ApplejackAsking",
                            new Vector2(600, 600) * Engine.BaseSystems.Game.ResolutionCoefficient));
                    GetComponent<TextMesh>()
                        .SetText("Yeah, I'd not mind helping. Yee-haw!")
                        .SetFont(EngineContent.LoadContent<SpriteFont>("Common/TwilightSpeechFont21"))
                        .SetOffset(new(-115, -90))
                        .SetWidth(250)
                        .SetColor(Color.Orange);
                    _state++;
                    break;
                case 2:
                    GameObject.Find("HelperText")[0].SetActive(true);
                    GameObject.Find("TreesGenerator")[0].SetActive(true);
                    GameObject.Find("ScorePanel")[0].SetActive(true);
                    GameObject.Find("AJRunning")[0].GetComponent<ApplejackRunning>().StartPlaying = true;
                    GameObject.Find("PonyTalking")[0].Destroy();
                    GameObject.Destroy();
                    break;
            }
        };
    }

    protected override List<Type> Requirements => new() { typeof(InputTrigger), typeof(TextMesh) };

    private int _state;
}
