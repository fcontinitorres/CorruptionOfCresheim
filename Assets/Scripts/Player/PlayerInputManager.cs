using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Take care of setting up the parameters to all druidic transformations and sinalizes to the PlayerController
// when to transform
public class PlayerInputManager : MonoBehaviour {

    private PlayerController controller;

    public float horizontalMove = 0f;
    public bool jump = false;
    public bool crouch = false;
    public int dash = 0;

    private void Awake()
    {
        controller = GetComponent<PlayerController>();
    }

    private void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump")) jump = true;
        else if (Input.GetButtonUp("Jump")) jump = false;

        if (Input.GetButtonDown("Crouch")) crouch = true;
        else if (Input.GetButtonUp("Crouch")) crouch = false;

        if (Input.GetButtonDown("DashLeft")) dash = -1;
        else if (Input.GetButtonDown("DashRight")) dash = 1;
        else if (Input.GetButtonUp("DashLeft") && Input.GetButtonUp("DashRight")) dash = 0;

        if (Input.GetButtonDown("Transform")) {
            controller.DruidicTransform();
        }
    }
}