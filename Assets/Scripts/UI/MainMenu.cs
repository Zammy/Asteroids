using SSLAB;
using TMPro;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] TextMeshProUGUI BestScoreText;

    void Start()
    {
        var sl = ServiceLocator.Instance;
        _gameStateManager = sl.GetService<IGameStateManager>();
        _inputService = sl.GetService<IInputService>();

        _gameStateManager.OnStateChanged += OnStateChanged;
    }

    private void OnDestroy()
    {
        _gameStateManager.OnStateChanged -= OnStateChanged;
    }

    private void Update()
    {
        if (_inputService.IsAnyButtonPressed())
        {
            _gameStateManager.ChangeStateTo(GameState.Game);
        }
    }

    private void OnStateChanged(GameState currentState)
    {
        if (currentState != GameState.MainMenu)
        {
            gameObject.SetActive(false);
            return;
        }
        gameObject.SetActive(true);

        BestScoreText.text = PlayerPrefs.GetInt(Player.BEST_SCORE_KEY, 0).ToString();
    }

    IGameStateManager _gameStateManager;
    IInputService _inputService;
}
