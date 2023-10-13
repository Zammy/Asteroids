using SSLAB;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum VFXType : int
{
    Explosion = 0,
    Debris,
    Bump,
}

public interface IVFXService : IService, IInitializable
{
    void PlayVFXAt(VFXType type, Vector2 pos);
}

public class VFXService : IVFXService
{
    public void Init()
    {
        var sl = ServiceLocator.Instance;
        _coroutineRunner = sl.GetService<ICoroutineRunner>();
        _objSpawner = sl.GetService<IObjSpawnerService>();

        _settings = sl.GetService<ISettingsService>().Settings.VFX;

        var values = Enum.GetValues(typeof(VFXType));
        _vfxCache = new Queue<GameObject>[values.Length];
        for (int i = 0; i < values.Length; i++)
        {
            _vfxCache[i] = new Queue<GameObject>();
        }
    }

    public void PlayVFXAt(VFXType type, Vector2 pos)
    {
        var vfxGo = GetVFX(type);
        vfxGo.transform.position = pos;
        vfxGo.gameObject.SetActive(true);

        float vfxDuration = _settings.Lifetimes[(int)type];
        _coroutineRunner.StartCoroutine(DoPlayVFXFor(vfxGo, type, vfxDuration));

        if (type == VFXType.Debris)
        {
            vfxGo.transform.localRotation = UnityEngine.Random.rotationUniform;
            _coroutineRunner.StartCoroutine(DoPlayFadeOutVFX(vfxGo, vfxDuration));
        }
        else if (type == VFXType.Bump)
        {
            _coroutineRunner.StartCoroutine(DoPlayFadeOutVFX(vfxGo, vfxDuration));
            _coroutineRunner.StartCoroutine(DoPlayZoomOutVFX(vfxGo, vfxDuration));
        }
    }

    GameObject GetVFX(VFXType type)
    {
        GameObject vfxGo;

        if (_vfxCache[(int)type].Count > 0)
        {
            vfxGo = _vfxCache[(int)type].Dequeue();
            vfxGo.SetActive(true);
        }
        else
        {
            vfxGo = _objSpawner.Spawn(_settings.VFXPrefabs[(int)type]);
        }

        return vfxGo;
    }

    IEnumerator DoPlayVFXFor(GameObject vfxGo, VFXType type, float duration)
    {
        yield return new WaitForSeconds(duration);
        vfxGo.SetActive(false);
        _vfxCache[(int)type].Enqueue(vfxGo);
    }

    IEnumerator DoPlayFadeOutVFX(GameObject vfxGo, float duration)
    {
        var renderer = vfxGo.GetComponentInChildren<Renderer>();
        float alpha = 1f;
        for (float t = 0.0f; t < 1f; t += Time.deltaTime / duration)
        {
            Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, 0f, t));
            renderer.material.color = newColor;
            yield return null;
        }
    }

    IEnumerator DoPlayZoomOutVFX(GameObject vfxGo, float duration)
    {
        var transform = vfxGo.transform;
        Vector3 scale = transform.localScale;
        Vector3 startScale = scale;
        Vector3 endScale = Vector2.one * 2;
        for (float t = 0.0f; t < 1f; t += Time.deltaTime / duration)
        {
            scale = Vector3.Lerp(startScale, endScale, t);
            transform.localScale = scale;
            yield return null;
        }
    }

    VFXSettings _settings;

    ICoroutineRunner _coroutineRunner;
    IObjSpawnerService _objSpawner;

    Queue<GameObject>[] _vfxCache;
}
