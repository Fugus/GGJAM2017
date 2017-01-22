using System.Collections;
using UnityEngine;

public class ForceFieldSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject forceFieldToSpawnPrefab;

    [SerializeField]
    private float Cooldown = 1f;

    private bool m_CanDoForceField = true;

    public void SpawnForceField(Vector3 targetPosition)
    {
        if (m_CanDoForceField)
        {
            GameObject instantiatedForceFieldGO = GameObject.Instantiate(forceFieldToSpawnPrefab);
            ForceField forceFieldComponent = instantiatedForceFieldGO.GetComponent<ForceField>();
            instantiatedForceFieldGO.transform.position = targetPosition;
            forceFieldComponent.AddGameObjectToIgnore(transform);

            StartCoroutine(CooldownCoroutine());
        }
    }

    IEnumerator CooldownCoroutine()
    {
        m_CanDoForceField = false;

        yield return new WaitForSeconds(Cooldown);

        m_CanDoForceField = true;
    }
}
