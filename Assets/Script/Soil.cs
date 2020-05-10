using UnityEngine;
using Playniax.Ignition.Framework;

namespace SpaceShooterArtPack02
{
    public class Soil : MonoBehaviour
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

        public Sprite[] sprites;

        public bool randomize = true;

        public AdditionalSettings additionalSettings;

        void Awake()
        {
            additionalSettings.Get(gameObject);

            if (additionalSettings.spriteRenderer == null) return;

            int i = Random.Range(0, sprites.Length);
            additionalSettings.spriteRenderer.sprite = sprites[i];

            transform.position = new Vector3(transform.position.x, -additionalSettings.screenBounds.extents.y + additionalSettings.spriteRenderer.bounds.size.y / 2, transform.position.z);

            _Repeat();
        }

        void _Repeat()
        {
            float height = additionalSettings.camera.orthographicSize * 2;
            float width = height / Screen.height * Screen.width;

            int tiles = (int)(width / additionalSettings.spriteRenderer.sprite.bounds.size.x);

            var position = transform.position;

            for (int i = -tiles; i <= tiles; i++)
            {
                if (i == 0) continue;

                var obj = new GameObject(name);

                var spriteRenderer = obj.AddComponent<SpriteRenderer>();
                spriteRenderer.sprite = additionalSettings.spriteRenderer.sprite;
                spriteRenderer.transform.localScale = additionalSettings.spriteRenderer.transform.localScale;
                spriteRenderer.sortingOrder = additionalSettings.spriteRenderer.sortingOrder;

                var offset = new Vector3(position.x + spriteRenderer.bounds.size.x * i, 0);

                obj.transform.position = position + offset;

                obj.transform.SetParent(transform);
            }
        }
    }
}