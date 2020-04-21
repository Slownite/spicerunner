using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class mainMenu : MonoBehaviour
{
    public Info gameInfo;
   public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void loadGame()
    {
        GameProgress game = Save.LoadGame();
        gameInfo.gold = game.gold;
        SceneManager.LoadScene(game.level);
        
    }
    public void quitGame()
    {
        //Debug.Log("quit");
        //Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
