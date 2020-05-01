using UnityEngine;
using Playniax.Ignition.Framework;
using Playniax.Ignition.SpriteSystem;

namespace SpaceShooterArtPack02
{
    public class Launcher : AnimationGroup
    {
        [System.Serializable]
        public class AdditionalSettings
        {
            public Camera camera;
            public Bounds screenBounds;
            public Vector2 size;
            public SpriteRenderer spriteRenderer;

            public void Get(GameObject obj)
            {
                if (camera == null) camera = Camera.main;
                if (camera) screenBounds = CameraHelpers.OrthographicBounds(camera);
                if (spriteRenderer == null) spriteRenderer = obj.GetComponent<SpriteRenderer>();
                if (spriteRenderer && size == Vector2.zero) size = spriteRenderer.bounds.size;
            }
        }

        [System.Serializable]
        public class Rocket
        {
            public GameObject prefab;
            public Vector3 position;
            public float timer = 1;
            public float interval = 3;
        }

        public Rocket rocket;
        public float height = 1.1f;
        public AdditionalSettings additionalSettings;

        void Awake()
        {
            additionalSettings.Get(gameObject);

            var position = transform.position;
            position.y = -additionalSettings.screenBounds.extents.y + additionalSettings.spriteRenderer.bounds.size.y / 2 + height;
            transform.position = position;
        }

        void Update()
        {
            if (_state == 0 && PlayersGroup.list.Count > 0)
            {
                rocket.timer -= 1 * Time.deltaTime;

                if (rocket.timer < 0) _state++;
            }
            else if (_state == 1)
            {
                if (Once("Hatch", additionalSettings.spriteRenderer) == 0) _state++;
            }
            else if (_state == 2)
            {
                var obj = Instantiate(rocket.prefab);
                obj.transform.position = transform.position + rocket.position;
                obj.GetComponent<SpriteRenderer>().sortingOrder = additionalSettings.spriteRenderer.sortingOrder - 1;
                _state++;
            }
            else if (_state == 3)
            {
                if (Once("Hatch", additionalSettings.spriteRenderer, -1) == 0)
                {
                    SetFrame("Hatch", 0);

                    rocket.timer = Random.Range(1, 1 + rocket.interval);

                    _state = 0;
                }
            }
        }

        int _state;
    }
}