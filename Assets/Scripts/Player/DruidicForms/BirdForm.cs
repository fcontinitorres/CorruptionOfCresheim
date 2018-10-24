using UnityEngine;
using System.Collections;

public class BirdForm: DruidicForm
{
    private PlayerController controller;
    private PlayerInputManager inputManager;
    private PlayerResourceManager resourceManager;

    public int health_max;

    public float jumpSpeed;
    public float airControl;
    public float runSpeed;
    public float dashForce;
    public int manaCost;

    public override bool canBeTransformed()
    {
        return resourceManager.HasMana(manaCost);
    }

    private void Awake()
    {
        controller = GetComponent<PlayerController>();
        inputManager = GetComponent<PlayerInputManager>();
        resourceManager = GetComponent<PlayerResourceManager>();
    }

    //Tranforming to this form
    void OnEnable ()
    {
        controller.SetJumpSpeed(jumpSpeed);
        controller.SetAirControl(airControl);
        controller.SetRunSpeed(runSpeed);
        controller.SetDashForce(dashForce);
        resourceManager.SetHealthMax(health_max);
        resourceManager.UseMana(manaCost);
    }

    //Applying the input
    void FixedUpdate()
    {
        controller.Move(inputManager.horizontalMove * Time.fixedDeltaTime,
            false, inputManager.jump);
    }
}
