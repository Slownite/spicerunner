using UnityEngine;

namespace Playniax.Ignition.Prototyping
{
    public class Activator : MonoBehaviour
    {
        public GameObject obj;
        public float timer;

        void Awake()
        {
            obj.SetActive(false);
        }

        void Update()
        {
            timer -= 1 * Time.deltaTime;

            if (timer <= 0)
            {
                timer = 0;
                obj.SetActive(true);
                enabled = false;
            }
        }
    }
}