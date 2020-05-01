using UnityEngine;

namespace Playniax.Ignition.SpriteSystem
{
    [AddComponentMenu("Playniax/Ignition/SpriteSystem/SpriteToScreenSize")]
    public class SpriteToScreenSize : MonoBehaviour
    {
        [System.Serializable]
        public class AdditionalSettings
        {
            public Camera camera;
            public SpriteRenderer spriteRenderer;

            public void Get(GameObject obj)
            {
                if (camera == null) camera = Camera.main;
                if (spriteRenderer == null) spriteRenderer = obj.GetComponent<SpriteRenderer>();
            }
        }

        public AdditionalSettings additionalSettings;

        public float scale = 1f;

        void Awake()
        {
            additionalSettings.Get(gameObject);

            UpdateSize();
        }

        public void UpdateSize()
        {
            if (additionalSettings.camera && additionalSettings.spriteRenderer && additionalSettings.spriteRenderer.sprite)
            {
                float cameraHeight = additionalSettings.camera.orthographicSize * 2;

                Vector2 cameraSize = new Vector2(additionalSettings.camera.aspect * cameraHeight, cameraHeight);
                Vector2 spriteSize = additionalSettings.spriteRenderer.sprite.bounds.size;

                if (cameraSize.x / spriteSize.x > cameraSize.y / spriteSize.y)
                {
                    transform.localScale *= cameraSize.x / spriteSize.x;
                }
                else
                {
                    transform.localScale *= cameraSize.y / spriteSize.y;
                }

                transform.localScale *= scale;
            }
        }
    }
}