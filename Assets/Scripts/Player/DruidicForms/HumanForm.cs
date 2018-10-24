using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanForm: DruidicForm
{
	private PlayerController controller;
    private PlayerInputManager inputManager;
    private PlayerResourceManager resourceManager;

    public int health_max;

    public float jumpSpeed;
    public float airControl;
    public float runSpeed;
    public float dashForce;

    private bool keepCrouch = false;
    private bool lastJumpInput = false;

    public override bool canBeTransformed() { return true; }

    private void Awake()
    {
        controller = GetComponent<PlayerController>();
        inputManager = GetComponent<PlayerInputManager>();
        resourceManager = GetComponent<PlayerResourceManager>();
    }

    void OnEnable () {
        controller.SetJumpSpeed(jumpSpeed);
        controller.SetAirControl(airControl);
        controller.SetRunSpeed(runSpeed);
        controller.SetDashForce(dashForce);
        resourceManager.setHealthMax(health_max);
    }

    //Applying the input
	void FixedUpdate ()
	{
        //If the player was crouching, it will continue if there's a ceiling above him
        if (inputManager.crouch || (!inputManager.crouch && controller.IsOnCeiling()))
        {
            keepCrouch = true;
        }
        else keepCrouch = false;

        //Can't jump when there's a ceiling directly above
        if (controller.IsOnCeiling()) inputManager.jump = false;

        //Filtering the jump input
        if (lastJumpInput && inputManager.jump) inputManager.jump = false;
        else lastJumpInput = inputManager.jump;

        //If the player isn't grounded, it can't double jump
        if (inputManager.jump && !controller.IsOnGround())
        {
            //Moving it, without double jumping
            controller.Move(inputManager.horizontalMove * Time.fixedDeltaTime,
                keepCrouch, false);
        }
        else
        {
            //Moving it, jumping or not
            controller.Move(inputManager.horizontalMove * Time.fixedDeltaTime,
                keepCrouch, inputManager.jump);
        }

        //Dash
        if (inputManager.dash != 0)
        {
            controller.Dash(inputManager.dash == 1);
            inputManager.dash = 0;
        }
    }
}
