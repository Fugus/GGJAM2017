using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region settings
    public Material Emission;
    public Material Skinned;
    #endregion


    // Use this for initialization
    void Start()
    {
        foreach (TrailRenderer r in GetComponentsInChildren<TrailRenderer>())
        {
            // change emissive in trails
            if (Emission != null)
                r.material = Emission;
        }

        foreach (MeshRenderer r in GetComponentsInChildren<MeshRenderer>())
        {
            var temp = r.materials;
            // change emissive in models
            if (Emission != null)
                temp[0] = Emission;
            // change skinned in models (platform)
            if (temp.Length > 1 && Skinned != null)
                temp[1] = Skinned;
            r.materials = temp;
            Debug.Log(r.gameObject.name + ":" + r.materials.Length);
        }

        // change characters skinned material (skin anim models such as robot)
        foreach (SkinnedMeshRenderer r in GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            if (Skinned != null)
                r.material = Skinned;
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
