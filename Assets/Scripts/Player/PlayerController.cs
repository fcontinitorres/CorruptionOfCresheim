using UnityEngine;

// Take care of movement, transformations, everything that envolves the entity player physically, and the sprites.
public class PlayerController : Entity
{
    private PlayerInputManager inputManager;
    private Animator animator;

    [Range(0, .3f)] [SerializeField] private float moveSmoothing = .05f;    // How much to smooth out the movement

	private bool isOnGround;            // Whether or not the player is grounded.
    private bool isOnCeiling;           // Whether or not the player has a ceiling above it

    private Rigidbody2D rigidbody_2D;
	private bool isFacingRight = true;  // For determining which way the player is currently facing.
	private Vector3 velocity = Vector3.zero;

    private float jumpSpeed;	    // Amount of force added when the player jumps.
    private float airControl;    // Percentage of control the player has on air.
    private float runSpeed;      // Max running speed
    private float dashForce;

    [SerializeField] private HumanForm humanForm;
    [SerializeField] private DruidicForm[] druidicForms;

    //Parameters that change when transforming between druidic forms
    public void SetJumpSpeed(float x) { jumpSpeed = x; }
    public void SetAirControl(float x) { airControl = x; }
    public void SetRunSpeed(float x) { runSpeed = x; }
    public void SetDashForce(float x) { dashForce = x; }

    //Ground and ceiling control
    public void SetIsOnCeiling(bool isOnCeiling) { this.isOnCeiling = isOnCeiling; }
    public bool IsOnCeiling() { return isOnCeiling; }
    public void SetIsOnGround(bool isOnGround) { this.isOnGround = isOnGround; }
    public bool IsOnGround() { return isOnGround; }

    private void Awake()
	{
        inputManager = GetComponent<PlayerInputManager>();
        animator = GetComponent<Animator>();
		rigidbody_2D = GetComponent<Rigidbody2D>();

        //Disabling all druidic forms, including the humanoid
        humanForm.enabled = false;
        for (int i = 0; i < druidicForms.Length; i++) druidicForms[i].enabled = false;

        //Enabling only the humanoid
        humanForm.enabled = true;

        animator.SetBool("IsDead", false);
	}

	public void Move(float move, bool jump)
	{
        move *= runSpeed;
        Vector3 targetVelocity = rigidbody_2D.velocity;

        //If on ground
        if (isOnGround)
		{
            animator.SetBool("IsGrounded", true);

            // Move the character by finding the target velocity
            targetVelocity.x = move * 10f;
            targetVelocity.y = 0;
        }
        //If not on ground
        else
        {
            animator.SetBool("IsGrounded", false);
            //Making ajustments to the momentum
            targetVelocity.x = targetVelocity.x + move * 10f * airControl;
            targetVelocity.x = Mathf.Min(move * 10f, targetVelocity.x);
            targetVelocity.x = Mathf.Max(move * 10f, targetVelocity.x);
        }

        // If the player should jump
        if (jump) targetVelocity.y = jumpSpeed;

        // And then smoothing it out and applying it to the character
        rigidbody_2D.velocity = Vector3.SmoothDamp(rigidbody_2D.velocity, targetVelocity, ref velocity, moveSmoothing);

        //Fliping player sprite to match move
        if (move > 0 && !isFacingRight) Flip();
        else if (move < 0 && isFacingRight) Flip();

        if(rigidbody_2D.velocity.y >= 0)
        {
            animator.SetFloat("MoveY+", rigidbody_2D.velocity.y);
            animator.SetFloat("MoveY-", 0);
        }
        else
        {
            animator.SetFloat("MoveY+", 0);
            animator.SetFloat("MoveY-", Mathf.Abs(rigidbody_2D.velocity.y));
        }

        animator.SetFloat("MoveX", Mathf.Abs(rigidbody_2D.velocity.x));
	}

    public void Dash(bool dashRight)
    {
        if (dashRight)
        {
            rigidbody_2D.AddForce(new Vector2(dashForce, 0));
        }
        else rigidbody_2D.AddForce(new Vector2(-dashForce, 0));
    }

	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		isFacingRight = !isFacingRight;
        transform.Rotate(0f, 180f, 0f);
	}

    // Transforming the player from humanoid to bird and vice-versa
    public void DruidicTransform()
    {
        //If is humanoid, will transform to a bird
        if (humanForm.enabled)
        {
            if (!druidicForms[0].CanBeTransformed()) return;

            humanForm.enabled = false;
            druidicForms[0].enabled = true;

            animator.SetInteger("DruidicForm", 1);
        }
        //Otherwise, is a bird and will transform back to humanoid
        else
        {
            if (!humanForm.CanBeTransformed()) return;

            druidicForms[0].enabled = false;
            humanForm.enabled = true;

            animator.SetInteger("DruidicForm", 0);
        }
    }

    public new void Die()
    {
        if (!humanForm.enabled) DruidicTransform();

        animator.SetBool("IsDead", true);
        inputManager.enabled = false;
    }
}
