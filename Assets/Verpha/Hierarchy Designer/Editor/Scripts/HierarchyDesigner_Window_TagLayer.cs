#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Verpha.HierarchyDesigner
{
    public class HierarchyDesigner_Window_TagLayer : EditorWindow
    {
        private GameObject gameObject;
        private bool isTag;
        private GUIContent hierarchyLabelContent;

        public static void OpenWindow(GameObject gameObject, bool isTag, Vector2 position)
        {
            HierarchyDesigner_Window_TagLayer window = GetWindow<HierarchyDesigner_Window_TagLayer>("Hierarchy Designer Tag and Layer");
            window.minSize = new Vector2(400, 60);
            window.position = new Rect(position, new Vector2(200, 50));
            window.gameObject = gameObject;
            window.isTag = isTag;
            window.hierarchyLabelContent = new GUIContent("Hierarchy Tag and Layer");
        }

        private void OnGUI()
        {
            bool cancelLayout = false;

            var customSettingsStyle = HierarchyDesigner_Info_OnGUI.CreateCustomStyle();
            GUILayout.BeginVertical(customSettingsStyle);
            GUILayout.Space(5);

            GUILayout.Label(hierarchyLabelContent, EditorStyles.boldLabel);
            GUILayout.Space(5);
            EditorGUI.BeginChangeCheck();

            if (isTag)
            {
                string newTag = EditorGUILayout.TagField("Tag", gameObject.tag);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(gameObject, "Change Tag");
                    gameObject.tag = newTag;
                    Close();
                }
            }
            else
            {
                int newLayer = EditorGUILayout.LayerField("Layer", gameObject.layer);
                if (EditorGUI.EndChangeCheck())
                {
                    bool shouldChangeLayer = true;

                    if (gameObject.transform.childCount > 0)
                    {
                        int result = AskToChangeChildrenLayers(gameObject, newLayer);
                        if (result == 2)
                        {
                            shouldChangeLayer = false;
                            cancelLayout = true;
                        }
                    }

                    if (shouldChangeLayer)
                    {
                        Undo.RecordObject(gameObject, "Change Layer");
                        gameObject.layer = newLayer;
                        Close();
                    }
                }
            }

            GUILayout.Space(5);
            GUILayout.EndVertical();

            if (cancelLayout)
            {
                return;
            }
        }

        private static int AskToChangeChildrenLayers(GameObject obj, int newLayer)
        {
            int option = EditorUtility.DisplayDialogComplex(
                           "Change Layer",
                           $"Do you want to set the layer to '{LayerMask.LayerToName(newLayer)}' for all child objects as well?",
                           "Yes, change children",
                           "No, this object only",
                           "Cancel"
                       );

            if (option == 0)
            {
                SetLayerRecursively(obj, newLayer);
            }

            return option;
        }

        private static void SetLayerRecursively(GameObject obj, int newLayer)
        {
            foreach (Transform child in obj.transform)
            {
                Undo.RecordObject(child.gameObject, "Change Layer");
                child.gameObject.layer = newLayer;
                SetLayerRecursively(child.gameObject, newLayer);
            }
        }
    }
}
#endif