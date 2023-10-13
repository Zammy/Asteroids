using SSLAB;
using TMPro;
using UnityEngine;

public class PlayerScoreWidget : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] TextMeshProUGUI ScoreText;

    // Start is called before the first frame update
    void Start()
    {
        _player = ServiceLocator.Instance.GetService<IPlayer>();
        _player.OnScoreChanged += OnScoreChanged;
    }

    private void OnDestroy()
    {
        _player.OnScoreChanged -= OnScoreChanged;
    }

    private void OnScoreChanged()
    {
        ScoreText.text = _player.Score.ToString();
    }

    IPlayer _player;
}
