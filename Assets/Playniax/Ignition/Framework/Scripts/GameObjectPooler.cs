using System.Collections.Generic;
using UnityEngine;

namespace Playniax.Ignition.Framework
{
    [System.Serializable]
    public class GameObjectPooler
    {
        public const string marker = "(Pooled Game Object)";

        public GameObject prefab;
        public int initialize;

        public GameObject GetAvailableObject(bool allowGrowth = true)
        {
            if (prefab == null) return null;

            if (initialize > 0 && _pool.Count == 0) Init();

            for (int i = 0; i < _pool.Count; i++)
            {
                if (_pool[i] && _pool[i].activeInHierarchy == false)
                {
                    var o = _pool[i];
                    o.SetActive(false);
                    return o;
                }
            }

            if (allowGrowth == false) return null;

            var n = Object.Instantiate(prefab);
            n.SetActive(false);
            n.name = n.name.Replace("(Clone)", marker);
            _pool.Add(n);
            return n;
        }

        public void Init()
        {
            if (prefab == null) return;

            if (prefab.scene.rootCount > 0) prefab.SetActive(false);

            for (int i = 0; i < initialize; i++)
            {
                var obj = Object.Instantiate(prefab);
                obj.SetActive(false);
                obj.name = obj.name.Replace("(Clone)", marker);
                _pool.Add(obj);
            }
        }

        List<GameObject> _pool = new List<GameObject>();
    }
}