using UnityEngine;
using SSLAB;

public interface IObjSpawnerService : IService, IInitializable
{
    void Destroy(GameObject gameObject);
    GameObject SpawnAt(GameObject prefab, Vector2 pos);
}

public class ObjSpawnerService : IObjSpawnerService
{

    public void Init()
    {

    }

    public GameObject SpawnAt(GameObject prefab, Vector2 pos)
    {
        return Object.Instantiate(prefab, (Vector3)pos, Quaternion.identity);
    }

    public void Destroy(GameObject gameObject)
    {
        Object.Destroy(gameObject);
    }

}

