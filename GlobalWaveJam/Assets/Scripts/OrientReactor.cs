using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrientReactor : MonoBehaviour
{
    [SerializeField]
    private float _raycastLength = 3f;

    public LayerMask _layersToConsider;

    void LateUpdate()
    {
        RaycastHit result;

        if (Physics.Raycast(transform.position, Vector3.down, out result, _raycastLength, _layersToConsider))
        {
            Quaternion rotateToNormal = Quaternion.FromToRotation(transform.up, result.normal);
            transform.rotation = rotateToNormal * transform.rotation;
        }
    }
}
