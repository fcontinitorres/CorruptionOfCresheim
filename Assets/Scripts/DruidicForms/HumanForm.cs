using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanForm: MonoBehaviour
{
	public PlayerController controller;
    
    public float jumpForce;
    public float airControl;
    public float runSpeed;

    void OnEnable () {
        controller.SetJumpForce(jumpForce);
        controller.SetAirControl(airControl);
    }

    //Applying the input
	void FixedUpdate ()
	{
        if (controller.inputManager.crouch || (!controller.inputManager.crouch && controller.inputManager.keepCrouch))
        {
            if (controller.IsOnCeiling())
            {
                controller.inputManager.jump = false;
                controller.inputManager.crouch = true;
            }
            else
            {
                Debug.Log("NOT CEILING");
                controller.inputManager.keepCrouch = false;
            }
        }
        Debug.Log(controller.inputManager.crouch);

        /*If the player was crouching, it will continue if there's a ceiling above him
        if (controller.inputManager.stopCrouch && !controller.inputManager.crouch)
        {
            if (controller.IsOnCeiling()) controller.inputManager.crouch = true;
            else
            {
                controller.inputManager.crouch = false;
                controller.inputManager.stopCrouch = false;
            }
        }*/

        //If the player isn't grounded, it can't double jump
        if (controller.inputManager.jump && !controller.IsOnGround())
        {
            //Moving it, without double jumping
            controller.Move(controller.inputManager.horizontalMove * runSpeed * Time.fixedDeltaTime,
                controller.inputManager.crouch, false);
        }
        else
        {
            //Moving it, jumping or not
            controller.Move(controller.inputManager.horizontalMove * runSpeed * Time.fixedDeltaTime,
                controller.inputManager.crouch, controller.inputManager.jump);
        }
    }
}
