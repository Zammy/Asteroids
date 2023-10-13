using UnityEngine;
using SSLAB;
using System;

public interface IPlayer : IWrappable, IEnablable, IService
{
    int Lives { get; }
    int Score { get; set; }
    Action OnLivesChanged { get; set; }
    Action OnScoreChanged { get; set; }
}

public class Player : MonoBehaviour, IPlayer, IWrappable
{
    public const string BEST_SCORE_KEY = "BestScore";

    [Header("Refs")]
    [SerializeField] Transform Muzzle;
    [SerializeField] GameObject[] Exaust;
    [SerializeField] GameObject[] BackExaust;
    [SerializeField] GameObject InvurnabilityVFX;

    public bool IsEnabled { get; set; }

    public Vector2 Pos
    {
        get => transform.position;
        set => transform.position = value;
    }

    public float WrapMargin => _settings.WrapMargin;

    public Action OnLivesChanged { get; set; }
    public int Lives { get; private set; }

    public Action OnScoreChanged { get; set; }
    public int Score { get; set; }

    void Awake()
    {
        _body = GetComponent<Rigidbody2D>();

        ServiceLocator.Instance.RegisterService(this);
    }

    void Start()
    {
        var sl = ServiceLocator.Instance;
        _input = sl.GetService<IInputService>();
        _projectileService = sl.GetService<IProjectileService>();
        _asteroidService = sl.GetService<IAsteroidService>();
        _vfxService = sl.GetService<IVFXService>();
        _gameStateManager = sl.GetService<IGameStateManager>();

        _settings = sl.GetService<ISettingsService>().Settings.Player;

        sl.GetService<ILevelWrapService>().RegisterWrappable(this);

        _asteroidService.OnAsteriodHit += OnAsteroidHit;
        _gameStateManager.OnStateChanged += OnStateChanged;
    }

    void OnDestroy()
    {
        ServiceLocator.Instance.UnregisterService(this);
        _asteroidService.OnAsteriodHit -= OnAsteroidHit;
        _gameStateManager.OnStateChanged -= OnStateChanged;
    }

    private void Update()
    {
        if (!IsEnabled)
            return;

        _shootTimer -= Time.deltaTime;

        InvurnabilityVFX.SetActive(_isInvurnableUntil > Time.time);
    }

    void FixedUpdate()
    {
        if (!IsEnabled)
            return;

        if (_input.IsPlayerMoveForwardPressed())
        {
            _body.AddForce(transform.up * _settings.Speed * Time.fixedDeltaTime, ForceMode2D.Force);
            AnimateExaust(Exaust, enable: true);
        }
        else
        {
            AnimateExaust(Exaust, enable: false);
        }

        if (_input.IsPlayerMoveBackwardPressed())
        {
            _body.AddForce(-transform.up * _settings.BackSpeedPart * _settings.Speed * Time.fixedDeltaTime, ForceMode2D.Force);
            AnimateExaust(BackExaust, enable: true);
        }
        else
        {
            AnimateExaust(BackExaust, enable: false);
        }

        if (_input.IsPlayerTurnLeftPressed())
        {
            _body.AddTorque(_settings.AngularSpeed * Time.fixedDeltaTime);
        }
        if (_input.IsPlayerTurnRightPressed())
        {
            _body.AddTorque(-_settings.AngularSpeed * Time.fixedDeltaTime);
        }

        if (_shootTimer < 0 && _input.IsPlayerShootPressed())
        {
            _projectileService.SpawnProjectile(Muzzle.position, Muzzle.up);
            _shootTimer = _settings.FireRate;
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (_isInvurnableUntil > Time.time)
            return;

        if (Lives < 0)
            return;

        var asteroid = col.collider.GetComponent<Asteroid>();
        if (asteroid)
        {
            Lives--;
            this?.OnLivesChanged();
            if (Lives < 0)
            {
                _gameStateManager.ChangeStateTo(GameState.EndGame);
                return;
            }
            else
            {
                _vfxService.PlayVFXAt(VFXType.Bump, col.contacts[0].point);
            }
            _isInvurnableUntil = Time.time + _settings.InvurnabilityTime;

            _asteroidService.AsteroidHit(asteroid, (asteroid.Pos - Pos).normalized);
        }
    }

    private void OnStateChanged(GameState currentState)
    {
        if (currentState == GameState.Game)
        {
            IsEnabled = true;

            Reset();
            return;
        }

        AnimateExaust(Exaust, enable: false);
        AnimateExaust(BackExaust, enable: false);
        InvurnabilityVFX.SetActive(false);

        IsEnabled = false;
    }

    private void Reset()
    {
        transform.position = Vector2.zero;
        transform.up = Vector2.up;
        Lives = _settings.Lives;
        Score = 0;
        this?.OnLivesChanged();
        this?.OnScoreChanged();
    }

    private void OnAsteroidHit(int score)
    {
        Score += score;
        this?.OnScoreChanged();
    }

    private void AnimateExaust(GameObject[] exaust, bool enable)
    {
        for (int i = 0; i < exaust.Length; i++)
        {
            exaust[i].SetActive(enable);
        }
    }

    PlayerSettings _settings;
    IProjectileService _projectileService;
    IInputService _input;
    IAsteroidService _asteroidService;
    IVFXService _vfxService;
    IGameStateManager _gameStateManager;

    Rigidbody2D _body;

    [Header("Debug")]
    public float _shootTimer;
    public float _isInvurnableUntil = -1f;
}
