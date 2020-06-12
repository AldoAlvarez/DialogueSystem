using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AGAC.General
{
    public static class GeneralMethods
    {
        public static Color GetGray(float rgbValue)
        {
            return new Color(rgbValue, rgbValue, rgbValue, 1);
        }
        public static Texture2D GetNewTexture(Color color)
        {
            Texture2D texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, color);
            texture.Apply();
            return texture;
        }
        public static Texture2D TintTexture(Texture2D texture, Color tint) 
        {
            return ModifyTexture(texture, tint);
        }
        public static Texture2D TintTexture(Texture2D texture, float multiple)
        {
            Vector4 multiplyValue = Vector4.zero;
            for (int i = 0; i < 4; ++i)
                multiplyValue[i] = multiple;
            return ModifyTexture(texture, multiplyValue);
        }

        private static Texture2D ModifyTexture(Texture2D texture, Vector4 multiplyValue) 
        {
            int width = texture.width;
            int height = texture.height;
            Texture2D tinted = new Texture2D(width, height);
            for (int x = 0; x < width; ++x)
            {
                for (int y = 0; y < height; ++y)
                {
                    Color color = texture.GetPixel(x, y);
                    tinted.SetPixel(x, y, color * multiplyValue);
                }
            }
            tinted.Apply();
            return tinted;
        }
        public static Texture2D ResizeTexture(Texture2D texture, int width, int height)
        {
            texture.filterMode = FilterMode.Point;
            RenderTexture rt = RenderTexture.GetTemporary(width, height);
            rt.filterMode = FilterMode.Point;
            RenderTexture.active = rt;
            Graphics.Blit(texture, rt);
            Texture2D nTex = new Texture2D(width, height);
            nTex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            nTex.Apply();
            RenderTexture.active = null;
            RenderTexture.ReleaseTemporary(rt);
            return nTex;
        }

    }
}