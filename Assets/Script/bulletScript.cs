using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletScript : MonoBehaviour
{
    public float speed = 30f;
    public float deactivate_Timer = 3f;
    public int damage = 1;

    [HideInInspector]
    public bool is_EnemyBullet = false;
    // Start is called before the first frame update
    void Start()
    {
        if (is_EnemyBullet)
            speed *= -1; //set to negative value to go left
        Invoke("DeactivateGameObject", deactivate_Timer);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        Vector2 temp = transform.position;
        temp.x += speed * Time.deltaTime;
        transform.position = temp;
    }

    void DeactivateGameObject()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "enemy")
        {
            Destroy(gameObject);
            Destroy(this);
        }
    }
}
