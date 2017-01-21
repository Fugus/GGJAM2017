using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeformSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject deformToSpawnPrefab;

    private Transform _instantiatedDeform;

    public void SpawnDeform(Vector3 targetPosition)
    {
        if (_instantiatedDeform == null)
        {
            GameObject instantiatedDeformGO = GameObject.Instantiate(deformToSpawnPrefab);
            _instantiatedDeform = instantiatedDeformGO.transform;
        }
        _instantiatedDeform.position = targetPosition;
    }
}
