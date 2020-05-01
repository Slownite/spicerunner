using UnityEngine;

namespace Playniax.Ignition.Prototyping
{
    public class LevelGeneratorRandomizer : LevelGeneratorBase
    {
        public GameObject[] randomizeList;

        public override void Awake()
        {
            base.Awake();

            for (int i = 0; i < randomizeList.Length; i++)
            {
                // Improve and add Instantiate system when object is a prefab?
                if (randomizeList[i].scene.rootCount > 0 && randomizeList[i].GetComponent<LevelGeneratorMarker>() == null) Position(randomizeList[i]);
            }
        }
    }
}