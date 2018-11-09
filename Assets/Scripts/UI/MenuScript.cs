using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using XInputDotNetPure;

public class MenuScript : MonoBehaviour {

    public GameObject PauseUI;

    [System.NonSerialized] public bool isUsingGamePad = false;
    [System.NonSerialized] public PlayerIndex gamePadIndex;

    private GamePadState prevState, currState;


    bool gameIsPaused = false;

	void Start () {
        PauseUI.SetActive(false);
	}

    void Update () {
        if (!isUsingGamePad) {
            if (Input.GetButtonDown("Pause")) {
                gameIsPaused = !gameIsPaused;
            }
        }
        else {
            prevState = currState;
            currState = GamePad.GetState(gamePadIndex);

            if (prevState.Buttons.Start == ButtonState.Released &&
                currState.Buttons.Start == ButtonState.Pressed) {
                gameIsPaused = !gameIsPaused;
            }
        }

        if (gameIsPaused) {
            PauseUI.SetActive(true);
            Time.timeScale = 0;
        }
        else {
            PauseUI.SetActive(false);
            Time.timeScale = 1;
        }
    }

    public void Resume() { gameIsPaused = false; }
    public void Restart() { SceneManager.LoadScene("Trainning Ground"); }
    public void Quit() { Application.Quit(); }
}
