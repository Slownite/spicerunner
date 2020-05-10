using UnityEngine;
using Playniax.Ignition.Framework;

namespace Playniax.Ignition.SpriteSystem
{
    [AddComponentMenu("Playniax/Ignition/SpriteSystem/EnemyAI")]
    public class EnemyAI : MonoBehaviour
    {
        [System.Serializable]
        public class CruiserSettings
        {
            public int minSpeed = 2;
            public int maxSpeed = 4;
            public float minReflex = 1;
            public float maxReflex = 3;
            public float timer;

            public void Update(GameObject cruiser, GameObject target)
            {
                if (cruiser == null) return;

                if (timer > 0)
                {
                    timer -= 1 * Time.deltaTime;
                }
                else
                {
                    timer = Random.Range(minReflex, maxReflex);

                    float angle = 0;

                    if (target)
                    {
                        angle = Math2DHelpers.GetAngle(target, cruiser);
                    }
                    else
                    {
                        angle = Random.Range(0, 359) * Mathf.Deg2Rad;
                    }

                    _speed += Random.Range(minSpeed, maxSpeed) * new Vector3(Mathf.Cos(angle), Mathf.Sin(angle));
                }

                cruiser.transform.position += _speed * Time.deltaTime;

                _speed.x *= .99f;
                _speed.y *= .99f;
            }

            Vector3 _speed;
        }

        [System.Serializable]
        public class HomingMissileSettings
        {
            public float intro = 0;
            public float speed = 8;
            public float rotationSpeed = 250f;

            public void Update(GameObject missile, GameObject target)
            {
                if (missile == null) return;

                if (intro > 0)
                {
                    intro -= 1 * Time.deltaTime;
                    if (intro < 0) intro = 0;
                }
                else
                {
                    if (target) _direction = target.transform.position - missile.transform.position;

                    float angle = Mathf.Atan2(_direction.y, _direction.x) * Mathf.Rad2Deg;

                    var targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);

                    missile.transform.rotation = Quaternion.RotateTowards(missile.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                }

                missile.transform.position += missile.transform.right * speed * Time.deltaTime;
            }

            Vector3 _direction;
        }

        [System.Serializable]
        public class TailerSettings
        {
            public float speed = 1;

            public void Update(GameObject tailer, GameObject target)
            {
                if (tailer == null) return;

                tailer.transform.position = Vector3.MoveTowards(tailer.transform.position, target.transform.position, speed * Time.deltaTime);
            }

            Vector3 _speed;
        }

        [System.Serializable]
        public class MagnetSettings
        {
            public int globalPolarity = 1;

            public float range = 5;
            public int polarity = 1;
            public float speed = 10;
            public float maxSpeed = 160;
            public float friction = 1;

            public void Update(GameObject magnet, GameObject target)
            {
                if (magnet == null) return;
                if (target == null) return;

                var angle = Math2DHelpers.GetAngle(target, magnet);
                int distance = (int)(Mathf.Abs(target.transform.position.x - magnet.transform.position.x) + Mathf.Abs(target.transform.position.y - magnet.transform.position.y));

                if (distance > 0)
                {
                    _speed.x += Mathf.Cos(angle) * (int)(range / distance) * speed * Time.deltaTime;
                    _speed.y += Mathf.Sin(angle) * (int)(range / distance) * speed * Time.deltaTime;
                }

                if (_speed.x > maxSpeed) _speed.x = maxSpeed;
                if (_speed.x < -maxSpeed) _speed.x = -maxSpeed;
                if (_speed.y > maxSpeed) _speed.y = maxSpeed;
                if (_speed.y < -maxSpeed) _speed.y = -maxSpeed;

                magnet.transform.position += globalPolarity * polarity * _speed * Time.deltaTime;

                if (friction != 0) _speed *= 1 / (1 + (Time.deltaTime * friction));
            }

            Vector3 _speed;
        }

        public enum Mode { Cruiser, HomingMissile, Magnet, Tailer };

        public Mode mode = Mode.HomingMissile;

        public CruiserSettings cruiserSettings;
        public HomingMissileSettings homingMissileSettings;
        public MagnetSettings magnetSettings;
        public TailerSettings tailerSettings;
        [Space(8)]
        public GameObject target;

        void Update()
        {
            if (target == null || target && target.activeInHierarchy == false) target = PlayersGroup.GetAny();

            if (mode == Mode.Cruiser)
            {
                cruiserSettings.Update(gameObject, target);
            }
            else if (mode == Mode.Magnet)
            {
                magnetSettings.Update(gameObject, target);
            }
            else if (mode == Mode.HomingMissile)
            {
                homingMissileSettings.Update(gameObject, target);
            }
            else if (mode == Mode.Tailer)
            {
                tailerSettings.Update(gameObject, target);
            }
        }

        /*
        void Test1()
        {
            Vector3 direction = target.transform.position - transform.position;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            transform.position += transform.right * speed * Time.deltaTime;
        }
        */

    }
}