using UnityEngine;
using Playniax.Ignition.Framework;
using Playniax.Ignition.SpriteSystem;

namespace SpaceShooterArtPack02
{
    public class Tank : AnimationGroup
    {
        [System.Serializable]
        public class BulletProperties
        {
            public GameObject prefab;
            public float timer = 1;
            public float interval = .25f;
            public float speed = 8;

            public Color glowColor;
            public SpriteRenderer glow;

            public void Update(Vector3 position, GameObject target)
            {
                glow.transform.position = position;

                timer -= 1 * Time.deltaTime;
                if (timer > 0) return;

                timer = interval;

                var obj = AdvancedGameObjectPooler.GetAvailableObject(prefab);
                if (obj == null) return;

                var bullet = obj.GetComponent<BulletBase>();
                if (bullet == null) return;

                obj.transform.position = position;

                obj.SetActive(true);

                var angle = Mathf.Atan2(target.transform.position.y - position.y, target.transform.position.x - position.x);

                bullet.velocity = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * speed;

                glow.color = glowColor;
                glow.enabled = true;
            }
        }

        public BulletProperties bulletProperties;

        public SpriteRenderer turret;
        public SpriteRenderer wheels;

        public Bounds screenBounds;

        public int turretFrames;

        void Awake()
        {
            bulletProperties.glowColor = bulletProperties.glow.color;
            bulletProperties.glow.enabled = false;

            screenBounds = CameraHelpers.OrthographicBounds(Camera.main);

            if (Find("Turret") != null) turretFrames = Find("Turret").sprites.Length;
        }

        void Update()
        {
            if (turret == null) return;
            if (wheels == null) return;

            _UpdateDriving();
            _UpdateTurret();
            _UpdateGlow();
        }

        void _UpdateDriving()
        {
            if (_state == 0)
            {
                _speed += new Vector3(Random.Range(-4, 4), 0, 0);
                _timer = Random.Range(1, 2);
                _state++;
            }
            else if (_state == 1)
            {
                _timer -= 1 * Time.deltaTime;
                if (_timer < 0) _state = 0;
            }

            transform.position += _speed * Time.deltaTime;

            _speed *= 1 / (1 + (Time.deltaTime * .99f));

            //            if (Mathf.Abs(transform.position.x + turret.bounds.size.x / 2) > screenBounds.extents.x) transform.position = new Vector3(-transform.position.x, transform.position.y, transform.position.z);
            if (transform.position.x < -screenBounds.extents.x - turret.bounds.size.x / 2)
            {
                transform.position = new Vector3(screenBounds.extents.x + turret.bounds.size.x / 2, transform.position.y, transform.position.z);

            }
            else if (transform.position.x > screenBounds.extents.x + turret.bounds.size.x / 2)
            {
                transform.position = new Vector3(-screenBounds.extents.x - turret.bounds.size.x / 2, transform.position.y, transform.position.z);

            }

            //if (Mathf.Abs(_speed.x) >= 1) EmitterGroup.Play("Smoke", transform.position + Vector3.down * .5f, 1, -2);

            Loop("Wheels", wheels, -_speed.x);
        }

        void _UpdateTurret()
        {
            if (PlayersGroup.selectedPlayer == null) return;

            var angle = Mathf.Atan2(PlayersGroup.selectedPlayer.transform.position.y - transform.position.y, PlayersGroup.selectedPlayer.transform.position.x - transform.position.x) * Mathf.Rad2Deg;

            if (angle < 0) angle += 360;

            if (angle < 0) return;
            if (angle > 179) return;

            float frame = angle / 5;

            if (frame < 0) frame = 0;
            if (frame > turretFrames - 1) frame = turretFrames - 1;

            turret.sprite = GetSprite("Turret", Mathf.RoundToInt(frame));

            bulletProperties.Update(transform.position + _bulletPositions[Mathf.RoundToInt(frame)] / 100f, PlayersGroup.selectedPlayer);
        }

        void _UpdateGlow()
        {
            if (bulletProperties.glow.enabled == false) return;

            bulletProperties.glow.color -= new Color(0, 0, 0, 05f) * Time.deltaTime;

            if (bulletProperties.glow.color.a <= 0) bulletProperties.glow.enabled = false;
        }

        int _state;
        Vector3 _speed;
        float _timer;

        Vector3[] _bulletPositions = new[]
        {
            new Vector3(44, -2),
            new Vector3(43, 0),
            new Vector3(41, 3),
            new Vector3(39, 6),
            new Vector3(36, 8),
            new Vector3(33, 10),
            new Vector3(30, 12),
            new Vector3(27, 13),
            new Vector3(24, 14),
            new Vector3(21, 15),
            new Vector3(18, 16),
            new Vector3(15, 17),
            new Vector3(13, 18),
            new Vector3(10, 19),
            new Vector3(7, 20),
            new Vector3(5, 21),
            new Vector3(2, 22),
            new Vector3(0, 23),
            new Vector3(-3, 21),
            new Vector3(-5, 20),
            new Vector3(-8, 18),
            new Vector3(-10, 17),
            new Vector3(-13, 17),
            new Vector3(-15, 16),
            new Vector3(-18, 15),
            new Vector3(-20, 14),
            new Vector3(-23, 13),
            new Vector3(-26, 12),
            new Vector3(-29, 11),
            new Vector3(-32, 10),
            new Vector3(-35, 8),
            new Vector3(-38, 6),
            new Vector3(-41, 3),
            new Vector3(-43, 0),
            new Vector3(-44, -2),
            new Vector3(-45, -5)
        };
    }
}