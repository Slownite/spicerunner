using System.Collections.Generic;
using UnityEngine;

namespace Playniax.Ignition.SpriteSystem
{
    [AddComponentMenu("Playniax/Ignition/SpriteSystem/Nukeable")]
    public class Nukeable : MonoBehaviour
    {
        public static List<CollisionData> list = new List<CollisionData>();

        public static void Nuke()
        {
            if (list.Count == 0) return;

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] && list[i].indestructible == false) list[i].Destroy();
            }
        }

        void OnEnable()
        {
            var scoreSystem = GetComponent<CollisionData>();
            if (scoreSystem) list.Add(scoreSystem);
        }

        void OnDisable()
        {
            var scoreSystem = GetComponent<CollisionData>();
            if (scoreSystem) list.Remove(scoreSystem);
        }
    }
}