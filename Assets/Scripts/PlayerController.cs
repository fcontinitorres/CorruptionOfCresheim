using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[Range(0, 1)] [SerializeField] private float m_CrouchSpeed = .36f;			// Amount of maxSpeed applied to crouching movement. 1 = 100%
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;	// How much to smooth out the movement
	
	[SerializeField] private LayerMask m_WhatIsGround;							// A mask determining what is ground to the character
	[SerializeField] private Transform m_GroundCheck;							// A position marking where to check if the player is grounded.
	[SerializeField] private Transform m_CeilingCheck;							// A position marking where to check for ceilings
	[SerializeField] private Collider2D m_CrouchDisableCollider;				// A collider that will be disabled when crouching

    const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
	private bool m_Grounded;            // Whether or not the player is grounded.
    private bool m_OnCeiling;           // Whether or not the player has a ceiling above it
	const float k_CeilingRadius = .5f; // Radius of the overlap circle to determine if the player can stand up
	private Rigidbody2D m_Rigidbody2D;
	private bool m_FacingRight = true;  // For determining which way the player is currently facing.
	private Vector3 velocity = Vector3.zero;

    public float m_JumpForce;							// Amount of force added when the player jumps.
    public float m_AirControl;                         // Percentage of control the player has on air.

    [SerializeField] private HumanForm humanForm;
    [SerializeField] private MonoBehaviour[] druidicForms = new MonoBehaviour[1];
    public Animator animator;
    public GameObject transformEffect;

    public void setJumpForce(float x) { m_JumpForce = x; }
    public void setAirControl(float x) { m_AirControl = x; }

    public bool isOnCeiling() { return m_OnCeiling; }
    public bool isOnGround() { return m_Grounded; }

    private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();

        //Disabling all druidic forms, including the humanoid
        for (int i = 0; i < druidicForms.Length; i++)
        {
            druidicForms[i].enabled = false;
        }
        humanForm.enabled = false;

        //Enabling only the humanoid
        humanForm.enabled = true;
	}


	private void FixedUpdate()
	{
		m_Grounded = false;

		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
				m_Grounded = true;
		}

        m_OnCeiling = Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround);
    }

	public void Move(float move, bool crouch, bool jump)
	{
        Vector3 targetVelocity = m_Rigidbody2D.velocity;

        //If on ground
        if (m_Grounded)
		{
			// If crouching
			if (crouch)
            {
                // Reduce the speed by the crouchSpeed multiplier
                move *= m_CrouchSpeed;

                // Disable one of the colliders when crouching
                if (m_CrouchDisableCollider != null)
                    m_CrouchDisableCollider.enabled = false;

                animator.SetBool("isCrouching", true);
            } else
            {
                // Enable the collider when not crouching
                if (m_CrouchDisableCollider != null)
                    m_CrouchDisableCollider.enabled = true;

                animator.SetBool("isCrouching", false);
            }

            // Move the character by finding the target velocity
            targetVelocity.x = move * 10f;
        }
        //If not on ground
        else
        {
            //Making ajustments to the momentum
            targetVelocity.x = targetVelocity.x + move * 10f * m_AirControl;
            targetVelocity.x = Mathf.Min(move * 10f, targetVelocity.x);
            targetVelocity.x = Mathf.Max(move * 10f, targetVelocity.x);
        }

        // And then smoothing it out and applying it to the character
        m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref velocity, m_MovementSmoothing);

        // If the input is moving the player right and the player is facing left...
        if (move > 0 && !m_FacingRight) Flip();
        // Otherwise if the input is moving the player left and the player is facing right...
        else if (move < 0 && m_FacingRight) Flip();

        // If the player should jump...
        if (jump)
		{
			// Add a vertical force to the player.
			m_Grounded = false;
			m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
		}

        if(m_Rigidbody2D.velocity.y >= 0)
        {
            animator.SetFloat("MoveY+", m_Rigidbody2D.velocity.y);
            animator.SetFloat("MoveY-", 0);
        }
        else
        {
            animator.SetFloat("MoveY+", 0);
            animator.SetFloat("MoveY-", Mathf.Abs(m_Rigidbody2D.velocity.y));
        }

        animator.SetFloat("MoveX", Mathf.Abs(m_Rigidbody2D.velocity.x));
	}

	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;

        transform.Rotate(0f, 180f, 0f);
	}

    public void DruidicTransform()
    {
        if (humanForm.enabled)
        {
            humanForm.enabled = false;
            druidicForms[0].enabled = true;

            animator.SetInteger("druidicForm", 1);
        }
        else
        {
            druidicForms[0].enabled = false;
            humanForm.enabled = true;

            animator.SetInteger("druidicForm", 0);
        }

        GameObject anim = Instantiate(transformEffect, transform.position, Quaternion.identity);
        Destroy(anim, anim.GetComponent<Animator>().runtimeAnimatorController.animationClips[0].length);
    }
}
