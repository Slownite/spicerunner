using UnityEngine;

namespace Playniax.Ignition.Framework
{
    [AddComponentMenu("Playniax/Ignition/Framework/ProgressCounter")]
    public class ProgressCounter : MonoBehaviour
    {
        public virtual void Awake()
        {
            SpawnerBase.ProgressCounter.Reset();
        }

        void OnEnable()
        {
            _progressCounter = GetComponent<SpawnerBase.ProgressCounter>();

            if (_progressCounter == null)
            {
                SpawnerBase.ProgressCounter.resetCounter++;

                PlayerData.progress += 1;
                PlayerData.enemyCount += 1;
            }
        }

        void OnDisable()
        {
            if (_progressCounter == null)
            {
                SpawnerBase.ProgressCounter.resetCounter--;

                PlayerData.progress -= 1;
            }
        }

        SpawnerBase.ProgressCounter _progressCounter;
    }
}