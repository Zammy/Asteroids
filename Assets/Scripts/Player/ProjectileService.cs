using System.Collections.Generic;
using SSLAB;
using UnityEngine;

public interface IProjectileService : IService, IInitializable, ITickableFixed
{
    void SpawnProjectile(Vector2 pos, Vector2 dir);
}

public class ProjectileService : IProjectileService
{
    public void Init()
    {
        _projectiles = new List<Projectile>();
        var sl = ServiceLocator.Instance;
        _settings = sl.GetService<ISettingsService>().Settings.Projectile;
        _objSpawnerService = sl.GetService<IObjSpawnerService>();
        _asteroidService = sl.GetService<IAsteroidService>();
        _vfxService = sl.GetService<IVFXService>();
    }

    public void TickFixed(float fixedDeltaTime)
    {
        for (int i = 0; i < _projectiles.Count; i++)
        {
            var projectile = _projectiles[i];
            projectile.Life -= fixedDeltaTime;
            if (projectile.Life < 0f)
            {
                //Has hit an asteroid
                if (projectile.Life < -99f)
                {
                    _vfxService.PlayVFXAt(VFXType.Explosion, projectile.transform.position);
                }
                //TODO: possible optimization to cache instances
                _objSpawnerService.Destroy(projectile.gameObject);
                _projectiles.Remove(projectile);
            }
        }
    }

    public void SpawnProjectile(Vector2 pos, Vector2 dir)
    {
        var projGo = _objSpawnerService.SpawnAt(_settings.ProjectilePrefab, pos);
        var proj = projGo.GetComponent<Projectile>();
        proj.transform.up = dir;
        proj.Life = _settings.Lifetime;
        proj.AsteroidService = _asteroidService;
        proj.SetVelocity(dir * _settings.Speed);
        _projectiles.Add(proj);
    }

    ProjectileSettings _settings;
    IObjSpawnerService _objSpawnerService;
    IAsteroidService _asteroidService;
    IVFXService _vfxService;

    List<Projectile> _projectiles;
}