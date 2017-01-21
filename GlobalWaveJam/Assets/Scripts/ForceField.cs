using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class ForceField : MonoBehaviour
{
    [SerializeField]
    private float forceStrength = 5f;

    [SerializeField]
    private float duration = 1f;


    [SerializeField]
    private float maxSphereRadius = 5f;

    [SerializeField]
    private AnimationCurve sphereScaleAnimationCurve;

    private SphereCollider _sphereColliderComponent;
    private float time = 0f;

    private List<Transform> transformsToIgnore = new List<Transform>(1);

    void Awake()
    {
        _sphereColliderComponent = GetComponent<SphereCollider>();
    }

    public void AddGameObjectToIgnore(Transform transformToIgnore)
    {
        transformsToIgnore.Add(transformToIgnore);
    }

    void FixedUpdate()
    {
        time += Time.fixedDeltaTime / duration;
        _sphereColliderComponent.radius = maxSphereRadius * sphereScaleAnimationCurve.Evaluate(time);
        if (time > 1.0f)
        {
            GameObject.Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (!transformsToIgnore.Contains(collider.transform))
        {
            transformsToIgnore.Add(collider.transform);
            Vector3 toCollider = collider.transform.position - transform.position;
            toCollider.Normalize();
            collider.GetComponent<Rigidbody>().AddForce(forceStrength * toCollider, ForceMode.Impulse);
        }
    }
}
