using UnityEngine;
using System.Collections;

public class BirdForm: MonoBehaviour
{
    public PlayerController controller;
    
    public float jumpForce;
    public float airControl;
    public float runSpeed;

    float horizontalMove = 0f;
    bool jump = false;
    bool powerTranform = false;

    //Tranforming to this form
    void OnEnable ()
    {
        controller.setJumpForce(jumpForce);
        controller.setAirControl(airControl);
    }

    //Getting the inputs
    void Update()
    {

        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
        }
        else if (Input.GetButtonUp("Jump"))
        {
            jump = false;
        }

        if (Input.GetButtonDown("Transform"))
        {
            powerTranform = true;
        }

    }

    //Applying the input
    void FixedUpdate()
    {
        if (powerTranform)
        {
            powerTranform = false;
            controller.DruidicTransform();
        }
        else
        {
            // Move our character
            controller.Move(horizontalMove * Time.fixedDeltaTime, false, jump);
        }
    }
}
