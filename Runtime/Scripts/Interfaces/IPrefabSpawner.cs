using UnityEngine;

public interface IPrefabSpawner
{
    T Spawn<T>(GameObject prefab) where T : MonoBehaviour;
}