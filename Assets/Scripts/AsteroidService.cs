using SSLAB;
using System.Collections.Generic;
using UnityEngine;

public interface IAsteroidService : IService, ITickable, IInitializable, IEnablable
{
    void AsteroidHit(Asteroid asteroid, Vector2 hit);
}

public class AsteroidService : IAsteroidService
{
    public bool IsEnabled
    {
        get => _isEnabled;
        set
        {
            _isEnabled = value;
        }
    }

    public void Init()
    {
        var sl = ServiceLocator.Instance;
        _objSpawnerService = sl.GetService<IObjSpawnerService>();
        _levelWrapService = sl.GetService<LevelWrapService>();

        _settings = sl.GetService<ISettingsService>()
                .Settings
                .Asteroids;

        _asteroids = new List<Asteroid>[4];
        for (int i = 0; i < 4; i++)
        {
            _asteroids[i] = new List<Asteroid>();
        }
        _limits = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0));
        _spawnTimer = _settings.SpawnTimer[0];
    }

    public void Tick(float deltaTime)
    {
        if (!IsEnabled)
            return;

        TickSpawnTimer(deltaTime);

        ApplyTorque(deltaTime);
    }

    private void TickSpawnTimer(float deltaTime)
    {
        _spawnTimer += deltaTime;

        //TODO: introduce level difficulty
        if (_spawnTimer > _settings.SpawnTimer[0])
        {
            _spawnTimer = 0f;

            for (int i = 0; i < 3; i++)
            {
                SpawnNewAsteroid();
            }
        }
    }

    private void SpawnNewAsteroid()
    {
        var asteroid = GetAsteroidLevel(3);
        asteroid.Pos = GenRndPosOnEdge(3);
        float speed = UnityEngine.Random.Range(_settings.Speeds[3].x, _settings.Speeds[3].y);
        asteroid.SetVelocity(UnityEngine.Random.insideUnitCircle * speed);
        asteroid.Torque = UnityEngine.Random.Range(-_settings.Torques[3], _settings.Torques[3]);
    }

    private void ApplyTorque(float deltaTime)
    {
        for (int i = 0; i < 4; i++)
        {
            for (int y = 0; y < _asteroids[i].Count; y++)
            {
                var asteroid = _asteroids[i][y];
                asteroid.ApplyTorque(deltaTime);
            }
        }
    }


    public void AsteroidHit(Asteroid asteroid, Vector2 hit)
    {
        int size = asteroid.Size;
        _asteroids[size].Remove(asteroid);
        asteroid.gameObject.SetActive(false);
        _levelWrapService.UnregisterWrappable(asteroid);

        if (asteroid.Size == 0)
        {
            //TODO: spawn vfx
            return;
        }

        int newSize = size - 1;
        for (int i = 0; i < 2; i++)
        {
            float angle = UnityEngine.Random.Range(-_settings.SplitAngles[newSize], _settings.SplitAngles[newSize]);
            Vector2 direction = Quaternion.AngleAxis(angle, Vector3.forward) * hit;

            var newAsteroid = GetAsteroidLevel(newSize);
            newAsteroid.Pos = asteroid.Pos;

            float speed = UnityEngine.Random.Range(_settings.Speeds[newSize].x, _settings.Speeds[newSize].y);
            newAsteroid.SetVelocity(direction * speed);

            float torque = asteroid.Torque;
            if (torque > 0f)
                torque += UnityEngine.Random.Range(0, _settings.Torques[newSize]);
            else
                torque -= UnityEngine.Random.Range(0, _settings.Torques[newSize]);
            newAsteroid.Torque = torque;
        }
    }

    Asteroid GetAsteroidLevel(int size)
    {
        for (int i = 0; i < _asteroids[size].Count; i++)
        {
            var cachedInstance = _asteroids[size][i];
            if (!cachedInstance.gameObject.activeSelf)
            {
                _levelWrapService.RegisterWrappable(cachedInstance);
                return cachedInstance;
            }
        }

        var asteroidObj = _objSpawnerService.Spawn(_settings.Prefabs[size]);
        var asteroid = asteroidObj.GetComponent<Asteroid>();
        asteroid.WrapMargin = _settings.WrapMargins[size];
        asteroid.Size = size;
        _asteroids[size].Add(asteroid);
        _levelWrapService.RegisterWrappable(asteroid);
        return asteroid;
    }

    Vector2 GenRndPosOnEdge(float wrapMargin)
    {
        //TODO: should be a service if code to be made testable
        int side = UnityEngine.Random.Range(0, 4);
        Vector2 pos;
        switch (side)
        {
            case 0:
                {
                    pos.x = UnityEngine.Random.Range(-_limits.x * wrapMargin, _limits.x * wrapMargin);
                    pos.y = -_limits.y;
                    break;
                }
            case 1:
                {
                    pos.x = UnityEngine.Random.Range(-_limits.x * wrapMargin, _limits.x * wrapMargin);
                    pos.y = _limits.y;
                    break;
                }
            case 2:
                {
                    pos.x = _limits.x;
                    pos.y = UnityEngine.Random.Range(-_limits.y * wrapMargin, _limits.y * wrapMargin);
                    break;
                }
            case 3:
                {
                    pos.x = -_limits.x;
                    pos.y = UnityEngine.Random.Range(-_limits.y * wrapMargin, _limits.y * wrapMargin);
                    break;
                }
            default:
                throw new UnityException("Should be Unreachable!");
        }

        return pos;
    }

    AsteroidSettings _settings;
    IObjSpawnerService _objSpawnerService;
    ILevelWrapService _levelWrapService;

    Vector2 _limits;
    List<Asteroid>[] _asteroids;
    bool _isEnabled;
    float _spawnTimer;
}
