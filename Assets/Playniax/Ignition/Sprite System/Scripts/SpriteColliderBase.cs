using System.Collections.Generic;
using UnityEngine;
using Playniax.Ignition.Framework;

namespace Playniax.Ignition.SpriteSystem
{
    [AddComponentMenu("Playniax/Ignition/SpriteSystem/SpriteColliderBase")]
    public class SpriteColliderBase : MonoBehaviour
    {
        public class Group
        {
            public string id;
            public List<SpriteColliderBase> collider = new List<SpriteColliderBase>();

            public static void Add(string id, SpriteColliderBase data)
            {
                for (int i = 0; i < _group.Count; i++)
                {
                    if (_group[i].id == id)
                    {
                        _group[i].collider.Add(data);

                        return;
                    }
                }

                var collisionList = new Group();
                collisionList.id = id;
                _group.Add(collisionList);
                Add(id, data);
            }

            public static void Remove(string id, SpriteColliderBase data)
            {
                for (int i = 0; i < _group.Count; i++)
                {
                    if (_group[i].id == id)
                    {
                        _group[i].collider.Remove(data);
                    }
                }
            }

            public static Group Get(string id)
            {
                for (int i = 0; i < _group.Count; i++)
                {
                    if (_group[i].id == id)
                    {
                        return _group[i];
                    }
                }
                return null;
            }

            static List<Group> _group = new List<Group>();
        }

        public static List<SpriteColliderBase> clearList = new List<SpriteColliderBase>();
        public static List<SpriteColliderBase> resetList = new List<SpriteColliderBase>();

        public Collider2D spriteCollider;

        [Tooltip("The collision group the object belongs to is used by the CollisionMonitor script.")]
        public string group = "Enemy";

        [Tooltip("Whether collision detection is enabled or not.")]
        public bool collisionEnabled = true;

        public bool isMarkedForDeath { get; set; }
        public bool isCollisionSuspended { get; set; }

        public static void Check(string a, string b)
        {
            var group1 = Group.Get(a);
            var group2 = Group.Get(b);

            if (group1 == null) return;
            if (group2 == null) return;

            for (int i1 = 0; i1 < group1.collider.Count; i1++)
            {
                if (group1.collider[i1].spriteCollider == null) continue;

                for (int i2 = 0; i2 < group2.collider.Count; i2++)
                {
                    if (group2.collider[i2].spriteCollider == null) continue;

                    if (group1.collider[i1] == group2.collider[i2]) continue;
                    if (group1.collider[i1].spriteCollider == group2.collider[i2].spriteCollider) continue;

                    if (group1.collider[i1].spriteCollider.Distance(group2.collider[i2].spriteCollider).isOverlapped) _Collision(group1.collider[i1], group2.collider[i2]);
                }
            }
        }

        public static void ClearList()
        {
            for (int i = 0; i < clearList.Count; i++)
            {
                if (clearList[i].name.Contains(GameObjectPooler.marker))
                {
                    clearList[i].gameObject.SetActive(false);

                    clearList[i].isMarkedForDeath = false;
                }
                else
                {
                    Destroy(clearList[i].gameObject);
                }
            }

            clearList.Clear();
        }

        public static void ResetList()
        {
            var frameCount = Time.renderedFrameCount;

            if (_frameCount == frameCount) return;

            _frameCount = frameCount;

            for (int i = 0; i < resetList.Count; i++)
            {
                resetList[i].OnReset();
            }

            resetList.Clear();
        }

        public virtual bool AllowCollision()
        {
            if (gameObject.activeInHierarchy == false) return false;
            if (isMarkedForDeath) return false;
            if (collisionEnabled == false) return false;
            if (isCollisionSuspended == true) return false;
            if (group == "") return false;

            return true;
        }

        public virtual void OnCollision(SpriteColliderBase collider)
        {
        }

        public virtual void OnDestroy()
        {
            clearList.Remove(this);
        }

        public virtual void OnDisable()
        {
            Group.Remove(group, this);
        }

        public virtual void OnEnable()
        {
            Group.Add(group, this);

            isCollisionSuspended = false;
        }

        public virtual void OnReset()
        {
        }

        public void Destroy()
        {
            if (isMarkedForDeath == true) return;

            clearList.Add(this);

            isMarkedForDeath = true;
        }

        static void _Collision(SpriteColliderBase a, SpriteColliderBase b)
        {
            if (a != null && b != null && a.gameObject && b.gameObject && a.AllowCollision() && b.AllowCollision()) a.OnCollision(b);
            //if (a != null && b != null && a.gameObject && b.gameObject && a.AllowCollision() && b.AllowCollision()) b.OnCollision(a);
        }

        static int _frameCount;
    }
}