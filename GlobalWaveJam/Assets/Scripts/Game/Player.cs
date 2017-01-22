﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Events generated by a player during a game (to give him point at the end of a game
/// </summary>
public class PlayerStats
{
    // variables
    public int Score;
    public bool Alive = true;
    public Dictionary<TrackingEvent, int> Events;
}


#region delegates
public delegate void DelegateDeath();
#endregion


/// <summary>
/// Manage player game logic stuff (non physical)
/// </summary>
public class Player : MonoBehaviour
{
    #region settings
    public Material Emission;
    public Material Skinned;
    #endregion

    #region variables
    public PlayerStats Stats;
    #endregion

    #region events
    public event DelegateDeath DeathEvent;
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
            //Debug.Log(r.gameObject.name + ":" + r.materials.Length);
        }

        // change characters skinned material (skin anim models such as robot)
        foreach (SkinnedMeshRenderer r in GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            if (Skinned != null)
                r.material = Skinned;
        }

        // register to game logic (for faster reference)
        GameLogic.Players.Add(this);
        DeathEvent += GameLogic.Instance.OnDeath;
    }

    // Update is called once per frame
    void Update()
    {

    }

    #region tracking
    public void Die()
    {
        Debug.Log("aaaaargh Rosebud !");
        if (DeathEvent != null)
            DeathEvent();
    }
    #endregion
}
