using UnityEngine;

namespace Playniax.Ignition.SpriteSystem
{
    public class SpawnerTrigger : MonoBehaviour
    {
        public BulletSpawnerBase[] spawners;

        public KeyCode key = KeyCode.Space;

        void LateUpdate()
        {
            if (Input.GetKey(key))
            {
                _UpdateSpawners();
            }
        }

        void OnEnable()
        {
            if (spawners != null && spawners.Length == 0) spawners = GetComponents<BulletSpawnerBase>();

            for (int i = 0; i < spawners.Length; i++)
            {
                spawners[i].automatically = false;
            }
        }

        void _UpdateSpawners()
        {
            for (int i = 0; i < spawners.Length; i++)
            {
                if (spawners[i].automatically == false) spawners[i].UpdateSpawner();
            }
        }
    }
}