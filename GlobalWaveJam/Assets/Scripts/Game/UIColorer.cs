using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script to add to any UI parent component you'd like to follow the characters color codes
/// </summary>
public class UIColorer : MonoBehaviour {

    // settings
    public Material Emission;

	// Use this for initialization
	void Start () {
        if (Emission == null)
            return;

        Color c = Emission.GetColor("_EmissionColor");

        foreach (Text t in GetComponentsInChildren<Text>())
        {
            t.color = c;
        }

        foreach (RawImage i in GetComponentsInChildren<RawImage>())
        {
            i.color = c;
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
