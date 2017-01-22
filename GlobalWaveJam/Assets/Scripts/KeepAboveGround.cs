using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepAboveGround : MonoBehaviour
{
    [SerializeField]
    private float m_BelowGroundCheckDistance = 10f;

    [SerializeField]
    private LayerMask _layersToConsiderForGround;

    public delegate void OnKeptAboveGroundDelegate(Vector3 newPosition, Vector3 groundNormal);
    public event OnKeptAboveGroundDelegate OnKeptAboveGround;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
#if UNITY_EDITOR
        // helper to visualise the ground check ray in the scene view
        Debug.DrawLine(transform.position + Vector3.up * 0.1f + m_BelowGroundCheckDistance * Vector3.up, transform.position + Vector3.up * 0.1f , Color.cyan);
#endif

        RaycastHit hitInfo;
        // Not grounded. Check if the ground is above us
        if (Physics.Raycast(transform.position + Vector3.up * 0.1f + m_BelowGroundCheckDistance * Vector3.up, Vector3.down, out hitInfo, m_BelowGroundCheckDistance, _layersToConsiderForGround))
        {
            transform.position = hitInfo.point;
            if (OnKeptAboveGround != null)
            {
                OnKeptAboveGround(hitInfo.point, hitInfo.normal);
            }
        }
    }
}
