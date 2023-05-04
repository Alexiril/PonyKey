using Engine.BaseComponents;
using Engine.BaseSystems;
using Engine.BaseTypes;

namespace Game.Components.Level1;

public class Score: Component
{
    public uint PlayerScore
    {
        get => _playerScore;
        set
        {
            _playerScore = value;
            _scorePanel.GetComponent<TextMesh>().Text = $"Score: {value}";
        }
    }

    public override void Start()
    {
        SceneManager.DontDestroyOnLoad(GameObject);
        _scorePanel = GameObject.Find("ScorePanel")[0];
    }

    private uint _playerScore;
    private GameObject _scorePanel;
}
