using UnityEngine;

namespace Playniax.Ignition.SpriteSystem
{
    [AddComponentMenu("Playniax/Ignition/SpriteSystem/CollisionDelay")]
    public class CollisionDelay : MonoBehaviour
    {
        public int count = 30;
        public float timer;
        public float delay = .1f;

        public SpriteRenderer spriteRenderer;

        public SpriteColliderBase spriteColliderBase;

        void Awake()
        {
            if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();

            if (spriteColliderBase == null) spriteColliderBase = GetComponent<SpriteColliderBase>();

            if (spriteRenderer == null)
            {
                enabled = false;

                return;
            }

            if (spriteColliderBase) spriteColliderBase.enabled = false;
        }

        void Update()
        {
            timer -= 1 * Time.deltaTime;

            if (timer < 0)
            {
                timer = delay;

                count -= 1;

                if (count > 0)
                {
                    if (spriteRenderer.enabled)
                    {
                        spriteRenderer.enabled = false;
                    }
                    else
                    {
                        spriteRenderer.enabled = true;
                    }
                }
                else
                {
                    spriteRenderer.enabled = true;

                    if (spriteColliderBase) spriteColliderBase.enabled = true;

                    enabled = false;
                }
            }
        }
    }
}
