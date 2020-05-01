using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Playniax.Ignition.Framework;

namespace Playniax.Ignition.SpriteSystem
{
    [AddComponentMenu("Playniax/Ignition/SpriteSystem/SpriteSpawnerBase")]
    public class SpriteSpawnerBase : SpawnerBase
    {
        [System.Serializable]
        public class SpawnerSettings
        {
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

            public string name;
            public GameObject prefab;
            public float delay = 1;
            public float interval;
            public int count = 1;
            public bool trackProgress = true;

            public StartPosition startPosition = StartPosition.Random;

            public LayerSettings layerSettings;

            public void Init()
            {
                _count = count;

                if (prefab && name == "") name = prefab.name;
            }

            public void Reset()
            {
                count = _count;
            }

            int _count;
        }

        [System.Serializable]
        public class LoopSettings
        {
            public float interval = 1;
            public int count = 3;
            public int index;
        }

        [System.Serializable]
        public class AdditionalSettings
        {
            public Camera camera;
            public Bounds screenBounds;

            public void Get(GameObject obj)
            {
                if (camera == null) camera = Camera.main;
                if (camera) screenBounds = CameraHelpers.OrthographicBounds(camera);
            }
        }

        public enum StartPosition { Left, Right, Top, Bottom, LeftOrRight, TopOrBottom, Random, Fixed };

        public SpawnerSettings[] spawnerSettings = new SpawnerSettings[1];

        public LoopSettings loopSettings;

        public float timer;

        public int counter;

        public AdditionalSettings additionalSettings;

        public override void Awake()
        {
            base.Awake();

            additionalSettings.Get(gameObject);

            for (int i = 0; i < spawnerSettings.Length; i++)
            {
                if (spawnerSettings[i].prefab) spawnerSettings[i].prefab.SetActive(false);
            }

            for (int i = 0; i < spawnerSettings.Length; i++)
            {
                spawnerSettings[i].Init();
            }

            _GetTotal();
        }

        public virtual GameObject OnSpawn()
        {
            var obj = AdvancedGameObjectPooler.GetAvailableObject(spawnerSettings[loopSettings.index].prefab);
            if (obj)
            {
                if (spawnerSettings[loopSettings.index].trackProgress)
                {
                    var spawnerProgress = obj.GetComponent<ProgressCounter>();
                    if (spawnerProgress == null) spawnerProgress = obj.AddComponent<ProgressCounter>();
                    //spawnerProgress.hideFlags= HideFlags.HideInInspector;
                }

                SetPosition(obj);

                obj.SetActive(true);

                spawnerSettings[loopSettings.index].layerSettings.OrderInLayer(obj);

                counter += 1;
            }

            return obj;
        }

        public virtual void SetPosition(GameObject obj)
        {
            if (spawnerSettings[loopSettings.index].startPosition == StartPosition.Left || spawnerSettings[loopSettings.index].startPosition == StartPosition.Right || spawnerSettings[loopSettings.index].startPosition == StartPosition.Top || spawnerSettings[loopSettings.index].startPosition == StartPosition.Bottom)
            {
                _SetPosition(obj, (int)spawnerSettings[loopSettings.index].startPosition);
            }
            else if (spawnerSettings[loopSettings.index].startPosition == StartPosition.LeftOrRight)
            {
                _SetPosition(obj, Random.Range(0, 2));
            }
            else if (spawnerSettings[loopSettings.index].startPosition == StartPosition.TopOrBottom)
            {
                _SetPosition(obj, Random.Range(2, 4));
            }
            else if (spawnerSettings[loopSettings.index].startPosition == StartPosition.Random)
            {
                _SetPosition(obj, Random.Range(0, 4));
            }
            else if (spawnerSettings[loopSettings.index].startPosition == StartPosition.Fixed)
            {
                obj.transform.position = transform.position;
            }
        }

        public virtual void UpdateSpawner()
        {
            if (loopSettings.index < 0) return;
            if (loopSettings.index >= spawnerSettings.Length) return;

            if (spawnerSettings[loopSettings.index].prefab == null) return;
            if (spawnerSettings[loopSettings.index].count == 0) return;

            if (loopSettings.count == 0) return;

            timer -= 1 * Time.deltaTime;

            if (_index == loopSettings.index && timer <= 0)
            {
                var obj = OnSpawn();
                if (obj)
                {
                    spawnerSettings[loopSettings.index].count -= 1;
                    if (spawnerSettings[loopSettings.index].count <= 0)
                    {
                        loopSettings.index += 1;
                        if (loopSettings.index >= spawnerSettings.Length)
                        {
                            loopSettings.count -= 1;
                            if (loopSettings.count <= 0)
                            {
                                enabled = false;
                            }
                            else
                            {
                                _index = -1;

                                loopSettings.index = 0;

                                for (int i = 0; i < spawnerSettings.Length; i++)
                                {
                                    spawnerSettings[i].Reset();
                                }

                                timer = loopSettings.interval;

                                UpdateSpawner();
                            }
                        }
                    }
                    else
                    {
                        timer = spawnerSettings[loopSettings.index].interval;

                        UpdateSpawner();
                    }
                }
            }
            else if (timer <= 0)
            {
                _index = loopSettings.index;

                timer = spawnerSettings[loopSettings.index].delay;

                UpdateSpawner();
            }

        }

        void Update()
        {
            UpdateSpawner();
        }

        void _GetTotal()
        {
            int counter = 0;

            for (int i = 0; i < spawnerSettings.Length; i++)
            {
                if (spawnerSettings[i].prefab)
                {
                    if (spawnerSettings[i].trackProgress) counter += 1 * spawnerSettings[i].count * loopSettings.count;
                }
            }

            PlayerData.enemyCount += counter;
            PlayerData.progress += counter;
        }

        void _SetPosition(GameObject obj, int segment)
        {
            var size = Vector2.zero;
            var position = Vector3.zero;
            var spriteRenderer = obj.GetComponent<SpriteRenderer>();
            if (spriteRenderer) size = spriteRenderer.bounds.size;

            if (segment == 0)
            {
                position.x = -additionalSettings.screenBounds.extents.x - size.x / 2;
                position.y = Random.Range(-additionalSettings.screenBounds.extents.y + size.y / 2, additionalSettings.screenBounds.extents.y - size.y / 2);
            }
            else if (segment == 1)
            {
                position.x = additionalSettings.screenBounds.extents.x + size.x / 2;
                position.y = Random.Range(-additionalSettings.screenBounds.extents.y + size.y / 2, additionalSettings.screenBounds.extents.y - size.y / 2);
            }
            else if (segment == 2)
            {
                position.x = Random.Range(-additionalSettings.screenBounds.extents.x + size.x / 2, additionalSettings.screenBounds.extents.x - size.x / 2);
                position.y = additionalSettings.screenBounds.extents.y + size.y / 2;
            }
            else if (segment == 3)
            {
                position.x = Random.Range(-additionalSettings.screenBounds.extents.x + size.x / 2, additionalSettings.screenBounds.extents.x - size.x / 2);
                position.y = -additionalSettings.screenBounds.extents.y - size.y / 2;
            }

            position.x += additionalSettings.camera.transform.position.x;
            position.y += additionalSettings.camera.transform.position.y;

            obj.transform.position = position;
        }

        int _index = -1;
    }
}