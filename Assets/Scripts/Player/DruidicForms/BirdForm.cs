using UnityEngine;
using System.Collections;

public class BirdForm: DruidicForm
{
    public override void FormEnable() {}
    public override void FormDisable() { }

    public override void Move()
    {
        controller.Move(inputManager.horizontalMove * Time.fixedDeltaTime, inputManager.jump);
    }
}
