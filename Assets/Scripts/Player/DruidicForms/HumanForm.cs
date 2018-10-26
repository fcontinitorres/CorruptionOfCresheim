using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanForm: GenericDruidicForm
{
    [SerializeField] private Collider2D standCollider;	// A collider that will be disabled when crouching
    [Range(0, 1)] [SerializeField] private float crouchSpeed = .4f; // Amount of maxSpeed applied to crouching movement. 1 = 100%

    [SerializeField] private MeleeAttack attacksGround;
    [SerializeField] private MeleeAttack attacksAir;
    [SerializeField] private RangedAttack specialAttack;
    [SerializeField] private int specialAttackManaCost;

    private bool keepCrouch = false;
    private bool lastJumpInput = false;

    protected override void OnDisable()
    {
        controller.SetIsOnCeiling(false);
        controller.SetIsOnGround(false);
        base.OnDisable();
    }

    public override void Move()
	{
        //Stop when ground attacking on ground and when doing the special attack
        if (attacksGround.cooldownCurr > attacksGround.cooldownTolerance ||
            specialAttack.cooldownCurr > specialAttack.cooldownTolerance)
        {
            controller.Move(0, false);
            return;
        }

        if (!inputManager.crouch && !keepCrouch)
        {
            if (inputManager.attack)
            {
                if (controller.IsOnGround()) attacksGround.Attack();
                else attacksAir.Attack();
            }
            else if (controller.IsOnGround() && inputManager.specialAttack)
            {
                if (controller.HasMana(specialAttackManaCost)) specialAttack.Attack();
                inputManager.specialAttack = false;
            }
        }



        //If the player was crouching, it will continue if there's a ceiling above him
        if (inputManager.crouch || (!inputManager.crouch && controller.IsOnCeiling() && keepCrouch))
        {
            keepCrouch = true;
            //Applying crouch speed modifier
            inputManager.horizontalMove *= crouchSpeed;
            //Disabling the stand collider if on ground
            if (standCollider) standCollider.enabled = false;
            //Setting animator to crouch
            animator.SetBool("IsCrouching", true);
        }
        else
        {
            keepCrouch = false;
            //Enabling the stand collider
            if (standCollider) standCollider.enabled = true;
            //Setting animator to not crouch
            animator.SetBool("IsCrouching", false);
        }

        //Can't jump when there's a ceiling directly above
        if (controller.IsOnCeiling()) inputManager.jump = false;

        //Filtering the jump input
        if (lastJumpInput && inputManager.jump) inputManager.jump = false;
        else lastJumpInput = inputManager.jump;

        //If the player isn't grounded, it can't double jump
        if (inputManager.jump && !controller.IsOnGround())
        {
            //Moving it, without double jumping
            inputManager.jump = false;
        }

        controller.Move(inputManager.horizontalMove * Time.fixedDeltaTime, inputManager.jump);

        //Dash
        if (!keepCrouch && inputManager.dash != 0)
        {
            controller.Dash(inputManager.dash == 1);
            inputManager.dash = 0;
        }
    }
}
