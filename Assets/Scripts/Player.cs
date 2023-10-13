using UnityEngine;
using SSLAB;


public class Player : MonoBehaviour, IWrappable
{
    [Header("Refs")]
    [SerializeField] Transform Muzzle;
    [SerializeField] GameObject[] Exaust;
    [SerializeField] GameObject[] BackExaust;
    [SerializeField] GameObject InvurnabilityVFX;

    public Vector2 Pos
    {
        get => transform.position;
        set => transform.position = value;
    }

    public float WrapMargin => _settings.WrapMargin;

    public int Lives;

    void Awake()
    {
        _body = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        var sl = ServiceLocator.Instance;
        _input = sl.GetService<IInputService>();
        _projectileService = sl.GetService<IProjectileService>();
        _asteroidService = sl.GetService<IAsteroidService>();

        sl.GetService<ILevelWrapService>()
            .RegisterWrappable(this);
        _settings = sl.GetService<ISettingsService>().Settings.Player;
        Lives = _settings.Lives;
    }

    private void Update()
    {
        _shootTimer -= Time.deltaTime;

        InvurnabilityVFX.SetActive(_isInvurnableUntil > Time.time);
    }

    void FixedUpdate()
    {
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

        var asteroid = col.collider.GetComponent<Asteroid>();
        if (asteroid)
        {
            Lives--;
            if (Lives == 0)
            {
                //TODO: Implement
                Debug.Log("YOU ARE DEAD BOI!");
            }
            _isInvurnableUntil = Time.time + _settings.InvurnabilityTime;

            _asteroidService.AsteroidHit(asteroid, (asteroid.Pos - Pos).normalized);
        }
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

    Rigidbody2D _body;

    [Header("Debug")]
    public float _shootTimer;
    public float _isInvurnableUntil = -1f;
}
