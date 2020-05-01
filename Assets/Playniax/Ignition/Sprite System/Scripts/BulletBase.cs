using UnityEngine;

namespace Playniax.Ignition.SpriteSystem
{
    [AddComponentMenu("Playniax/Ignition/SpriteSystem/BulletBase")]
    public class BulletBase : MonoBehaviour
    {
        public Vector3 velocity;

        public static void Aim(GameObject bullet, GameObject target, float speed, Vector3 position = default)
        {
            if (position != default) bullet.transform.position = position;

            bullet.SetActive(true);

            var bulletBase = bullet.GetComponent<BulletBase>();

            if (bulletBase == null) bulletBase = bullet.AddComponent(typeof(BulletBase)) as BulletBase;

            var angle = Math2DHelpers.GetAngle(target, bullet);
            bulletBase.transform.rotation = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg);
            bulletBase.velocity = new Vector3(Mathf.Cos(angle) * speed, Mathf.Sin(angle) * speed);
        }

        void Update()
        {
            transform.position += velocity * Time.deltaTime;
        }
    }
}