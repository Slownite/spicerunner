using UnityEngine;
using Playniax.Ignition.Framework;

namespace SpaceShooterArtPack02
{
    public class Haze : MonoBehaviour
    {
        [System.Serializable]
        public class AdditionalSettings
        {
            public Camera camera;
            public Bounds screenBounds;
            public SpriteRenderer spriteRenderer;

            public void Get(GameObject obj)
            {
                if (camera == null) camera = Camera.main;
                if (camera) screenBounds = CameraHelpers.OrthographicBounds(camera);
                if (spriteRenderer == null) spriteRenderer = obj.GetComponent<SpriteRenderer>();
            }
        }

        public AdditionalSettings additionalSettings;

        void Awake()
        {
            additionalSettings.Get(gameObject);

            if (additionalSettings.spriteRenderer == null) return;

            float height = additionalSettings.camera.orthographicSize * 2;
            float width = height / Screen.height * Screen.width;

            transform.localScale = new Vector3(width / additionalSettings.spriteRenderer.sprite.bounds.size.x, transform.localScale.y, transform.localScale.z);
            transform.position = new Vector3(transform.position.x, -additionalSettings.screenBounds.extents.y + additionalSettings.spriteRenderer.bounds.size.y / 2, transform.position.z);
        }
    }
}