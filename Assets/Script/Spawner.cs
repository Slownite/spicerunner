using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // only need to randomize between y axis values
    public float min_Y = -4.3f, max_Y = 4.3f;

    public GameObject[] asteroids;
    public GameObject[] enemies;

    public float timer = 4f;
    // Start is called before the first frame update
    void Start()
    {
        Invoke("Spawn", timer);
    }

    // Update is called once per frame
    void Update()
    {

    }
    void Spawn()
    {
        float pos_Y = Random.Range(min_Y, max_Y);
        Vector2 temp = transform.position;
        temp.y = pos_Y;

        
        
            Instantiate(asteroids[Random.Range(0, asteroids.Length)], temp, Quaternion.identity);
        
      
        Invoke("Spawn", timer);
    }
}