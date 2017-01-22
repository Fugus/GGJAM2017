using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

#region delegates
public delegate void DelegateDeath(Player sender);
#endregion

[RequireComponent(typeof(AudioSource))]
/// <summary>
/// Manage player game logic stuff (non physical)
/// </summary>
public class Player : MonoBehaviour
{
    #region settings
    public Material Emission;
    public Material Skinned;
    public int Index;

	AudioSource m_audioSource;
	public AudioClip m_AudioDeath;
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
		m_audioSource = GetComponent<AudioSource> ();
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

    #region tracking
    public void Die()
    {
        // kill and score
        Alive = false;
        GetComponent<ThirdPersonUserControl>().enabled = false;

		m_audioSource.PlayOneShot (m_AudioDeath);

        if (DeathEvent != null)
            DeathEvent(this);
    }
    #endregion
}
