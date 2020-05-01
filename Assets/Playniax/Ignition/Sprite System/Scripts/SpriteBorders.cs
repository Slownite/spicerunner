using UnityEngine;
using Playniax.Ignition.Framework;

namespace Playniax.Ignition.SpriteSystem
{
    [AddComponentMenu("Playniax/Ignition/SpriteSystem/SpriteBorders")]
    public class SpriteBorders : MonoBehaviour
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

        public AdditionalSettings additionalSettings;

        void Awake()
        {
            additionalSettings.Get(gameObject);
        }

        void OnDisable()
        {
            _previousPosition = Vector3.zero;
        }

        void Update()
        {
            _Update();
        }

        void _OnBorder()
        {
            gameObject.SetActive(false);

            if (name.Contains(GameObjectPooler.marker) == false) Destroy(gameObject);
        }

        void _Update()
        {
            if (_previousPosition == Vector3.zero) _previousPosition = transform.position;

            var moved = transform.position - _previousPosition;

            var position = transform.position;

            position -= new Vector3(additionalSettings.camera.transform.position.x, additionalSettings.camera.transform.position.y);

            if (moved.x > 0 && position.x - additionalSettings.size.x / 2 > additionalSettings.screenBounds.size.x / 2)
            {
                _OnBorder();
            }
            else if (moved.x < 0 && position.x + additionalSettings.size.x / 2 < -additionalSettings.screenBounds.size.x / 2)
            {
                _OnBorder();
            }

            if (moved.y > 0 && position.y - additionalSettings.size.y / 2 > additionalSettings.screenBounds.size.y / 2)
            {
                _OnBorder();
            }
            else if (moved.y < 0 && position.y + additionalSettings.size.y / 2 < -additionalSettings.screenBounds.size.y / 2)
            {
                _OnBorder();
            }

            _previousPosition = transform.position;
        }

        Vector3 _previousPosition;
    }
}