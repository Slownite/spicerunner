using UnityEngine;

namespace Playniax.Ignition.Prototyping
{
    public class TextureHelpers
    {
        public static Sprite TextureToSprite(Texture2D texture, float pixelsPerUnit = 100.0f)
        {
            return Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f), pixelsPerUnit);
        }

        public static Texture2D MergeTextures(Texture2D background, Texture2D layer1)
        {
            int startX = 0;
            int startY = 0;

            startX = background.width - layer1.width;
            startX = (int)(startX * .5f);
            startY = background.height - layer1.height;
            startY = (int)(startY * .5f);

            Texture2D newTex = new Texture2D(background.width, background.height, background.format, false);

            for (int x = 0; x < background.width; x++)
            {
                for (int y = 0; y < background.height; y++)
                {
                    if (x >= startX && y >= startY && x < layer1.width + startX && y < layer1.height + startY)
                    {
                        Color bgColor = background.GetPixel(x, y);
                        Color wmColor = layer1.GetPixel(x - startX, y - startY);

                        Color final_color = Color.Lerp(bgColor, wmColor, wmColor.a / 1.0f);

                        newTex.SetPixel(x, y, final_color);
                    }
                    else
                        newTex.SetPixel(x, y, background.GetPixel(x, y));
                }
            }

            newTex.Apply();

            return newTex;
        }
    }
}