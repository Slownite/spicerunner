using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
    public static bool gamePause = false;
    public GameObject pauseMenuUI;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (gamePause)
                resume();
            else
                pause();
        }
    }
    public void resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gamePause = false;
    }
    public void loadMenu()
    {
        SceneManager.LoadScene(0);
    }
    void pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gamePause = true;
    }
}
