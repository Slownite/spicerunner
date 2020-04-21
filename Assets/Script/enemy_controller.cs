using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chasing : MonoBehaviour {

	public Transform target;
	public float speed;

	private float health;
	private float maxHealth;
	public GameObject explostion;

	private Text healText;
	private Image healBar;

	// Use this for initialization
	void Start () {
		speed = 10f;
		health = 10.0f;
		maxHealth = 10.0f;
		healText = transform.Find("EnemyCanvas").Find("HealthBarText").GetComponent<Text>();
		healBar = transform.Find("EnemyCanvas").Find("MaxHealthBar").Find("HealthBar").GetComponent<Image>();
	}

	// Update is called once per frame
	void Update () {
		transform.LookAt(target, Vector3.up);
		transform.position += transform.forward * speed * Time.deltaTime;
		healText.text = health.ToString();
		healBar.fillAmount = health / maxHealth;
	}

	void OnCollisionEnter(Collision col) {
		if(col.gameObject.tag == "Bullet") {
			health -= 5;
			if(health < 1) {
				Destroy(this);
				Instantiate(explostion, transform.position, transform.rotation);
				Destroy(gameObject);
			}
		}
	}
}
