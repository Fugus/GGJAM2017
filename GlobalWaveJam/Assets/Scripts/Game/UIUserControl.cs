using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;


public class UIUserControl : MonoBehaviour {

    public ControllerSettings Controls = new ControllerSettings();

    // reference
    private Animation Anim;

    // Use this for initialization
    void Start () {
        // check if this player has a controller
        GamePadState state = GamePad.GetState(Controls.Index);
        if (state.IsConnected == false)
        {
            Destroy(this.gameObject);
            return;
        }

        Anim = GetComponent<Animation>();
    }

    // Update is called once per frame
    void Update () {
        GamePadState state = GamePad.GetState(Controls.Index);
        if (state.IsConnected == false)
            return;

        // show my shit
        float force = (Controls.Side == ControllerSide.Left ? state.Triggers.Left : state.Triggers.Right);
        if (force > float.Epsilon && !Anim.isPlaying)
        {
            Anim.Play();
        }

        // start !
        if (state.Buttons.Start == ButtonState.Pressed)
        {
            GameLogic.State = GameState.Intro;
        }
    }
}
