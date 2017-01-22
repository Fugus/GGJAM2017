using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class Rumble
{
    public ControllerSettings Pad;
    public float Time;
    public float Force;
}

/// <summary>
/// Static rumble manager because controllers are shared between 2 players each
/// </summary>
public class RumbleManager : Singleton<RumbleManager>
{
    private List<Rumble> Rumbles = new List<Rumble>();

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        foreach (Rumble r in Instance.Rumbles)
        {
            if (r.Time < 0)
                continue;

            r.Time -= Time.deltaTime;
            if (r.Time < 0)
            {
                Rumble(r.Pad, r.Time, 0);
            }
        }
    }

    public static void Rumble(ControllerSettings controls, float time, float force)
    {
        List<Rumble> rumble = Instance.Rumbles.FindAll(x => x.Pad == controls);
        if (rumble.Count <= 0)
        {
            Instance.Rumbles.Add(new global::Rumble() { Pad = controls });
            rumble = Instance.Rumbles.FindAll(x => x.Pad == controls);
        }

        rumble[0].Time = time;
        rumble[0].Force = force;
        SetRumble(controls, force);
    }

    static void SetRumble(ControllerSettings controls, float force)
    {
        Rumble left = Instance.Rumbles.Find(x => x.Pad.Index == controls.Index && x.Pad.Side == ControllerSide.Left);
        if (left == null)
            left = new global::Rumble() { Pad = new ControllerSettings() { Side = ControllerSide.Left, Index = controls.Index }, Force = 0, Time = 0 };
        Rumble right = Instance.Rumbles.Find(x => x.Pad.Index == controls.Index && x.Pad.Side == ControllerSide.Right);
        if (right == null)
            right = new global::Rumble() { Pad = new ControllerSettings() { Side = ControllerSide.Right, Index = controls.Index }, Force = 0, Time = 0 };

        // set
        GamePad.SetVibration(controls.Index, left.Force, right.Force);
        Debug.Log("rumble " + controls.Index + ": " + left.Force + " " + right.Force);
    }

}
