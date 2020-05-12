using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Playniax.Ignition.SpriteSystem;

public class Health : MonoBehaviour
{
    public GameObject player;
    public Slider healthBar;

    public void Start()
    {
        healthBar.maxValue = 1000;
        healthBar.value = 1000;
        
    }
    public void Update()
    {
        GameObject Player = GameObject.Find("Player");
        CollisionData hpData = Player.GetComponent<CollisionData>();
        healthBar.value = hpData.structuralIntegrity;
    }

    public void setHealth(int health)
    {
        healthBar.value = health;
    }
}
