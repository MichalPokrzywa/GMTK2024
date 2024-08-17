#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Verpha.HierarchyDesigner
{
    public static class HierarchyDesigner_Info_OnGUI
    {
        private static Dictionary<Color, Texture2D> textureCache = new Dictionary<Color, Texture2D>();
        private static GUIStyle cachedCustomStyle = null;

        public static GUIStyle CreateCustomStyle()
        {
            Color backgroundColor = EditorGUIUtility.isProSkin ? HierarchyDesigner_Shared_ColorUtility.BackgroundColor_DarkTheme : HierarchyDesigner_Shared_ColorUtility.BackgroundColor_LightTheme;

            if (cachedCustomStyle != null && cachedCustomStyle.normal.background != null)
            {
                Color cachedColor = cachedCustomStyle.normal.background.GetPixel(0, 0);
                if (cachedColor == backgroundColor)
                {
                    return cachedCustomStyle;
                }
            }

            cachedCustomStyle = new GUIStyle(EditorStyles.helpBox)
            {
                normal = { background = GetOrCreateTexture(2, 2, backgroundColor) }
            };

            return cachedCustomStyle;
        }

        private static Texture2D GetOrCreateTexture(int width, int height, Color color)
        {
            if (textureCache.TryGetValue(color, out Texture2D existingTexture) && existingTexture != null)
            {
                return existingTexture;
            }

            Texture2D newTexture = new Texture2D(width, height);
            Color[] pix = new Color[width * height];
            for (int i = 0; i < pix.Length; ++i)
            {
                pix[i] = color;
            }

            newTexture.SetPixels(pix);
            newTexture.Apply();

            textureCache[color] = newTexture;
            return newTexture;
        }
    }
}
#endif