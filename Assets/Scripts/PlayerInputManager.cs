using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour {

    public float horizontalMove = 0f;
    public bool jump = false;
    public bool crouch = false;
    public bool powerTransform = false;

    private void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump")) jump = true;
        else if (Input.GetButtonUp("Jump")) jump = false;

        if (Input.GetButtonDown("Crouch")) crouch = true;
        else if (Input.GetButtonUp("Crouch")) crouch = false;

        if (Input.GetButtonDown("Transform")) powerTransform = true;
        else if (Input.GetButtonUp("Transform")) powerTransform = false;
    }
}