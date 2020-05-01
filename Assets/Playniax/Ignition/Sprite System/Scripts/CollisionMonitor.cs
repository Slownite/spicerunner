using UnityEngine;

namespace Playniax.Ignition.SpriteSystem
{
    [AddComponentMenu("Playniax/Ignition/SpriteSystem/CollisionMonitor")]
    public class CollisionMonitor : MonoBehaviour
    {
        public string group1 = "Player";
        public string group2 = "Enemy";

        void Awake()
        {
        }

        void Update()
        {
            SpriteColliderBase.ResetList();

            SpriteColliderBase.Check(group1, group2);

            SpriteColliderBase.ClearList();
        }
    }
}