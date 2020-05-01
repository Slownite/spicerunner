using UnityEngine;

namespace Playniax.Ignition.SpriteSystem
{
    [AddComponentMenu("Playniax/Ignition/SpriteSystem/AlphaEffects")]
    public class AlphaEffects : MonoBehaviour
    {
        [System.Serializable]
        public class PingPong
        {
            public float speed = 1;
            public float min = .25f;
            public float max = 1;

            public Color Update(Color color, bool update = true)
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

        public enum Mode { PingPong }

        public Mode mode = Mode.PingPong;

        public PingPong pingPong;

        SpriteRenderer spriteRenderer;

        void Awake()
        {
            if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();

            if (spriteRenderer != null) _Update(false);
        }

        void Update()
        {
            _Update();
        }

        void _Update(bool update = true)
        {
            if (mode == Mode.PingPong)
            {
                spriteRenderer.color = pingPong.Update(spriteRenderer.color, update);
            }
        }
    }
}