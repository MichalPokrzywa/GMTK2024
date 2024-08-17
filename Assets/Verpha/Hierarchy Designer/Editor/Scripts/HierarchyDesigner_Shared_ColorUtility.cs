#if UNITY_EDITOR
using UnityEngine;
using System;
using UnityEditor;

namespace Verpha.HierarchyDesigner
{
    public static class HierarchyDesigner_Shared_ColorUtility
    {
        public static readonly Color LightModeColor = HexToColor("#C8C8C8");
        public static readonly Color DarkModeColor = HexToColor("#383838");
        public static readonly Color LightModeHighlightColor = HexToColor("#B5B5B5");
        public static readonly Color DarkModeHighlightColor = HexToColor("#464646");
        public static readonly Color SelectedLightModeColor = HexToColor("#3372B7");
        public static readonly Color SelectedDarkModeColor = HexToColor("#2D5C8E");
        public static readonly Color BackgroundColor_DarkTheme = HexToColor("#282828");
        public static readonly Color BackgroundColor_LightTheme = HexToColor("#D7D7D7");

        public static Color GetEditorBackgroundColor()
        {
            return EditorGUIUtility.isProSkin ? DarkModeColor : LightModeColor;
        }

        public static Color GetBackgroundColor(bool isHovering, int instanceID)
        {
            if (IsSelected(instanceID))
            {
                return EditorGUIUtility.isProSkin ? SelectedDarkModeColor : SelectedLightModeColor;
            }
            else if (isHovering)
            {
                return EditorGUIUtility.isProSkin ? DarkModeHighlightColor : LightModeHighlightColor;
            }
            else
            {
                return EditorGUIUtility.isProSkin ? DarkModeColor : LightModeColor;
            }
        }

        private static bool IsSelected(int instanceID)
        {
            return Array.IndexOf(Selection.instanceIDs, instanceID) >= 0;
        }

        public static string ColorToString(Color color)
        {
            return $"{color.r}:{color.g}:{color.b}:{color.a}";
        }

        public static Color ParseColor(string colorString)
        {
            string[] colorParts = colorString.Split(':');
            float[] colorValues = new float[4];

            for (int i = 0; i < colorParts.Length; i++)
            {
                if (!float.TryParse(colorParts[i], out colorValues[i]))
                {
                    return Color.white;
                }
            }

            return new Color(colorValues[0], colorValues[1], colorValues[2], colorValues.Length > 3 ? colorValues[3] : 1.0f);
        }

        public static Color HexToColor(string hex)
        {
            try
            {
                hex = hex.Replace("0x", "").Replace("#", "");
                byte a = 255;
                byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
                byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);

                if (hex.Length == 8)
                {
                    a = byte.Parse(hex.Substring(6, 2), System.Globalization.NumberStyles.HexNumber);
                }

                return new Color32(r, g, b, a);
            }
            catch (Exception ex)
            {
                Debug.LogError("Color parsing failed: " + ex.Message);
                return Color.white;
            }
        }
    }
}
#endif