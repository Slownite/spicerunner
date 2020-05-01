using UnityEngine;
using Playniax.Ignition.Framework;
using Playniax.Ignition.SpriteSystem;

namespace Playniax.Ignition.VisualEffects
{
    public class RotatingLaser : MonoBehaviour
    {
        public enum TargetType { Player, Enemy };

        public TargetType targetType = TargetType.Player;

        public float speed = 25;
        public SpriteRenderer beam;

        void LateUpdate()
        {
            if (beam == null) return;
            if (beam.enabled == false) return;

            transform.Rotate(Vector3.back * Time.deltaTime * speed);

            if (targetType == TargetType.Player)
            {
                _TargetPlayer();
            }
            else if (targetType == TargetType.Enemy)
            {
                _TargetEnemy();
            }
        }

        bool _CircleLine(float cx, float cy, float radius, float x1, float y1, float x2, float y2)
        {
            var dx = x2 - x1;
            var dy = y2 - y1;
            var ld = Mathf.Sqrt((dx * dx) + (dy * dy));
            var lux = dx / ld;
            var luy = dy / ld;
            //			var lnx = luy;
            //			var lny = -lux;
            var dx1 = cx - (x1 - lux * radius);
            var dy1 = cy - (y1 - luy * radius);

            var d = Mathf.Sqrt((dx1 * dx1) + (dy1 * dy1));
            dx1 = dx1 / d;
            dy1 = dy1 / d;

            var dx2 = cx - (x2 + lux * radius);
            var dy2 = cy - (y2 + luy * radius);

            d = Mathf.Sqrt((dx2 * dx2) + (dy2 * dy2));
            dx2 = dx2 / d;
            dy2 = dy2 / d;

            var dot1 = (dx1 * lux) + (dy1 * luy);
            var dot2 = (dx2 * lux) + (dy2 * luy);
            var px = x1 - cx;
            var py = y1 - cy;

            var distsq = (Mathf.Abs((dx * py - px * dy) / ld));

            return ((dot1 >= 0 && dot2 <= 0) || (dot1 <= 0 && dot2 >= 0)) && (distsq <= radius);
        }

        Vector3 _GetLaserFocusPoint()
        {
            var angle = beam.transform.rotation.eulerAngles.z * Mathf.Deg2Rad;
            var length = beam.bounds.size.x;

            var x = beam.transform.position.x + Mathf.Cos(angle) * length;
            var y = beam.transform.position.y + Mathf.Sin(angle) * length;

            return new Vector3(x, y);
        }

        void _TargetEnemy()
        {
            /*
            for (int i = 0; i < CollisionData.collisionList.Count; i++)
            {
                if (CollisionData.collisionList[i] && CollisionData.collisionList[i].gameObject.activeInHierarchy)
                {
                    var laserFocusPoint = _GetLaserFocusPoint();

                    if (_CircleLine(CollisionData.collisionList[i].transform.position.x, CollisionData.collisionList[i].transform.position.y, .15f, transform.position.x, transform.position.y, laserFocusPoint.x, laserFocusPoint.y))
                    {
                        var collisionData = CollisionData.collisionList[i].GetComponent<CollisionData>();
                        if (collisionData) collisionData.OnStructuralFailure();
                    }
                }
            }
            */
        }

        void _TargetPlayer()
        {
            for (int i = 0; i < PlayersGroup.list.Count; i++)
            {
                if (PlayersGroup.list[i] && PlayersGroup.list[i].gameObject.activeInHierarchy)
                {
                    var laserFocusPoint = _GetLaserFocusPoint();

                    if (_CircleLine(PlayersGroup.list[i].transform.position.x, PlayersGroup.list[i].transform.position.y, .15f, transform.position.x, transform.position.y, laserFocusPoint.x, laserFocusPoint.y))
                    {
                        var collisionData = PlayersGroup.list[i].GetComponent<CollisionData>();
                        if (collisionData && collisionData.isMarkedForDeath == false && collisionData.indestructible == false) collisionData.OnStructuralFailure(null);
                    }
                }
            }
        }
    }
}