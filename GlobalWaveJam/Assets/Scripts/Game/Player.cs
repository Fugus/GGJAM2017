using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;


#region delegates
public delegate void DelegateDeath(Player sender);
#endregion


/// <summary>
/// Manage player game logic stuff (non physical)
/// </summary>
public class Player : MonoBehaviour
{
    #region settings
    public Material Emission;
    public Material Skinned;
    public int Index;
    public List<GameObject> VFXPrefabs;
    #endregion

    #region variables
    [HideInInspector]
    public bool Alive = true;
    #endregion

    #region events
    public event DelegateDeath DeathEvent;
    #endregion

    // Use this for initialization
    void Start()
    {
        // GENERATE OBJECTS
        foreach (GameObject o in VFXPrefabs)
        {
            SSpawner.AddElements(o, 1);
        }

        // COLORS !
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
            //Debug.Log(r.gameObject.name + ":" + r.materials.Length);
        }

        // lights and stuff
        if (Emission != null)
        {
            Color c = Emission.GetColor("_EmissionColor");
            foreach (Light l in GetComponentsInChildren<Light>())
            {
                l.color = c;
            }

            foreach (ParticleSystem p in GetComponentsInChildren<ParticleSystem>())
            {
                ParticleSystem.MainModule module = p.main;
                module.startColor = c;
            }

        }

        // change characters skinned material (skin anim models such as robot)
        foreach (SkinnedMeshRenderer r in GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            if (Skinned != null)
                r.material = Skinned;
        }

        // register to game logic (for faster reference), only first time (then just update chara reference next time) 
        if (GameLogic.Players.FindAll(x => x.Index == Index).Count == 0)
            GameLogic.Players.Add(new PlayerStats() { Chara = this, Index = this.Index });
        else
            GameLogic.Players.Find(x => x.Index == Index).Chara = this;

        DeathEvent += GameLogic.Instance.OnDeath;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnDestroy()
    {
    }

    #region tracking
    public void Die()
    {
        // kill and score
        Alive = false;
        GetComponent<ThirdPersonUserControl>().enabled = false;

        // display death VFX (TODO: input the vfx string name)
        SSpawner.Spawn("death_fx", transform.position, Quaternion.identity);

        // display cam shake
        Camera.main.GetComponent<Animation>().Play();

        // controller
        GetComponent<UnityStandardAssets.Characters.ThirdPerson.ThirdPersonUserControl>().Rumble(GameSettings.Rumble[RumbleEvent.Death].force, GameSettings.Rumble[RumbleEvent.Death].time);


        if (DeathEvent != null)
            DeathEvent(this);
    }
    #endregion
}
