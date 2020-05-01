using UnityEngine;
using Playniax.Ignition.Framework;
using Playniax.Ignition.SpriteSystem;

namespace Playniax.Ignition.VisualEffects
{
    public class Laser : MonoBehaviour
    {
        public int playerIndex = -1;

        public SpriteRenderer beam;
        public SpriteRenderer sourceGlow;
        public SpriteRenderer targetGlow;

        public static void Fire(GameObject prefab, int orderInLayer, Timer timer, CollisionData collisionData, float range = 0, float ttl = .25f, float size = 1, int damage = 1, AudioProperties audioProperties = null)
        {
            if (prefab == null) return;
            if (timer.count == 0) return;
            if (collisionData == null) return;

            var target = Targetable.GetClosest(collisionData, true, range);
            if (target == null) return;

            if (timer.Update() == false) return;

            var newLaser = Instantiate(prefab);
            //        if (newLaser == null) return;

            var laserComponent = newLaser.GetComponent<Laser>();
            if (laserComponent == null) return;

            laserComponent.Fire(orderInLayer, collisionData, target, ttl, size, damage);

            if (audioProperties != null) audioProperties.Play();
        }

        public void Fire(int orderInLayer, CollisionData source, CollisionData target, float ttl = .25f, float size = 1, int damage = 1)
        {
            if (source == null || target == null) return;

            gameObject.SetActive(true);

            beam.sortingOrder = orderInLayer;
            sourceGlow.sortingOrder = orderInLayer + 1;
            targetGlow.sortingOrder = orderInLayer + 1;

            _damage = damage;
            _size = size;
            _source = source;
            _target = target;
            _ttl = ttl;
            _ttlTimer = ttl;

            _UpdateLaser();
        }

        void Update()
        {
            _UpdateLaser();
        }

        float _GetAngle(float x1, float y1, float x2, float y2)
        {
            return Mathf.Atan2(y1 - y2, x1 - x2) * Mathf.Rad2Deg;
        }

        void _UpdateGlow()
        {
            sourceGlow.color = new Color(sourceGlow.color.r, sourceGlow.color.g, sourceGlow.color.b, beam.color.a);
            targetGlow.color = new Color(targetGlow.color.r, targetGlow.color.g, targetGlow.color.b, beam.color.a);

            sourceGlow.transform.position = _source.transform.position;
            targetGlow.transform.position = _target.transform.position;
        }

        void _UpdateLaser()
        {
            if (_source && _target)
            {
                transform.GetChild(0).transform.position = _source.transform.position;

                var angle = _GetAngle(_target.transform.position.x, _target.transform.position.y, _source.transform.position.x, _source.transform.position.y);

                beam.transform.localRotation = Quaternion.Euler(0, 0, angle);

                if (_ttlTimer > 0)
                {
                    beam.color = _targetColor - (_targetColor - _startColor) * (_ttlTimer / _ttl);

                    _UpdateGlow();

                    _ttlTimer -= 1 * Time.deltaTime;

                    beam.transform.localScale = new Vector3(Vector3.Distance(_source.transform.position, _target.transform.position) / (beam.sprite.rect.width / beam.sprite.pixelsPerUnit), _size, 1);
                }
                else
                {
                    _target.structuralIntegrity -= _damage;

                    if (_target.structuralIntegrity <= 0)
                    {
                        _target.structuralIntegrity = 0;

                        _target.OnStructuralFailure(_source);

                        if (_source.playerIndex == -1 && playerIndex >= 0) PlayerData.Get(playerIndex).scoreboard += _target.points;
                    }

                    Destroy(gameObject);
                }
            }
            else
            {
                _source = null;
                _target = null;

                Destroy(gameObject);
            }

        }
        int _damage = 1;

        float _size = 1;
        float _ttl = 1;
        float _ttlTimer = 1;

        CollisionData _source;
        CollisionData _target;

        Color _startColor = new Color(1, 1, 1, 1);
        Color _targetColor = new Color(1, 1, 1, 0);
    }
}