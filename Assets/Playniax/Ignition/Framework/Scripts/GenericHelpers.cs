using UnityEngine;

namespace Playniax.Ignition.Framework
{
    [System.Serializable]
    public class Timer
    {
        public float timer;
        public float delay = .5f;
        public int count = -1;

        public bool Update()
        {
            if (count == 0) return false;

            timer -= 1 * Time.deltaTime;

            if (timer >= 0) return false;

            if (count > 0) count--;
            if (count != 0) timer = delay;

            return true;
        }
    }
}