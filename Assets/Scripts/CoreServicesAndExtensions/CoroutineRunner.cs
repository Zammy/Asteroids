using SSLAB;
using System.Collections;
using UnityEngine;

public interface ICoroutineRunner : IService
{
    public Coroutine StartCoroutine(IEnumerator routine);
    public void StopCoroutine(Coroutine routine);
}
public class CoroutineRunner : MonoBehaviour, ICoroutineRunner
{
    void Awake()
    {
        ServiceLocator.Instance.RegisterService(this);
    }
    void OnDestroy()
    {
        ServiceLocator.Instance.UnregisterService(this);
    }
}