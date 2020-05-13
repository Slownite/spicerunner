using UnityEngine;
using UnityEngine.SceneManagement;
using Playniax.Ignition.Framework;
using Playniax.Ignition.SpriteSystem;


public class gameHandling : MonoBehaviour
{
    public int score_next_scene;
    // Start is called before the first frame update
    void Update()
    {
        gameOver();
        nextLevel();    
    }
    public void gameOver()
    {
        
        if (GameObject.Find("Player") == null || GameObject.Find("Player").GetComponent<CollisionData>().structuralIntegrity <= 0)
        {
            int level = SceneManager.GetSceneByPath("Assets/Scenes/GameOver.unity").buildIndex;
            SceneManager.LoadScene(7);
        }
    }
    public void nextLevel()
    {
        if (PlayerData.Get(0).scoreboard >= score_next_scene)
        {
            int level =  SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(level + 1);
        }
    }



    // Update is called once per frame
    
}
