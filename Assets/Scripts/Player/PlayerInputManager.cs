using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

// Take care of setting up the parameters to all druidic transformations and sinalizes to the PlayerController
// when to transform
public class PlayerInputManager : MonoBehaviour {

    private PlayerController controller;

    [System.NonSerialized] public float horizontalMove;
    [System.NonSerialized] public bool jump;
    [System.NonSerialized] public bool crouch;
    [System.NonSerialized] public int dash;
    [System.NonSerialized] public bool attack;
    [System.NonSerialized] public bool specialAttack;

    private bool isUsingGamePad = false;
    private PlayerIndex gamePadIndex;

    private GamePadState prevState, currState;

    private void Awake()
    {
        controller = GetComponent<PlayerController>();
        ResetInput();

        // Setting up the controller
        for (int i = 0; i < 4; ++i) {
            PlayerIndex testGamePad = (PlayerIndex) i;
            GamePadState testState = GamePad.GetState(testGamePad);
            if (testState.IsConnected) {
                Debug.Log(string.Format("GamePad found {0}", testGamePad));
                gamePadIndex = testGamePad;
                isUsingGamePad = true;
                break;
            }
        }

        FindObjectOfType<MenuScript>().isUsingGamePad = isUsingGamePad;
    }

    private void ResetInput()
    {
        horizontalMove = 0f;
        jump = false;
        crouch = false;
        dash = 0;
        attack = false;
        specialAttack = false;
    }

    private void OnEnable() { ResetInput(); }
    private void OnDisable() { ResetInput(); }

    private void Update()
    {
        if (!isUsingGamePad) {
            horizontalMove = Input.GetAxisRaw("Horizontal");

            if (Input.GetButtonDown("Jump"))
                jump = true;
            else if (Input.GetButtonUp("Jump"))
                jump = false;

            if (Input.GetButtonDown("Crouch")) {
                crouch = true;
                horizontalMove = 0;
            }
            else if (Input.GetButtonUp("Crouch"))
                crouch = false;

            if (Input.GetButtonDown("DashLeft"))
                dash = -1;
            else if (Input.GetButtonDown("DashRight"))
                dash = 1;
            else if (Input.GetButtonUp("DashLeft") && Input.GetButtonUp("DashRight"))
                dash = 0;

            if (Input.GetButtonDown("Fire1"))
                attack = true;
            else if (Input.GetButtonUp("Fire1"))
                attack = false;

            if (Input.GetButtonDown("Fire2"))
                specialAttack = true;
            else if (Input.GetButtonUp("Fire2"))
                specialAttack = false;

            if (Input.GetButtonDown("Transform")) {
                controller.DruidicTransform();
            }
        }
        else {
            prevState = currState;
            currState = GamePad.GetState(gamePadIndex);

            horizontalMove = prevState.ThumbSticks.Left.X;

            if (prevState.Buttons.A == ButtonState.Released &&
                currState.Buttons.A == ButtonState.Pressed)
                jump = true;
            else if (prevState.Buttons.A == ButtonState.Pressed &&
                currState.Buttons.A == ButtonState.Released)
                jump = false;

            if (prevState.ThumbSticks.Left.Y <= -0.8)
                crouch = true;
            else
                crouch = false;

            if (prevState.Buttons.LeftShoulder == ButtonState.Released &&
                currState.Buttons.LeftShoulder == ButtonState.Pressed)
                dash = -1;
            else if (prevState.Buttons.RightShoulder == ButtonState.Released &&
                currState.Buttons.RightShoulder == ButtonState.Pressed)
                dash = 1;
            else if (currState.Buttons.LeftShoulder == ButtonState.Released &&
                currState.Buttons.RightShoulder == ButtonState.Released)
                dash = 0;

            if (prevState.Buttons.X == ButtonState.Released &&
                currState.Buttons.X == ButtonState.Pressed)
                attack = true;
            else if (prevState.Buttons.X == ButtonState.Pressed &&
                currState.Buttons.X == ButtonState.Released)
                attack = false;

            if (prevState.Buttons.Y == ButtonState.Released &&
                currState.Buttons.Y == ButtonState.Pressed)
                specialAttack = true;
            else if (prevState.Buttons.Y == ButtonState.Pressed &&
                currState.Buttons.Y == ButtonState.Released)
                specialAttack = false;

            if (prevState.Buttons.B == ButtonState.Released &&
                currState.Buttons.B == ButtonState.Pressed)
                controller.DruidicTransform();
        }
    }
}