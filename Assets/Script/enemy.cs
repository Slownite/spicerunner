using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{
    public int damage;

    public float speed;
    public float rotate_speed;

    public float health;
    private float maxHealth;

    public Player player;
    private Transform target;

    private bool canMove = true;
    public bool canRotate;
    public bool canShoot;

    public GameObject bullet;
    public GameObject enemyBullet;
    public Transform weapon;

    public GameObject explosion;
    public AudioSource explosionSound;
    public AudioSource enemyWeaponSound;

    public float bound_X = -11f;

    // Start is called before the first frame update
    void Start()
    {
        target = player.transform;
        if(canRotate)
        {
            if(Random.Range(0,2) > 0) // returns 0 or 1
            {
                rotate_speed = Random.Range(rotate_speed, rotate_speed + 20f); // randomize rotate speed +20
                rotate_speed *= -1; // rotate forwards or backwards
            } else {
                rotate_speed = Random.Range(rotate_speed, rotate_speed + 20f);
            }
        }
        if (canShoot)
            Invoke("EnemyShoot", Random.Range(1f, 2f));
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        RotateEnemy();
        die();
    }

    void Move()
    {
        if(canMove)
        {
            Vector3 temp = transform.position;
            temp.x -= speed * Time.deltaTime;
            transform.position = temp;

            if (temp.x < bound_X)
                gameObject.SetActive(false);
        }
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
            Destroy(gameObject);
            explosionSound.Play();
            Instantiate(explosion, transform.position, transform.rotation);    
            FindObjectOfType<Player>().gold += 1;
            
        }
        if (transform.position.x == -9)
        {
            Destroy(gameObject);      
        }
    }

    void RotateEnemy()
    {
        if(canRotate)
        {
            transform.Rotate(new Vector3(0f, 0f, rotate_speed * Time.deltaTime), Space.World);
        }
    }

    void EnemyShoot()
    {
        enemyWeaponSound.Play();
        GameObject bullet = Instantiate(enemyBullet, weapon.position, Quaternion.identity);
        bullet.GetComponent<bulletScript>().is_EnemyBullet = true;

        if (canShoot)
            Invoke("EnemyShoot", Random.Range(1f, 2f));
    }
}