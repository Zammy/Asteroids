using SSLAB;
using TMPro;
using UnityEngine;

public class PlayerLivesWidget : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] TextMeshProUGUI ScoreText;

    void Start()
    {
        _player = ServiceLocator.Instance.GetService<IPlayer>();
        _player.OnLivesChanged += OnPlayerLivesChanged;
    }

    private void OnDestroy()
    {
        _player.OnLivesChanged -= OnPlayerLivesChanged;
    }

    private void OnPlayerLivesChanged()
    {
        ScoreText.text = string.Format("x{0}", _player.Lives);
    }


    IPlayer _player;
}
