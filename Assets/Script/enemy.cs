using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{
    public int damage;
    public float speed;
    public int health;
    public Player player;
    private Transform target;
        // Start is called before the first frame update
    void Start()
    {
        target = player.transform;
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 temp = transform.position;
        temp.x -= speed * Time.deltaTime;
        transform.position = temp;
        die();
        
    }

    public void make_damage(Player obj)
    {
        obj.health -= damage;
    }
    public void take_damage(bulletScript bullet)
    {
        health -= bullet.damage;
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "bullet")
        {
         
            take_damage(collision.gameObject.GetComponent<bulletScript>());
        }
        if (collision.gameObject.tag == "Player")
        {
            make_damage(collision.gameObject.GetComponent<Player>());
            Destroy(gameObject);
        }
    }
    private void die()
    {
        if (health <= 0)
        {
            FindObjectOfType<Player>().gold += 1;
            Destroy(gameObject);
        }
        if (transform.position.x == -9)
        {
            Destroy(gameObject);
        }
    }
}
