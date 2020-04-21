using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int health = 100;
    public int gold;
    public Health healthBar;
    public float speed = 20f;

    public float min_Y, max_Y, min_X, max_X;

    public GameObject bullet;

    public Transform weapon;

    public float attack_timer = 4.0f;
    private float current_attack_timer;
    private bool canAttack;

    public AudioSource weaponFire;
    public AudioSource engineSound;


    // Start is called before the first frame update
    void Start()
    {
        engineSound.Play();
        current_attack_timer = attack_timer;
        healthBar.setMaxHealth(health);
        healthBar.setHealth(health);
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        Shoot();
        healthBar.setHealth(health);
        die();
    }

    void MovePlayer()
    {
        if(Input.GetAxisRaw("Vertical") > 0f)
        {
            Vector3 temp = transform.position;
            temp.y += speed * Time.deltaTime;

            if (temp.y > max_Y)
                temp.y = max_Y;

            transform.position = temp;
        }
        else if (Input.GetAxisRaw("Vertical") < 0f)
        {
            Vector3 temp = transform.position;
            temp.y -= speed * Time.deltaTime;

            if (temp.y < min_Y)
                temp.y = min_Y;

            transform.position = temp;
        }
        if (Input.GetAxisRaw("Horizontal") > 0f)
        {
            Vector3 temp = transform.position;
            temp.x += speed * Time.deltaTime;

            if (temp.x > max_X)
                temp.x = max_X;

            transform.position = temp;
        }
        else if (Input.GetAxisRaw("Horizontal") < 0f)
        {
            Vector3 temp = transform.position;
            temp.x -= speed * Time.deltaTime;

            if (temp.x < min_X)
                temp.x = min_X;

            transform.position = temp;
        }
    }

    void Shoot()
    {
        attack_timer += Time.deltaTime;
        if (attack_timer > current_attack_timer)
        {
            canAttack = true;
        }
        if(Input.GetKeyDown(KeyCode.J))
        {
            if (canAttack)
            {
                canAttack = false;
                attack_timer = 0f;
                weaponFire.Play();
                Instantiate(bullet, weapon.position, Quaternion.identity);
            }
        }
    }
    public void loadPlayerData()
    {
        Info info = FindObjectOfType<Info>();
        if (info != null)
        {
            this.gold = info.gold;
        }
    }
    public void die()
    {
        if(health<=0)
        {
            FindObjectOfType<gameHandling>().gameOverBool = true;
        }
    }
}
