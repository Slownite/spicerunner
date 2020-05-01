using UnityEngine;
using Playniax.Ignition.SpriteSystem;
using Playniax.Ignition.VisualEffects;

namespace SpaceShooterArtPack02
{
    public class Revengestar : MonoBehaviour
    {
        public float sensitivity = .1f;
        public Sprite[] sprites;

        public SpriteRenderer spriteRenderer;

        void Awake()
        {
            if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
        }

        void Update()
        {
            var velocity = (transform.position - _previousPosition) / Time.deltaTime;

            velocity.y *= sensitivity;

            if (velocity.y < -1) velocity.y = -1;
            if (velocity.y > 1) velocity.y = 1;

            if (sprites.Length > 0)
            {
                var idle = sprites.Length / 2;

                int frame = idle - (int)(idle * velocity.y);

                spriteRenderer.sprite = sprites[frame];
            }

            _previousPosition = transform.position;
        }

        Vector3 _previousPosition;
    }
}