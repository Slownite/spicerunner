using UnityEngine;

namespace Playniax.Ignition.SpriteSystem
{
    [AddComponentMenu("Playniax/Ignition/SpriteSystem/MotionBase")]
    public class MotionBase : MonoBehaviour
    {
        [System.Serializable]
        public class Linear
        {
            public Vector3 velocity;
            public float friction;
            public bool enabled = true;

            public void Update(GameObject obj)
            {
                if (enabled)
                {
                    obj.transform.position += velocity * Time.deltaTime;

                    if (friction != 0) velocity *= 1 / (1 + (Time.deltaTime * friction));
                }
            }
        }

        [System.Serializable]
        public class Rotate
        {
            public Vector3 rotation = new Vector3(0, 0, 100);
            public bool enabled = true;

            public void Update(GameObject obj)
            {
                if (enabled) obj.transform.Rotate(rotation * Time.deltaTime);
            }
        }

        public enum Mode { Linear, Rotate };

        public Mode mode = Mode.Linear;

        public Linear linear;
        public Rotate rotate;

        void Update()
        {
            if (mode == Mode.Linear)
            {
                linear.Update(gameObject);
            }
            else if (mode == Mode.Rotate)
            {
                rotate.Update(gameObject);
            }
        }
    }
}