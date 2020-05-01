using UnityEngine;
using Playniax.Ignition.SpriteSystem;
using Playniax.Ignition.ParticleSystem;
using Playniax.Ignition.VisualEffects;

namespace SpaceShooterArtPack02
{
    public class WeaponSystem : SpriteBoxCollider
    {
        [System.Serializable]
        public class UpgradeSettings
        {
            public int frontCannon = 500;
            public int phaser = 250;
            public int laser = 125;
        }

        public BulletSpawner frontCannon;
        public BulletSpawner phaser;
        public LaserSpawner laser;
        public GameObject shield;

        public UpgradeSettings upgradeSettings;

        public void AddShield()
        {
            if (_shield == null && shield)
            {
                _shield = Instantiate(shield);
                _shield.transform.SetParent(gameObject.transform, false);
            }
        }

        public override void OnCollision(SpriteColliderBase collider)
        {
            if (collider.name == "Pickup Shield")
            {
                AddShield();

                collider.Destroy();
            }
            else if (collider.name == "Pickup Bullets")
            {
                frontCannon.count += upgradeSettings.frontCannon;

                collider.Destroy();
            }
            else if (collider.name == "Pickup Phaser" && phaser)
            {
                phaser.count += upgradeSettings.phaser;

                collider.Destroy();
            }
            else if (collider.name == "Pickup Laser" && laser)
            {
                laser.timer.count += upgradeSettings.laser;

                collider.Destroy();
            }
            else if (collider.name == "Pickup Nuke")
            {
                Nukeable.Nuke();

                EmitterGroup.Play("Nuke", default);

                collider.Destroy();
            }
        }

        GameObject _shield;
    }
}