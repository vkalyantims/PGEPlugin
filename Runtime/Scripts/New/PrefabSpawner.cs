using UnityEngine;

public class PrefabSpawner : IPrefabSpawner
{
    public T Spawn<T>(GameObject prefab) where T : MonoBehaviour
    {
        var go = Object.Instantiate(prefab);
        return go.GetComponent<T>();
    }
}