using UnityEngine;
using Playniax.Ignition.Framework;

namespace SpaceShooterArtPack02
{
    public class Crusher : MonoBehaviour
    {
        [System.Serializable]
        public class AdditionalSettings
        {
            public Camera camera;
            public Bounds screenBounds;
            public Vector2 size;

            public void Get(GameObject obj)
            {
                if (camera == null) camera = Camera.main;
                if (camera) screenBounds = CameraHelpers.OrthographicBounds(camera);
            }
        }

        public float timer = 3;
        public string interval = "2";

        public AdditionalSettings additionalSettings;

        void Awake()
        {
            additionalSettings.Get(gameObject);

            var position = transform.position;
            position.y = -additionalSettings.screenBounds.extents.y - additionalSettings.size.y / 2;
            transform.position = position;

            _start = transform.position;
        }

        void Update()
        {
            if (_state == 0)
            {
                timer -= 1 * Time.deltaTime;
                if (timer < 0) _state += 1;
            }
            else if (_state == 1)
            {
                transform.position += 2 * Vector3.up * Time.deltaTime;

                if (transform.position.y > _start.y + additionalSettings.size.y) _state++;
            }
            else if (_state == 2)
            {
                transform.position += 15 * Vector3.down * Time.deltaTime;

                if (transform.position.y < _start.y)
                {
                    transform.position = _start;
                    timer = _RandomFloat(interval, 1);
                    _state = 0;
                }
            }
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

        Vector3 _start;
        int _state;
    }
}