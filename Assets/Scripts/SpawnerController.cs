using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class SpawnerController : NetworkBehaviour
{
    [SerializeField] GameObject objectPrefab;
    [SerializeField] int maxObjectInstanceCount = 3;

    private void Awake()
    {
        // Initial pool

    }

    public void LaunchObjects()
    {
        if (!IsServer) return;

        for (int i = 0; i < maxObjectInstanceCount; i++)
        {
            GameObject go = Instantiate(objectPrefab, new Vector3(Random.Range(-10, 10), 10.0f, Random.Range(-10, 10)), Quaternion.identity);
            go.GetComponent<NetworkObject>().Spawn();
            // Pool instantiation
        }
    }

}
