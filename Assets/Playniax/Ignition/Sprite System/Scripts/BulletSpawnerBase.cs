using UnityEngine;

namespace Playniax.Ignition.SpriteSystem
{
    public class BulletSpawnerBase : MonoBehaviour
    {
        public string id;
        public bool automatically = true;

        public virtual void UpdateSpawner()
        {
        }

        void LateUpdate()
        {
            if (automatically) UpdateSpawner();
        }
    }
}
