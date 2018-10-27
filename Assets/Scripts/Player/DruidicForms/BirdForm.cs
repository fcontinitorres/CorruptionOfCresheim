using UnityEngine;
using System.Collections;

public class BirdForm: GenericDruidicForm
{
    protected override void OnDisable()
    {
        // Moving player up, so it doesn't get stuck on the ground
        GetComponentInParent<PlayerController>().transform.Translate(0f, 0.5f, 0f);
        base.OnDisable();
    }

    public override void Move()
    {
        // Simple movement, just move and infinity jumps aka flight
        controller.Move(inputManager.horizontalMove * Time.fixedDeltaTime, inputManager.jump);

        //Dash
        if (inputManager.dash != 0)
        {
            controller.Dash(inputManager.dash == 1);
            inputManager.dash = 0;
        }
    }
}
