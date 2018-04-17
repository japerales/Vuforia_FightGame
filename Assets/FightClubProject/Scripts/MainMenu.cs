using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour {


    public void LoadMainScene()
    {
        SceneManager.LoadScene("Game");
    }

    public void Exit() {
        Application.Quit();
    }
}
