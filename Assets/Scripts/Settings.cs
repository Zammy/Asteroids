using UnityEngine;

[CreateAssetMenu(menuName = "Game/Settings", fileName = "GameSettings")]
public class Settings : ScriptableObject
{
    public PlayerSettings Player;
    public ProjectileSettings Projectile;
}

[System.Serializable]
public class PlayerSettings
{
    public float Speed = 1f;
    public float AngularSpeed = 1f;
    public float FireRate = 1f;
    public float WrapMargin = 1f;
}

[System.Serializable]
public class ProjectileSettings
{
    public GameObject ProjectilePrefab;
    public float Speed = 1f;
    public float Lifetime = 1f;
}