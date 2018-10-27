using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour {

    public GameObject PauseUI;

    bool gameIsPaused = false;

	void Start () {
        PauseUI.SetActive(false);
	}
	
	void Update () {
		if (Input.GetButtonDown("Pause")) {
            gameIsPaused = !gameIsPaused;
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
    public void Restart() { SceneManager.LoadScene("Main"); }
    public void Quit() { Application.Quit(); }
}
