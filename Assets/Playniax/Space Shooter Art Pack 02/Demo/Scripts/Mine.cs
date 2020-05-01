using UnityEngine;
using Playniax.Ignition.Framework;
using Playniax.Ignition.SpriteSystem;
using Playniax.Ignition.ParticleSystem;

namespace SpaceShooterArtPack02
{
    public class Mine : AnimationGroup
    {
        [System.Serializable]
        public class ShrapnelProperties
        {
            public GameObject prefab;
            public float speed = 12;
            public int parts = 16;

            public void Init()
            {
                if (prefab && prefab.scene.rootCount > 0) prefab.SetActive(false);
            }
        }

        public float armedDistance = 4f;

        public ShrapnelProperties shrapnelProperties;

        public SpriteRenderer spriteRenderer;

        void Awake()
        {
            if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();

            shrapnelProperties.Init();
        }

        void Update()
        {
            if (_state == 0)
            {
                Loop("Idle", spriteRenderer);

                var closest = _GetClosest();

                if (closest && Vector3.Distance(closest.transform.position, transform.position) < armedDistance) _state = 1;
            }
            else if (_state == 1)
            {
                var playing = Once("Armed", spriteRenderer);

                if (playing == 0)
                {
                    EmitterGroup.Play("Explosion 1", transform.position, 1, spriteRenderer.sortingOrder);

                    _Shrapnel();

                    Destroy(gameObject);
                }
            }
        }

        GameObject _GetClosest()
        {
            var active = PlayersGroup.GetActive();

            if (active.Count == 0) return null;

            if (active.Count == 1 && active[0]) return active[0].gameObject;

            var closest = active[0].gameObject;

            for (int i = 0; i < active.Count; i++)
            {
                if (closest != active[i] && Vector3.Distance(transform.position, active[i].transform.position) < Vector3.Distance(transform.position, closest.transform.position))
                {
                    closest = active[i].gameObject;
                }
            }

            return closest;
        }

        void _Shrapnel()
        {
            for (int i = 0; i < shrapnelProperties.parts; i++)
            {
                var obj = AdvancedGameObjectPooler.GetAvailableObject(shrapnelProperties.prefab);
                if (obj)
                {
                    var bullet = obj.GetComponent<BulletBase>();
                    if (bullet)
                    {
                        obj.transform.position = transform.position;

                        float angle = i * (360f / shrapnelProperties.parts) * Mathf.Deg2Rad;

                        obj.transform.eulerAngles = new Vector3(0, 0, angle * Mathf.Rad2Deg);

                        bullet.velocity = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * shrapnelProperties.speed;
                    }

                    obj.SetActive(true);
                }
            }
        }

        int _state;
    }
}
