using UnityEngine;
using SSLAB;

public interface ISettingsService : IService
{
    Settings Settings { get; }
}

public class SettingsService : MonoBehaviour, ISettingsService
{
    [Header("Settings")]
    public Settings settings;

    public Settings Settings => settings;

    void Awake()
    {
        ServiceLocator.Instance.RegisterService(this);
    }

    void OnDestroy()
    {
        ServiceLocator.Instance.UnregisterService(this);
    }
}