using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Playniax.Ignition.SpriteSystem
{
    [AddComponentMenu("Playniax/Ignition/SpriteSystem/Targetable")]
    public class Targetable : MonoBehaviour
    {
        public static List<CollisionData> list = new List<CollisionData>();

        public static GameObject GetClosest(GameObject gameObject, float targetRange = 0)
        {
            if (gameObject == null) return null;

            if (gameObject.activeInHierarchy == false) return null;

            if (list.Count == 0) return null;

            GameObject target = null;

            for (int i = 0; i < list.Count; i++)
            {
                if (target == null && list[i].isMarkedForDeath == false && _InRange(gameObject, list[i].gameObject, targetRange) == true && list[i].gameObject.activeInHierarchy == true && list[i].spriteRenderer != null && list[i].spriteRenderer.isVisible == true)
                {
                    target = list[i].gameObject;

                    continue;
                }

                if (target != null && list[i] != target && list[i].isMarkedForDeath == false && _InRange(gameObject, list[i].gameObject, targetRange) == true && list[i].gameObject.activeInHierarchy == true && list[i].spriteRenderer != null && list[i].spriteRenderer.isVisible == true && Vector3.Distance(gameObject.transform.position, list[i].gameObject.transform.position) < Vector3.Distance(gameObject.transform.position, target.gameObject.transform.position)) target = list[i].gameObject;
            }

            return target;
        }

        public static CollisionData GetClosest(CollisionData scoreSystem, bool toughestFirst = false, float targetRange = 0)
        {
            if (scoreSystem == null) return null;

            if (scoreSystem.gameObject.activeInHierarchy == false) return null;

            if (list.Count == 0) return null;

            CollisionData target = null;

            for (int i = 0; i < list.Count; i++)
            {
                if (target == null && list[i].isMarkedForDeath == false && _InRange(scoreSystem.gameObject, list[i].gameObject, targetRange) == true && list[i].gameObject.activeInHierarchy == true && list[i].spriteRenderer != null && list[i].spriteRenderer.isVisible == true)
                {
                    target = list[i];

                    continue;
                }

                if (toughestFirst == true)
                {
                    if (target != null && list[i].structuralIntegrity > target.structuralIntegrity && list[i] != target && list[i].isMarkedForDeath == false && _InRange(scoreSystem.gameObject, list[i].gameObject, targetRange) == true && list[i].gameObject.activeInHierarchy == true && list[i].spriteRenderer != null && list[i].spriteRenderer.isVisible == true && Vector3.Distance(scoreSystem.transform.position, list[i].gameObject.transform.position) < Vector3.Distance(scoreSystem.transform.position, target.gameObject.transform.position)) target = list[i];
                }
                else
                {
                    if (target != null && list[i] != target && list[i].isMarkedForDeath == false && _InRange(scoreSystem.gameObject, list[i].gameObject, targetRange) == true && list[i].gameObject.activeInHierarchy == true && list[i].spriteRenderer != null && list[i].spriteRenderer.isVisible == true && Vector3.Distance(scoreSystem.transform.position, list[i].gameObject.transform.position) < Vector3.Distance(scoreSystem.transform.position, target.gameObject.transform.position)) target = list[i];
                }
            }

            return target;
        }

        static bool _InRange(GameObject a, GameObject b, float range)
        {
            if (range == 0) return true;

            var distance = Vector3.Distance(a.transform.position, b.transform.position);
            if (distance > range) return false;

            return true;
        }

        /*
        static bool _InRange(CollisionData a, CollisionData b, float range)
        {
            if (range == 0) return true;

            var distance = Vector3.Distance(a.transform.position, b.transform.position);
            if (distance > range) return false;

            return true;
        }
        */
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