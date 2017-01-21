using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
public class ThirdPersonCharacter : MonoBehaviour
{
    public Animator m_Animator;

    Rigidbody m_Rigidbody;
    bool m_IsGrounded;
    float m_OrigGroundCheckDistance;
    float m_TurnAmount;
    float m_ForwardAmount;
    Vector3 m_GroundNormal;
    SphereCollider m_SphereCollider;

    [SerializeField]
    float m_StompPower = 12f;
    [SerializeField]
    private LayerMask _layersToConsiderForGround;
    [SerializeField]
    float m_BelowGroundCheckDistance = 10f;

    [HeaderAttribute("Grounded Control")]
    [SerializeField]
    float m_MovingTurnSpeedGrounded = 360;
    [SerializeField]
    float m_StationaryTurnSpeedGrounded = 180;
    [SerializeField]
    float m_MoveSpeedMultiplierGrounded = 1f;
    [SerializeField]
    float m_GroundCheckDistance = 0.1f;

    [HeaderAttribute("Airborne Control")]
    [SerializeField]
    float m_MovingTurnSpeedAirborne = 360;
    [SerializeField]
    float m_StationaryTurnSpeedAirborne = 180;
    [SerializeField]
    float m_MoveSpeedMultiplierAirborne = 1f;
    [Range(1f, 4f)]
    [SerializeField]
    float m_GravityMultiplier = 2f;

    [HeaderAttribute("Jump")]
    [SerializeField]
    float m_JumpPower = 12f;

    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_SphereCollider = GetComponent<SphereCollider>();

        m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        m_OrigGroundCheckDistance = m_GroundCheckDistance;
    }

    public void Move(Vector3 move, bool crouch, bool jump)
    {
        // convert the world relative moveInput vector into a local-relative
        // turn amount and forward amount required to head in the desired
        // direction.
        if (move.magnitude > 1f)
        {
            move.Normalize();
        }
        move = transform.InverseTransformDirection(move);
        CheckGroundStatus();
        move = Vector3.ProjectOnPlane(move, m_GroundNormal);
        m_TurnAmount = Mathf.Atan2(move.x, move.z);
        m_ForwardAmount = move.z;

        float moveSpeedMultiplier = 1f;

        // control and velocity handling is different when grounded and airborne:
        if (m_IsGrounded)
        {
            // help the character turn faster (this is in addition to root rotation in the animation)
            float turnSpeed = Mathf.Lerp(m_StationaryTurnSpeedGrounded, m_MovingTurnSpeedGrounded, m_ForwardAmount);
            transform.Rotate(0, m_TurnAmount * turnSpeed * Time.deltaTime, 0);

            // check whether conditions are right to allow a jump:
            if (jump && !crouch && m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Grounded"))
            {
                // jump!
                m_Rigidbody.AddForce(m_GroundNormal * m_JumpPower, ForceMode.Impulse);
                m_IsGrounded = false;
                m_GroundCheckDistance = 0.1f;
            }

            moveSpeedMultiplier = m_MoveSpeedMultiplierGrounded;
        }
        else
        {
            // help the character turn faster (this is in addition to root rotation in the animation)
            float turnSpeed = Mathf.Lerp(m_StationaryTurnSpeedAirborne, m_MovingTurnSpeedAirborne, m_ForwardAmount);
            transform.Rotate(0, m_TurnAmount * turnSpeed * Time.deltaTime, 0);

            // apply extra gravity from multiplier:
            Vector3 extraGravityForce = (Physics.gravity * m_GravityMultiplier) - Physics.gravity;
            m_Rigidbody.AddForce(extraGravityForce);

            m_GroundCheckDistance = m_Rigidbody.velocity.y < 0 ? m_OrigGroundCheckDistance : 0.01f;

            if (jump && !crouch && m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Airborne"))
            {
                // stomp!
                m_Rigidbody.AddRelativeForce(-Vector3.up * m_StompPower, ForceMode.Impulse);
            }

            moveSpeedMultiplier = m_MoveSpeedMultiplierAirborne;
        }

        // send input and other state parameters to the animator
        UpdateAnimator(move);

        // we implement this function to override the default root motion.
        // this allows us to modify the positional speed before it's applied.
        if (Time.deltaTime > 0)
        {
            m_Rigidbody.AddRelativeTorque(Vector3.up * m_TurnAmount, ForceMode.VelocityChange);
            m_Rigidbody.AddRelativeForce(Vector3.forward * m_ForwardAmount * moveSpeedMultiplier, ForceMode.VelocityChange);
        }
    }

    void UpdateAnimator(Vector3 move)
    {
        // update the animator parameters
        m_Animator.SetFloat("Forward", m_ForwardAmount, 0.1f, Time.deltaTime);
        m_Animator.SetFloat("Turn", m_TurnAmount, 0.1f, Time.deltaTime);
        m_Animator.SetBool("OnGround", m_IsGrounded);
        if (!m_IsGrounded)
        {
            m_Animator.SetFloat("Jump", m_Rigidbody.velocity.y);
        }
    }

    void CheckGroundStatus()
    {
        RaycastHit hitInfo;
#if UNITY_EDITOR
        // helper to visualise the ground check ray in the scene view
        Debug.DrawLine(transform.position + (Vector3.up * 0.1f), transform.position + (Vector3.up * 0.1f) + (Vector3.down * m_GroundCheckDistance));
#endif
        // 0.1f is a small offset to start the ray from inside the character 
        // it is also good to note that the transform position in the sample assets is at the base of the character
        if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo, m_GroundCheckDistance, _layersToConsiderForGround))
        {
            m_GroundNormal = hitInfo.normal;
            m_IsGrounded = true;
        }
        else
        {
            m_IsGrounded = false;
            m_GroundNormal = Vector3.up;

#if UNITY_EDITOR
            // helper to visualise the ground check ray in the scene view
            Debug.DrawLine(transform.position + m_BelowGroundCheckDistance * Vector3.up, transform.position, Color.cyan);
#endif
            // Not grounded. Check if the ground is above us
            if (Physics.Raycast(transform.position + m_BelowGroundCheckDistance * Vector3.up, Vector3.down, out hitInfo, m_BelowGroundCheckDistance, _layersToConsiderForGround))
            {
                transform.position = hitInfo.point;
            }
        }
    }
}
