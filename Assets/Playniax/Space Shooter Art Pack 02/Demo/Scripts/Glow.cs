using UnityEngine;

namespace SpaceShooterArtPack02
{
    public class Glow : MonoBehaviour
    {
        public float speed = 1;
        public float min = .25f;
        public float max = 1;

        SpriteRenderer spriteRenderer;

        void Awake()
        {
            if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();

            if (spriteRenderer != null) _Update(spriteRenderer.color, true);
        }

        void Update()
        {
            spriteRenderer.color = _Update(spriteRenderer.color, true);
        }

        public Color _Update(Color color, bool update = true)
        {
            if (update) color.a += speed * Time.deltaTime;

            if (color.a < min)
            {
                color.a = min;

                speed = Mathf.Abs(speed);
            }

            if (color.a > max)
            {
                color.a = max;

                speed = -Mathf.Abs(speed);
            }

            return color;
        }
    }
}