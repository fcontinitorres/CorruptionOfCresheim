using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanForm: MonoBehaviour
{
	public CharacterController2D controller;
    
    public float jumpForce;
    public float airControl;
    public float runSpeed;

	float horizontalMove = 0f;
	bool jump = false;
    bool crouch = false;
    bool checkCeiling = false;
    bool powerTranform = false;

    void OnEnable () {
        controller.setJumpForce(jumpForce);
        controller.setAirControl(airControl);
    }

    //Getting the inputs
    void Update () {

		horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
        }
        else if (Input.GetButtonUp("Jump"))
        {
            jump = false;
        }

		if (Input.GetButtonDown("Crouch"))
        {
            crouch = true;
            checkCeiling = true;
		} else if (Input.GetButtonUp("Crouch"))
        {
            crouch = false;
		}

        if (Input.GetButtonDown("Transform"))
        {
            powerTranform = true;
        }

	}

    //Applying the input
	void FixedUpdate ()
	{
        if (powerTranform)
        {
            powerTranform = false;
            controller.druidicTransform();
        }
        else
        {
            //If the player was crouching, it will continue if there's a ceiling above him
            if (checkCeiling)
            {
                if (controller.isOnCeiling()) crouch = true;
                else if (crouch == true)
                {
                    crouch = false;
                    checkCeiling = false;
                }
            }

            //If the player isn't grounded, it can't double jump
            if (jump && !controller.isOnGround())
            {
                controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, false);
            }
            else
            {
                controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump);
            }

            // Move our character
            
        }
    }
}
