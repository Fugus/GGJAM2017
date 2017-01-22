using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Collider))]
public class ForceApplyTrigger : MonoBehaviour
{
    [SerializeField]
    private float ForceStrength = 10;
    [SerializeField]
    private Transform OnEnterForceTransform;
    [SerializeField]
    private Transform OnStayForceTransform;
    [SerializeField]
    private Transform OnExitForceTransform;

#if UNITY_EDITOR
    public bool DoAddChildren = false;
    private void Update()
    {
        if (DoAddChildren)
        {
            DoAddChildren = false;
            if (transform.FindChild("OnEnterForceTransform") == null)
            {
                GameObject child = new GameObject("OnEnterForceTransform");
                child.transform.SetParent(transform, worldPositionStays: false);
                OnEnterForceTransform = child.transform;
            }
            if (transform.FindChild("OnStayForceTransform") == null)
            {
                GameObject child = new GameObject("OnStayForceTransform");
                child.transform.SetParent(transform, worldPositionStays: false);
                OnStayForceTransform = child.transform;
            }
            if (transform.FindChild("OnExitForceTransform") == null)
            {
                GameObject child = new GameObject("OnExitForceTransform");
                child.transform.SetParent(transform, worldPositionStays: false);
                OnExitForceTransform = child.transform;
            }
        }
    }

#endif // UNITY_EDITOR

    void OnTriggerEnter(Collider collider)
    {
        if (OnEnterForceTransform != null)
        {
            Rigidbody rigidBody = collider.GetComponent<Rigidbody>();
            if (rigidBody != null)
            {
                rigidBody.AddForce(ForceStrength * OnEnterForceTransform.forward, ForceMode.Impulse);
            }
        }
    }

    void OnTriggerStay(Collider collider)
    {
        if (OnStayForceTransform != null)
        {
            Rigidbody rigidBody = collider.GetComponent<Rigidbody>();
            if (rigidBody != null)
            {
                rigidBody.AddForce(ForceStrength * OnStayForceTransform.forward, ForceMode.Impulse);
            }
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (OnExitForceTransform != null)
        {
            Rigidbody rigidBody = collider.GetComponent<Rigidbody>();
            if (rigidBody != null)
            {
                rigidBody.AddForce(ForceStrength * OnExitForceTransform.forward, ForceMode.Impulse);
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        if (OnEnterForceTransform != null)
        {
            DrawArrow.ForGizmo(OnEnterForceTransform.position, OnEnterForceTransform.forward);
        }
        if (OnStayForceTransform != null)
        {
            DrawArrow.ForGizmo(OnStayForceTransform.position, OnStayForceTransform.forward);
        }
        if (OnExitForceTransform != null)
        {
            DrawArrow.ForGizmo(OnExitForceTransform.position, OnExitForceTransform.forward);
        }
    }
}
