using System;
using UnityEngine;
//using UnityStandardAssets.CrossPlatformInput;
using XInputDotNetPure;

namespace UnityStandardAssets.Characters.ThirdPerson
{
    public class ControllerSettings
    {
        public PlayerIndex Index = PlayerIndex.One;
        public ControllerSide Side = ControllerSide.Left;
    }

    [RequireComponent(typeof(ThirdPersonCharacter))]
    public class ThirdPersonUserControl : MonoBehaviour
    {
        private ThirdPersonCharacter m_Character; // A reference to the ThirdPersonCharacter on the object
        private Transform m_Cam;                  // A reference to the main camera in the scenes transform
        private Vector3 m_CamForward;             // The current forward direction of the camera
        private Vector3 m_Move;
        private ButtonStateEvent m_Jump;                      // the world-relative desired move direction, calculated from the camForward and user input.
        private ButtonStateEvent m_Force;                   // activate the force field

        #region controls settings
        private ControllerSettings _controls = new ControllerSettings();
        public ControllerSettings Controls
        {
            get { return _controls; }
            set
            {
                _controls = value;
            }
        }

        private static ButtonStateEvent ChangeButton(ButtonStateEvent button, bool press)
        {
            return press ? (button == ButtonStateEvent.Release || button == ButtonStateEvent.NONE ? button = ButtonStateEvent.Press : button = ButtonStateEvent.Hold)
                : (button == ButtonStateEvent.Press || button == ButtonStateEvent.Hold ? button = ButtonStateEvent.Release : button = ButtonStateEvent.NONE);
        }
        #endregion

        private void Start()
        {
            // get the transform of the main camera
            if (Camera.main != null)
            {
                m_Cam = Camera.main.transform;
            }
            else
            {
                Debug.LogWarning(
                    "Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.", gameObject);
                // we use self-relative controls in this case, which probably isn't what the user wants, but hey, we warned them!
            }

            // get the third person character ( this should never be null due to require component )
            m_Character = GetComponent<ThirdPersonCharacter>();
        }


        private void Update()
        {
        }


        // Fixed update is called in sync with physics
        private void FixedUpdate()
        {
            GamePadState state = GamePad.GetState(Controls.Index);
            if (state.IsConnected == false)
                return;

            // buttons
            m_Jump = ChangeButton(m_Jump, (Controls.Side == ControllerSide.Left ? state.Buttons.LeftShoulder : state.Buttons.RightShoulder) == ButtonState.Pressed);  //CrossPlatformInputManager.GetButtonDown("Jump");
            m_Force = ChangeButton(m_Force, (Controls.Side == ControllerSide.Left ? state.Triggers.Left : state.Triggers.Right) > float.Epsilon);  //CrossPlatformInputManager.GetButtonDown("Jump");

            // read inputs
            float h = (Controls.Side == ControllerSide.Left ? state.ThumbSticks.Left.X : state.ThumbSticks.Right.X); //CrossPlatformInputManager.GetAxis("Horizontal");
            float v = (Controls.Side == ControllerSide.Left ? state.ThumbSticks.Left.Y : state.ThumbSticks.Right.Y); //CrossPlatformInputManager.GetAxis("Vertical");
            bool crouch = Input.GetKey(KeyCode.C);

            // calculate move direction to pass to character
            if (m_Cam != null)
            {
                // calculate camera relative direction to move:
                m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
                m_Move = v * m_CamForward + h * m_Cam.right;
            }
            else
            {
                // we use world-relative directions in the case of no main camera
                m_Move = v * Vector3.forward + h * Vector3.right;
            }
#if !MOBILE_INPUT
            // walk speed multiplier
            if (Input.GetKey(KeyCode.LeftShift)) m_Move *= 0.5f;
#endif

            // pass all parameters to the character control script
            m_Character.Move(m_Move, m_Force, m_Jump);
        }

    }
}
