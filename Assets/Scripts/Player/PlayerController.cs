using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public PlayerInputManager inputManager;

	[Range(0, 1)] [SerializeField] private float crouchSpeed = .36f;			// Amount of maxSpeed applied to crouching movement. 1 = 100%
	[Range(0, .3f)] [SerializeField] private float moveSmoothing = .05f;	// How much to smooth out the movement
	
	[SerializeField] private LayerMask whatIsGround;							// A mask determining what is ground to the character
	[SerializeField] private Transform groundPointCheck;						// A position marking where to check if the player is grounded.
	[SerializeField] private Transform ceilingPointCheck;						// A position marking where to check for ceilings
	[SerializeField] public Collider2D colliderToDisableWhenCrouch;			// A collider that will be disabled when crouching

	private bool isOnGround;            // Whether or not the player is grounded.
    private bool isOnCeiling;           // Whether or not the player has a ceiling above it

    private Rigidbody2D rigidbody_2D;
	private bool isFacingRight = true;  // For determining which way the player is currently facing.
	private Vector3 velocity = Vector3.zero;

    public float jumpForce;							// Amount of force added when the player jumps.
    public float airControl;                         // Percentage of control the player has on air.

    [SerializeField] private HumanForm humanForm;
    [SerializeField] private MonoBehaviour[] druidicForms = new MonoBehaviour[1];
    public Animator animator;
    public GameObject transformEffect;

    //Parameters that change when transforming between druidic forms
    public void SetJumpForce(float x) { jumpForce = x; }
    public void SetAirControl(float x) { airControl = x; }

    //Ground and ceiling control
    public void SetIsOnCeiling(bool isOnCeiling) { this.isOnCeiling = isOnCeiling; }
    public bool IsOnCeiling() { return isOnCeiling; }
    public void SetIsOnGround(bool isOnGround) { this.isOnGround = isOnGround; }
    public bool IsOnGround() { return isOnGround; }

    private void Awake()
	{
		rigidbody_2D = GetComponent<Rigidbody2D>();

        //Disabling all druidic forms, including the humanoid
        humanForm.enabled = false;
        for (int i = 0; i < druidicForms.Length; i++)
        {
            druidicForms[i].enabled = false;
        }

        //Enabling only the humanoid
        humanForm.enabled = true;
	}


	private void FixedUpdate()
	{
        //Transforming the player
        if (inputManager.powerTransform)
        {
            inputManager.powerTransform = false;
            DruidicTransform();
        }
    }

	public void Move(float move, bool crouch, bool jump)
	{
        Vector3 targetVelocity = rigidbody_2D.velocity;

        if (isOnGround)
		{
            animator.SetBool("IsGrounded", true);
			// If crouching
			if (crouch)
            {
                // Reduce the speed by the crouchSpeed multiplier
                move *= crouchSpeed;

                // Disable one of the colliders when crouching
                if (colliderToDisableWhenCrouch != null)
                    colliderToDisableWhenCrouch.enabled = false;

                animator.SetBool("IsCrouching", true);
            } else
            {
                // Enable the collider when not crouching
                if (colliderToDisableWhenCrouch != null)
                    colliderToDisableWhenCrouch.enabled = true;

                animator.SetBool("IsCrouching", false);
            }

            // Move the character by finding the target velocity
            targetVelocity.x = move * 10f;
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

        // And then smoothing it out and applying it to the character
        rigidbody_2D.velocity = Vector3.SmoothDamp(rigidbody_2D.velocity, targetVelocity, ref velocity, moveSmoothing);

        //Fliping player sprite to match move
        if (move > 0 && !isFacingRight) Flip();
        else if (move < 0 && isFacingRight) Flip();

        // If the player should jump
        if (jump)
		{
			// Add a vertical force to the player.
			rigidbody_2D.AddForce(new Vector2(0f, jumpForce));
		}

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
            humanForm.enabled = false;
            druidicForms[0].enabled = true;

            animator.SetInteger("DruidicForm", 1);
        }
        //Otherwise, is a bird and will transform back to humanoid
        else
        {
            druidicForms[0].enabled = false;
            humanForm.enabled = true;

            animator.SetInteger("DruidicForm", 0);
        }

        // Creating the transform effect, and destroying it after it's finished
        GameObject anim = Instantiate(transformEffect, transform.position, Quaternion.identity);
        Destroy(anim, anim.GetComponent<Animator>().runtimeAnimatorController.animationClips[0].length);
    }
}
