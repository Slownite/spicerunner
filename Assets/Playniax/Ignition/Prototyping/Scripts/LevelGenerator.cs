using UnityEngine;

namespace Playniax.Ignition.Prototyping
{
    public class LevelGenerator : LevelGeneratorBase
    {
        public GameObject prefab;
        public int count = 10;

        public override void Awake()
        {
            if (prefab == null) return;
            if (prefab && prefab.scene.rootCount > 0) prefab.SetActive(false);

            base.Awake();

            for (int i = 0; i < count; i++)
            {
                var clone = Instantiate(prefab);

                Position(clone);
            }
        }
    }
}