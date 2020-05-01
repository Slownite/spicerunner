using UnityEngine;

namespace Playniax.Ignition.SpriteSystem
{
    public class Math2DHelpers
    {
        public static float GetAngle(GameObject gameObject1, GameObject gameObject2, bool otherwiseRandom = true)
        {
            if (gameObject1 == null || gameObject2 == null)
            {
                if (otherwiseRandom)
                {
                    return Random.Range(0, 359) * Mathf.Deg2Rad;
                }
                else
                {
                    return 0;
                }
            }

            return Mathf.Atan2(gameObject1.transform.position.y - gameObject2.transform.position.y, gameObject1.transform.position.x - gameObject2.transform.position.x);
        }

        public static bool PointInsideRect(float pointX, float pointY, float x, float y, float width, float height, float pivotX = .5f, float pivotY = .5f)
        {
            x -= width * pivotX;
            y -= height * pivotY;

            var leftX = x;
            var rightX = x + width;
            var topY = y;
            var bottomY = y + height;

            return leftX <= pointX && pointX <= rightX && topY <= pointY && pointY <= bottomY;
        }

    }
}