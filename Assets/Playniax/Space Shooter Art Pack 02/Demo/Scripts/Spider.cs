using UnityEngine;
using Playniax.Ignition.Framework;
using Playniax.Ignition.SpriteSystem;

namespace SpaceShooterArtPack02
{
    public class Spider : AnimationGroup
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
        public class SoundSettings
        {
            public AudioProperties intro;
        }

        public float timer;

        public float speed = 4.5f;

        public float friction = .99f;

        public BulletSpawnerBase bulletSpawner;

        public enum StartPosition { Left, Right, Top, Bottom };

        public StartPosition startPosition = StartPosition.Left;

        public SoundSettings soundSettings;
        public AdditionalSettings additionalSettings;

        void Awake()
        {
            additionalSettings.Get(gameObject);

            _StartOffScreen();

            if (bulletSpawner == null) bulletSpawner = GetComponent<BulletSpawnerBase>();

            if (timer > 0)
            {
                additionalSettings.spriteRenderer.enabled = false;
            }
        }

        void Update()
        {
            if (additionalSettings.spriteRenderer == null) return;

            if (additionalSettings.spriteRenderer.enabled == false)
            {
                timer -= 1 * Time.deltaTime;
                if (timer > 0) return;

                soundSettings.intro.Play();

                additionalSettings.spriteRenderer.enabled = true;
            }

            transform.position += _velocity * Time.deltaTime;

            if (friction != 0) _velocity *= 1 / (1 + (Time.deltaTime * friction));

            if (_state == 0)
            {
                Loop("Idle", additionalSettings.spriteRenderer);

                if ((startPosition == StartPosition.Left || startPosition == StartPosition.Right) && Mathf.Abs(_velocity.x) < .25f) _state += 1;
                if ((startPosition == StartPosition.Top || startPosition == StartPosition.Bottom) && Mathf.Abs(_velocity.y) < .25f) _state += 1;
            }
            else if (_state == 1)
            {
                if (Once("Tilt", additionalSettings.spriteRenderer) == 0) _state++;
            }
            else if (_state == 2)
            {
                Loop("Rotating", additionalSettings.spriteRenderer);

                if (bulletSpawner && bulletSpawner.automatically == false) bulletSpawner.UpdateSpawner();
            }
        }

        void _StartOffScreen()
        {
            var position = transform.position;

            if (startPosition == StartPosition.Left)
            {
                position.x = -additionalSettings.screenBounds.extents.x - additionalSettings.size.x / 2;

                _velocity.x = speed;
            }
            else if (startPosition == StartPosition.Right)
            {
                position.x = additionalSettings.screenBounds.extents.x + additionalSettings.size.x / 2;

                _velocity.x = -speed;
            }
            else if (startPosition == StartPosition.Top)
            {
                position.y = additionalSettings.screenBounds.extents.y + additionalSettings.size.y / 2;

                _velocity.y = -speed;
            }
            else if (startPosition == StartPosition.Bottom)
            {
                position.y = -additionalSettings.screenBounds.extents.y - additionalSettings.size.y / 2;

                _velocity.y = speed;
            }

            transform.position = position;
        }

        int _state;
        Vector3 _velocity;
    }
}