using UnityEngine;
using Playniax.Ignition.Framework;
using Playniax.Ignition.SpriteSystem;

namespace SpaceShooterArtPack02
{
    public class SateliteBossGun : MonoBehaviour
    {
        [System.Serializable]
        public class BulletSettings
        {
            public GameObject prefab;
            public Vector3 position;
            public float speed = 16f;
            public GameObject effectPrefab;
        }

        public float speed = 10;
        public float frame;
        public Sprite[] frames;
        public SpriteRenderer spriteRenderer;

        public float timer;
        public int state;

        public BulletSettings bulletSettings;

        void Awake()
        {
            if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();

            _timer = Random.Range(2, 4);
        }

        void Update()
        {
            spriteRenderer.sprite = frames[(int)frame];

            var closest = PlayersGroup.GetAny();
            if (closest == null) return;

            if (state == 0)
            {
                timer -= 1 * Time.deltaTime;
                if (timer < 0)
                {
                    state = 1;
                }
            }

            if (state == 1)
            {
                if ((transform.position - closest.transform.position).x < 0)
                {
                    frame += speed * Time.deltaTime;
                    if (frame > frames.Length - 1)
                    {
                        frame = frames.Length - 1;
                        timer = Random.Range(3, 6);
                        state = 0;
                    }
                }
                else if ((transform.position - closest.transform.position).x > 0)
                {
                    frame -= speed * Time.deltaTime;
                    if (frame < 0)
                    {
                        frame = 0;
                        timer = Random.Range(3, 6);
                        state = 0;
                    }
                }
            }

            _Fire();
        }

        void _Fire()
        {
            _timer -= 1 * Time.deltaTime;
            if (_timer > 0) return;

            _timer = Random.Range(.25f, .75f);

            if (frame == 0 || frame == frames.Length - 1)
            {
                if (bulletSettings.prefab)
                {
                    var obj = AdvancedGameObjectPooler.GetAvailableObject(bulletSettings.prefab);
                    if (obj)
                    {
                        obj.transform.position = transform.position;
                        obj.transform.rotation = transform.rotation;
                        obj.SetActive(true);

                        var bulletBase = obj.GetComponent<BulletBase>();
                        if (bulletBase)
                        {
                            var direction = -1;
                            var position = bulletSettings.position;

                            if (frame == frames.Length - 1) direction = -direction;

                            position.x *= -direction;

                            bulletBase.velocity = new Vector3(bulletSettings.speed + Random.Range(0, 5), 0, 0) * direction;

                            bulletBase.transform.position += position;

                            if (direction == -1) bulletBase.transform.rotation *= Quaternion.Euler(0, 180, 0);

                            var spark = AdvancedGameObjectPooler.GetAvailableObject(bulletSettings.effectPrefab);
                            spark.transform.position = bulletBase.transform.position;
                            spark.transform.SetParent(transform, true);
                            spark.SetActive(true);
                        }
                    }
                }
            }
        }
        float _timer;
    }
}