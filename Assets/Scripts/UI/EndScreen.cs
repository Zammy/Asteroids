using SSLAB;
using TMPro;
using UnityEngine;

public class EndScreen : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] TextMeshProUGUI ScoreText;
    [SerializeField] GameObject NewBestScoreText;

    void Start()
    {
        var sl = ServiceLocator.Instance;
        _gameStateManager = sl.GetService<IGameStateManager>();
        _inputService = sl.GetService<IInputService>();
        _player = sl.GetService<IPlayer>();

        _gameStateManager.OnStateChanged += OnStateChanged;

        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        _gameStateManager.OnStateChanged -= OnStateChanged;
    }

    private void Update()
    {
        if (_inputService.IsAnyButtonPressed())
        {
            _gameStateManager.ChangeStateTo(GameState.MainMenu);
        }
    }

    private void OnStateChanged(GameState currentState)
    {
        if (currentState != GameState.EndGame)
        {
            gameObject.SetActive(false);
            return;
        }
        gameObject.SetActive(true);

        SetPlayerScore();
    }

    private void SetPlayerScore()
    {
        int score = _player.Score;
        ScoreText.text = score.ToString();

        int bestScore = PlayerPrefs.GetInt(Player.BEST_SCORE_KEY, 0);
        if (score > bestScore)
        {
            PlayerPrefs.SetInt(Player.BEST_SCORE_KEY, score);
            PlayerPrefs.Save();
            NewBestScoreText.SetActive(true);
        }
        else
        {
            NewBestScoreText.SetActive(false);
        }
    }

    IGameStateManager _gameStateManager;
    IInputService _inputService;
    IPlayer _player;
}
