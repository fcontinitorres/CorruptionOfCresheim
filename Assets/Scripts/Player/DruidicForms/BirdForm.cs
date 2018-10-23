using UnityEngine;
using System.Collections;

public class BirdForm: MonoBehaviour
{
    public PlayerController controller;
    
    public float jumpForce;
    public float airControl;
    public float runSpeed;

    //Tranforming to this form
    void OnEnable ()
    {
        controller.SetJumpForce(jumpForce);
        controller.SetAirControl(airControl);
    }

    //Applying the input
    void FixedUpdate()
    {
        // Move our character
        controller.Move(controller.inputManager.horizontalMove * runSpeed * Time.fixedDeltaTime,
            false, controller.inputManager.jump);
    }
}
