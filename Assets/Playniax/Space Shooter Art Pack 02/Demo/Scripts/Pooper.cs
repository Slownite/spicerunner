using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Playniax.Ignition.Framework;
using Playniax.Ignition.SpriteSystem;

namespace SpaceShooterArtPack02
{
    public class Pooper : MonoBehaviour
    {
        [System.Serializable]
        public class AdditionalSettings
        {
            public Camera camera;
            public Bounds screenBounds;
            public Vector2 size;
            public SpriteRenderer spriteRenderer;
            public Animator animator;

            public void Get(GameObject obj)
            {
                if (camera == null) camera = Camera.main;
                if (camera) screenBounds = CameraHelpers.OrthographicBounds(camera);
                if (spriteRenderer == null) spriteRenderer = obj.GetComponent<SpriteRenderer>();
                if (spriteRenderer && size == Vector2.zero) size = spriteRenderer.bounds.size;
                if (animator == null) animator = obj.GetComponent<Animator>();
            }
        }

        [System.Serializable]
        public class BulletSettings
        {
            public GameObject prefab;
            public float scale = 1;
            public float rotation = -90;
            public Vector3 position;
            public float speed = 16f;
            public GameObject effectPrefab;
        }

        [System.Serializable]
        public class SoundSettings
        {
            public AudioProperties intro;
        }

        public enum StartPosition { Left, Right };

        public float timer;
        public StartPosition startPosition = StartPosition.Left;
        public bool movement = true;
        public float speed = 1f;
        public int fireFrame = 3;
        public BulletSettings bulletSettings;
        public SoundSettings soundSettings;
        public AdditionalSettings additionalSettings;

        void Awake()
        {
            additionalSettings.Get(gameObject);

            if (movement) _StartOffScreen();

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

            if (movement) _Movement();
            _Fire();
        }

        void _Fire()
        {
            var frame = additionalSettings.animator.GetCurrentAnimatorClipInfo(0)[0].clip.length * (additionalSettings.animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1) * additionalSettings.animator.GetCurrentAnimatorClipInfo(0)[0].clip.frameRate;

            if (_state == 0 && (int)frame == fireFrame)
            {
                if (bulletSettings.prefab)
                {
                    var obj = AdvancedGameObjectPooler.GetAvailableObject(bulletSettings.prefab);
                    if (obj)
                    {
                        obj.transform.position = transform.position;
                        obj.transform.rotation = transform.rotation;
                        obj.transform.localScale *= bulletSettings.scale;

                        var bulletBase = obj.GetComponent<BulletBase>();
                        if (bulletBase)
                        {
                            bulletBase.transform.Translate(bulletSettings.position, Space.Self);
                            bulletBase.transform.eulerAngles = new Vector3(0, 0, bulletSettings.rotation);

                            bulletBase.velocity = obj.transform.rotation * new Vector3(bulletSettings.speed, 0, 0);
                        }

                        obj.SetActive(true);
                    }
                }

                if (bulletSettings.effectPrefab)
                {
                    var obj = AdvancedGameObjectPooler.GetAvailableObject(bulletSettings.effectPrefab);
                    obj.transform.SetParent(transform, false);
                    obj.transform.Translate(bulletSettings.position, Space.Self);
                    obj.transform.localScale *= bulletSettings.scale;

                    obj.SetActive(true);
                }

                _state = 1;
            }
            else if (_state == 1 && (int)frame != fireFrame)
            {
                _state = 0;
            }
        }

        void _Movement()
        {
            var position = transform.position;

            if (startPosition == StartPosition.Left)
            {
                position += Vector3.right * speed * Time.deltaTime;

                if (position.x > additionalSettings.screenBounds.extents.x + additionalSettings.size.x / 2)
                {
                    position.x = -additionalSettings.screenBounds.extents.x - additionalSettings.size.x / 2;
                }
            }
            else if (startPosition == StartPosition.Right)
            {
                position += Vector3.left * speed * Time.deltaTime;

                if (position.x < -additionalSettings.screenBounds.extents.x - additionalSettings.size.x / 2)
                {
                    position.x = additionalSettings.screenBounds.extents.x + additionalSettings.size.x / 2;
                }
            }

            transform.position = position;
        }

        void _StartOffScreen()
        {
            var position = transform.position;

            if (startPosition == StartPosition.Left)
            {
                position.x = -additionalSettings.screenBounds.extents.x - additionalSettings.size.x / 2;
            }
            else if (startPosition == StartPosition.Right)
            {
                position.x = additionalSettings.screenBounds.extents.x + additionalSettings.size.x / 2;
            }

            transform.position = position;
        }

        int _state;
    }
}