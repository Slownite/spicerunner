using UnityEngine;
using Playniax.Ignition.Framework;

namespace SpaceShooterArtPack02
{
    public class Rock : MonoBehaviour
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
        public string scale = "1";

        public AdditionalSettings additionalSettings;

        void Awake()
        {
            additionalSettings.Get(gameObject);

            if (additionalSettings.spriteRenderer == null) return;

            int i = Random.Range(0, sprites.Length);
            additionalSettings.spriteRenderer.sprite = sprites[i];

            var s = _RandomFloat(scale, 1);
            transform.localScale *= s;

            var position = Vector3.zero;

            position.x = Random.Range(-additionalSettings.screenBounds.extents.x, additionalSettings.screenBounds.extents.x);
            position.y = -additionalSettings.screenBounds.extents.y + additionalSettings.spriteRenderer.bounds.size.y / 2;

            transform.position = position;

        }

        float _RandomFloat(string str, float defaultValue = 0)
        {
            if (str.Trim() == "") return defaultValue;
            string[] r = str.Split(',');
            if (r.Length == 1) return float.Parse(str, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
            float min = float.Parse(r[0], System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
            float max = float.Parse(r[1], System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture);
            return Random.Range(min, max);
        }
    }
}