using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceFieldSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject forceFieldToSpawnPrefab;

    public void SpawnForceField(Vector3 targetPosition)
    {
        GameObject instantiatedForceFieldGO = GameObject.Instantiate(forceFieldToSpawnPrefab);
		ForceField forceFieldComponent = instantiatedForceFieldGO.GetComponent<ForceField>();
        instantiatedForceFieldGO.transform.position = targetPosition;
		forceFieldComponent.AddGameObjectToIgnore(transform);
    }
}
