using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletScript : MonoBehaviour
{
    public float speed = 30f;
    public float deactivate_Timer = 3f;
    public int damage = 1;
    // Start is called before the first frame update
    void Start()
    {
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
}
