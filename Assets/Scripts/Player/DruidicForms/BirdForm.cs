using UnityEngine;
using System.Collections;

public class BirdForm: GenericDruidicForm
{
    public override void Move()
    {
        controller.Move(inputManager.horizontalMove * Time.fixedDeltaTime, inputManager.jump);

        //Dash
        if (inputManager.dash != 0)
        {
            controller.Dash(inputManager.dash == 1);
            inputManager.dash = 0;
        }
    }
}
