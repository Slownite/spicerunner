using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gameHandling : MonoBehaviour
{
    public Player player;
    public bool gameOverBool;
    public bool endLevel;
    // Start is called before the first frame update
    void Start()
    {
        gameOverBool = false;
        endLevel = false;
    }
    void Update()
    {
        gameOver();
        nextLevel();    
    }
    public void gameOver()
    {
        if (gameOverBool)
        {
            int level = SceneManager.GetActiveScene().buildIndex;
            Save.SaveGame(player, level);
            Info info = FindObjectOfType<Info>();
            info.gold = player.gold;
            SceneManager.LoadScene(level+1);
        }
    }
    public void nextLevel()
    {
        if (endLevel)
        {
            int level =  SceneManager.GetActiveScene().buildIndex;
            Save.SaveGame(player, level);
            Info info = FindObjectOfType<Info>();
            info.gold = player.gold;
            SceneManager.LoadScene(level + 1);
        }
    }



    // Update is called once per frame
    
}
