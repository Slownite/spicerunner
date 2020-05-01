using UnityEngine;
using Playniax.Ignition.Framework;

namespace Playniax.Ignition.SpriteSystem
{
    [AddComponentMenu("Playniax/Ignition/SpriteSystem/StartOffScreen")]
    public class StartOffScreen : MonoBehaviour
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

        public enum StartPosition { Left, Right, Top, Bottom };

        public StartPosition startPosition = StartPosition.Left;

        public AdditionalSettings additionalSettings;

        void OnEnable()
        {
            _Init();
        }

        void _Init()
        {
            additionalSettings.Get(gameObject);

            //if (additionalSettings.spriteRenderer == null) return;

            var position = transform.position;

            if (startPosition == StartPosition.Left)
            {
                position.x = -additionalSettings.screenBounds.extents.x - additionalSettings.size.x / 2;
            }
            else if (startPosition == StartPosition.Right)
            {
                position.x = additionalSettings.screenBounds.extents.x + additionalSettings.size.x / 2;
            }
            else if (startPosition == StartPosition.Top)
            {
                position.y = additionalSettings.screenBounds.extents.y + additionalSettings.size.y / 2;
            }
            else if (startPosition == StartPosition.Bottom)
            {
                position.y = -additionalSettings.screenBounds.extents.y - additionalSettings.size.y / 2;
            }

            transform.position = position;
        }
    }
}