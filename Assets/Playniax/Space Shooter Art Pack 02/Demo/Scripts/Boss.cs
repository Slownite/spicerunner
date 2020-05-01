using UnityEngine;
using Playniax.Ignition.Framework;
using Playniax.Ignition.SpriteSystem;

namespace SpaceShooterArtPack02
{
    public class Boss : MonoBehaviour
    {
        [System.Serializable]
        public class BulletSettings
        {
            public GameObject prefab;
            public Vector3 position;
            public float speed = 16f;
            public GameObject effectPrefab;
        }

        public float rotation = 0;
        public float radius = 1.7f;
        public Sprite[] frames;
        public SpriteRenderer[] guns;

        public BulletSettings bulletSettings;

        void Update()
        {
            _Update();
        }

        void _Update()
        {
            var fire = -1;

            _timer += 1 * Time.deltaTime;
            if (_timer > .1)
            {
                fire = Random.Range(0, guns.Length);

                _timer = 0;
            }

            for (int i = 0; i < guns.Length; i++)
            {
                if (guns[i] == null) continue;

                var angle = i * Mathf.PI * 2f / guns.Length;

                angle += rotation;

                guns[i].transform.localPosition = new Vector3(Mathf.Cos(angle) * radius, -Mathf.Sin(angle) * radius);

                var frame = (int)MathHelpers.Mod(angle * Mathf.Rad2Deg, 360);

                guns[i].sprite = frames[frame];

                if (bulletSettings.prefab && fire == i)
                {
                    var obj = AdvancedGameObjectPooler.GetAvailableObject(bulletSettings.prefab);
                    if (obj)
                    {
                        obj.transform.position = guns[i].transform.position;
                        obj.transform.rotation = guns[i].transform.rotation;

                        var bulletBase = obj.GetComponent<BulletBase>();
                        if (bulletBase)
                        {
                            bulletBase.transform.eulerAngles = new Vector3(0, 0, -frame);

                            bulletBase.velocity = obj.transform.rotation * new Vector3(bulletSettings.speed + Random.Range(0, 5), 0, 0);

                            bulletBase.transform.Translate(bulletSettings.position, Space.Self);
                        }

                        obj.SetActive(true);
                    }
                }

                if (bulletSettings.effectPrefab && fire == i)
                {
                    var obj = AdvancedGameObjectPooler.GetAvailableObject(bulletSettings.effectPrefab);
                    obj.transform.SetParent(guns[i].transform, false);
                    obj.SetActive(true);
                }
            }

            rotation += -1 * Time.deltaTime;
        }

        float _timer;
    }
}