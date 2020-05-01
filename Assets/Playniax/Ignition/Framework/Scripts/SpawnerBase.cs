using UnityEngine;

namespace Playniax.Ignition.Framework
{
    [AddComponentMenu("Playniax/Ignition/Framework/SpawnerBase")]
    public class SpawnerBase : MonoBehaviour
    {
        public class ProgressCounter : MonoBehaviour
        {
            public static int resetCounter;

            public static void Reset()
            {
                if (resetCounter == 0)
                {
                    PlayerData.progress = 0;
                    PlayerData.enemyCount = 0;
                }
            }

            void OnDisable()
            {
                PlayerData.progress -= 1;
            }
        }

        public static int count;

        public virtual void Awake()
        {
            ProgressCounter.Reset();
        }

        void OnEnable()
        {
            ProgressCounter.resetCounter++;

            count++;
        }

        void OnDisable()
        {
            ProgressCounter.resetCounter--;

            count--;
        }
    }
}