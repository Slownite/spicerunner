using UnityEngine;
using Playniax.Ignition.Framework;
using Playniax.Ignition.SpriteSystem;

namespace Playniax.Ignition.VisualEffects
{
    public class LaserSpawner : BulletSpawnerBase
    {
        public GameObject prefab;
        public float size = 1;
        public int orderInLayer = 5;
        public Timer timer;
        public float range = 0;
        public int damage = 5;
        public float ttl = .25f;
        public AudioProperties audioProperties;

        public CollisionData collisionData;

        public override void UpdateSpawner()
        {
            Laser.Fire(prefab, orderInLayer, timer, collisionData, range, ttl, size, damage, audioProperties);
        }

        void Awake()
        {
            if (prefab.scene.rootCount > 0) prefab.SetActive(false);

            if (collisionData == null) collisionData = GetComponent<CollisionData>();
        }
    }
}