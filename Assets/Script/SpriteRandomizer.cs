using UnityEngine;
using Playniax.Ignition.Framework;

namespace SpaceShooterArtPack02
{
    public class SpriteRandomizer : MonoBehaviour
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

            if (randomize == false) return;

            var spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer == null) return;

            int i = Random.Range(0, sprites.Length);
            spriteRenderer.sprite = sprites[i];

            var s = _RandomFloat(scale, 1);
            transform.localScale *= s;

            var p = Vector3.zero;

            p.x = Random.Range(-additionalSettings.screenBounds.extents.x, additionalSettings.screenBounds.extents.x);
            p.y = Random.Range(-additionalSettings.screenBounds.extents.y, additionalSettings.screenBounds.extents.y);

            transform.position = p;
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