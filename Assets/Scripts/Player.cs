using UnityEngine;
using SSLAB;


public class Player : MonoBehaviour, IWrappable
{
    [Header("Refs")]
    [SerializeField] Transform Muzzle;
    [SerializeField] GameObject[] Exaust;

    public Vector2 Pos
    {
        get => transform.position;
        set => transform.position = value;
    }

    public float WrapMargin => _settings.WrapMargin;

    void Awake()
    {
        _body = GetComponent<Rigidbody2D>();
        //ServiceLocator.Instance.RegisterService(this);
    }

    void Start()
    {
        var sl = ServiceLocator.Instance;
        _input = sl.GetService<IInputService>();
        _projectileService = sl.GetService<IProjectileService>();
        sl.GetService<ILevelWrapService>()
            .RegisterWrappable(this);
        _settings = sl.GetService<ISettingsService>().Settings.Player;
    }

    private void Update()
    {
        _shootTimer -= Time.deltaTime;
    }

    void FixedUpdate()
    {
        if (_input.IsPlayerMoveForwardPressed())
        {
            _body.AddForce(transform.up * _settings.Speed * Time.fixedDeltaTime, ForceMode2D.Force);
            AnimateExaust(enable:true);
        }
        else
        {
            AnimateExaust(enable: false);
 
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

    private void AnimateExaust(bool enable)
    {
        for (int i = 0; i < Exaust.Length; i++)
        {
            Exaust[i].SetActive(enable);
        }
    }

    PlayerSettings _settings;
    IProjectileService _projectileService;
    IInputService _input;

    Rigidbody2D _body;

    [Header("Debug")]
    public  float _shootTimer;
}
