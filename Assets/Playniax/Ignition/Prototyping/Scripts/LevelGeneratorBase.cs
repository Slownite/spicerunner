using UnityEngine;
using System.Collections.Generic;
using Playniax.Ignition.Framework;

namespace Playniax.Ignition.Prototyping
{
    public class LevelGeneratorBase : MonoBehaviour
    {
        public static List<GameObject> objects = new List<GameObject>();

        public Vector2 bounds;
        public float border = .5f;
        public float safeZone = 1.5f;
        public float distance = .5f;
        public int failSafe = 1000;

        public bool pixelMode;
        public Vector2 resolution;

        public virtual void Awake()
        {
            if (objects.Count == 0)
            {
                var markers = FindObjectsOfType<LevelGeneratorMarker>();
                if (markers != null)
                {
                    for (int i = 0; i < markers.Length; i++)
                    {
                        objects.Add(markers[i].gameObject);
                    }
                }
            }

            if (bounds == Vector2.zero)
            {
                bounds.x = CameraHelpers.OrthographicBounds(Camera.main).extents.x;
                bounds.y = CameraHelpers.OrthographicBounds(Camera.main).extents.y;
            }

            if (pixelMode && resolution.x > 0 && resolution.y > 0)
            {
                bounds.x = resolution.x / 100 / 2;
                bounds.y = resolution.y / 100 / 2;
            }
        }

        public void Position(GameObject clone)
        {
            clone.transform.position = _GetRange();

            while (_EnoughDistance(clone) == false && _fails < failSafe)
            {
                clone.transform.position = _GetRange();

                _fails += 1;
            }

            if (_fails < failSafe)
            {
                clone.SetActive(true);

                objects.Add(clone);
            }
            else
            {
                Destroy(clone);

                return;
            }
        }

        void OnDestroy()
        {
            if (objects != null && objects.Count > 0) objects.Clear();
        }

        bool _EnoughDistance(GameObject obj)
        {
            for (int i = 0; i < objects.Count; i++)
            {
                if (Vector3.Distance(obj.transform.position, objects[i].transform.position) <= distance) return false;

                var r1 = obj.GetComponent<SpriteRenderer>();
                var r2 = objects[i].GetComponent<SpriteRenderer>();

                if (r1 && r2 && r1.bounds.Intersects(r2.bounds)) return false;
            }

            return true;
        }

        Vector3 _GetRange()
        {
            var x = Random.Range(-bounds.x + border, bounds.x - border);
            var y = Random.Range(-bounds.y + border, bounds.y - border);

            var position = new Vector3(x, y);

            while (Vector3.Distance(Vector3.zero, position) <= safeZone && _fails < failSafe)
            {
                position.x = Random.Range(-bounds.x + border, bounds.x - border);
                position.y = Random.Range(-bounds.y + border, bounds.y - border);

                _fails += 1;
            }

            return position;
        }

        int _fails;
    }
}