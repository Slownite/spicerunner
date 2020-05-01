using UnityEngine;
using Playniax.Ignition.Framework;

namespace Playniax.Ignition.SpriteSystem
{
    [AddComponentMenu("Playniax/Ignition/SpriteSystem/BulletSpawner")]
    public class BulletSpawner : BulletSpawnerBase
    {
        [System.Serializable]
        public class DirectionSettings
        {
            public bool smartSpawn;
            public Vector3 rotation;
        }

        [System.Serializable]
        public class TargetEnemySettings
        {
            public bool toughestFirst;
            public float targetRange;
        }

        [System.Serializable]
        public class LayerSettings
        {
            public enum Mode { Hard, Offset };

            public bool enabled;
            public Mode mode;
            public int value;

            public void OrderInLayer(GameObject obj)
            {
                if (obj && enabled)
                {
                    var spriteRenderer = obj.GetComponent<SpriteRenderer>();
                    if (spriteRenderer)
                    {
                        if (mode == Mode.Hard)
                        {
                            spriteRenderer.sortingOrder = value;

                        }
                        else if (mode == Mode.Offset)
                        {
                            spriteRenderer.sortingOrder += value;
                        }
                    }
                }
            }
        }

        public enum Mode { TargetPlayer, TargetEnemy, Random, Direction };

        public GameObject prefab;
        [Tooltip("Timer.")]
        public float timer;
        [Tooltip("Time between shots.")]
        public float interval = 1;
        [Tooltip("Bullet counter. -1 means unlimited.")]
        public int count = -1;
        [Tooltip("Bullet speed. To randomize use 2 values separated by a comma.")]
        public string speed = "8";
        [Tooltip("Start (offset) position.")]
        public Vector3 position;
        public Mode mode = Mode.TargetPlayer;
        public TargetEnemySettings targetEnemySettings;
        public DirectionSettings directionSettings;
        public LayerSettings layerSettings;

        public virtual void Awake()
        {
            if (prefab && prefab.scene.rootCount > 0 && prefab.activeInHierarchy) prefab.SetActive(false);
        }

        public virtual GameObject OnSpawn()
        {
            if (mode == Mode.TargetPlayer)
            {
                var target = PlayersGroup.GetAny();
                if (target)
                {
                    var obj = AdvancedGameObjectPooler.GetAvailableObject(prefab);
                    if (obj)
                    {
                        obj.transform.position = transform.position;
                        obj.transform.rotation = transform.rotation;
                        obj.transform.Translate(position, Space.Self);

                        layerSettings.OrderInLayer(obj);

                        obj.SetActive(true);

                        var bulletBase = obj.GetComponent<BulletBase>();
                        if (bulletBase)
                        {
                            var angle = Mathf.Atan2(target.transform.position.y - obj.transform.position.y, target.transform.position.x - obj.transform.position.x);

                            obj.transform.eulerAngles = new Vector3(0, 0, angle * Mathf.Rad2Deg);

                            bulletBase.velocity = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * _RandomFloat(speed);
                        }
                    }

                    return obj;
                }
            }
            else if (mode == Mode.TargetEnemy)
            {
                var target = Targetable.GetClosest(gameObject, targetEnemySettings.targetRange);
                if (target)
                {
                    var obj = AdvancedGameObjectPooler.GetAvailableObject(prefab);
                    if (obj)
                    {
                        obj.transform.position = transform.position;
                        obj.transform.rotation = transform.rotation;
                        obj.transform.Translate(position, Space.Self);

                        layerSettings.OrderInLayer(obj);

                        obj.SetActive(true);

                        var bulletBase = obj.GetComponent<BulletBase>();
                        if (bulletBase)
                        {
                            var angle = Mathf.Atan2(target.transform.position.y - obj.transform.position.y, target.transform.position.x - obj.transform.position.x);

                            obj.transform.eulerAngles = new Vector3(0, 0, angle * Mathf.Rad2Deg);

                            bulletBase.velocity = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * _RandomFloat(speed);
                        }
                    }

                    return obj;
                }
            }
            else if (mode == Mode.Random)
            {
                var obj = AdvancedGameObjectPooler.GetAvailableObject(prefab);
                if (obj)
                {
                    obj.transform.position = transform.position;
                    obj.transform.rotation = transform.rotation;
                    obj.transform.Translate(position, Space.Self);

                    layerSettings.OrderInLayer(obj);

                    obj.SetActive(true);

                    var bulletBase = obj.GetComponent<BulletBase>();
                    if (bulletBase)
                    {
                        var angle = Random.Range(0, 359) * Mathf.Deg2Rad;

                        obj.transform.eulerAngles = new Vector3(0, 0, angle * Mathf.Rad2Deg);

                        bulletBase.velocity = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * _RandomFloat(speed);
                    }
                }

                return obj;
            }
            else if (mode == Mode.Direction)
            {
                if (directionSettings.smartSpawn == true && ObjectCounter.counter > 0 || directionSettings.smartSpawn == false)
                {
                    var obj = AdvancedGameObjectPooler.GetAvailableObject(prefab);
                    if (obj)
                    {
                        obj.transform.position = transform.position;
                        obj.transform.rotation = transform.rotation * Quaternion.Euler(directionSettings.rotation);
                        obj.transform.Translate(position, Space.Self);

                        layerSettings.OrderInLayer(obj);

                        obj.SetActive(true);

                        var bulletBase = obj.GetComponent<BulletBase>();
                        if (bulletBase)
                        {
                            bulletBase.velocity = obj.transform.rotation * new Vector3(_RandomFloat(speed), 0, 0);
                        }
                    }

                    return obj;
                }
            }

            return null;
        }

        public override void UpdateSpawner()
        {
            if (prefab == null) return;
            if (count == 0) return;

            timer -= 1 * Time.deltaTime;

            if (timer <= 0)
            {
                var obj = OnSpawn();
                if (obj)
                {
                    if (count == -1)
                    {
                        timer = interval;

                        if (timer > 0) UpdateSpawner();
                    }
                    else
                    {
                        count -= 1;

                        timer = interval;

                        UpdateSpawner();
                    }
                }
                else
                {
                    timer = interval;
                    //UpdateSpawner();
                }
            }
        }
/*
        public void Fire(string id)
        {
            if (automatically == false && id == this.id) UpdateSpawner();
        }

        public void Fire(string id, char splitter)
        {
            if (automatically == true) return;

            var id2 = id.Split(splitter);

            for (int i = 0; i < id2.Length; i++)
            {
                if (id == id2[i]) UpdateSpawner();
            }
        }
*/
        static float _RandomFloat(string str, float defaultValue = 0)
        {
            if (str.Trim() == "") return defaultValue;
            string[] r = str.Split(',');
            if (r.Length == 1) return float.Parse(str);
            float min = float.Parse(r[0]);
            float max = float.Parse(r[1]);
            return Random.Range(min, max);
        }
    }
}
 