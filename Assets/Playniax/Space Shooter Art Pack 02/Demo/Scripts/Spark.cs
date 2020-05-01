using UnityEngine;

namespace SpaceShooterArtPack02
{
    public class Spark : MonoBehaviour
    {
        public float speed = 10f;
        public SpriteRenderer spriteRenderer;

        void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();

            if (spriteRenderer == null) Destroy(gameObject);
        }

        void Update()
        {
            spriteRenderer.color -= new Color(0, 0, 0, speed) * Time.deltaTime;

            if (spriteRenderer.color.a <= 0) Destroy(gameObject);
        }
    }
}