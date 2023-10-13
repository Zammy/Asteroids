using UnityEngine;

[CreateAssetMenu(menuName = "Game/Settings", fileName = "GameSettings")]
public class Settings : ScriptableObject
{
    public PlayerSettings Player;
    public ProjectileSettings Projectile;
    public AsteroidSettings Asteroids;
    public VFXSettings VFX;
}

[System.Serializable]
public class PlayerSettings
{
    public float Speed = 1f;
    public float BackSpeedPart = .5f;
    public float AngularSpeed = 1f;
    public float FireRate = 1f;
    public float WrapMargin = 1f;
    public float InvurnabilityTime = 3f;
    public int Lives = 3;
}

[System.Serializable]
public class ProjectileSettings
{
    public GameObject ProjectilePrefab;
    public float Speed = 1f;
    public float Lifetime = 1f;
}

[System.Serializable]
public class AsteroidSettings
{
    public GameObject[] Prefabs;
    public float[] WrapMargins;
    public Vector2[] Speeds;
    public int[] Score;
    public float[] SplitAngles;

    public float SpawnTimer = 1f;
    public float MaximumBigSizeAsteroids = 50;
    public float[] Torques;
}

[System.Serializable]
public class VFXSettings
{
    public GameObject[] VFXPrefabs;
    public float[] Lifetimes;
}

