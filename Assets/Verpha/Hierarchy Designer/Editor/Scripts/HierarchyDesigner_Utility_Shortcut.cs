#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Verpha.HierarchyDesigner
{
    [InitializeOnLoad]
    public static class HierarchyDesigner_Utility_Shortcut
    {
        static HierarchyDesigner_Utility_Shortcut()
        {
            EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyWindowItemGUI;
        }

        private static void OnHierarchyWindowItemGUI(int instanceID, Rect selectionRect)
        {
            Event currentEvent = Event.current;

            if (currentEvent.type != EventType.KeyDown && currentEvent.type != EventType.MouseDown)
            {
                return;
            }

            GameObject obj = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
            if (obj == null || !selectionRect.Contains(currentEvent.mousePosition))
            {
                return;
            }

            if (IsShortcutPressed(HierarchyDesigner_Manager_Settings.EnableDisableShortcut))
            {
                Undo.RecordObject(obj, "Toggle Active State");
                obj.SetActive(!obj.activeSelf);
                Event.current.Use();
            }

            if (IsShortcutPressed(HierarchyDesigner_Manager_Settings.LockUnlockShortcut))
            {
                bool isCurrentlyLocked = HierarchyDesigner_Utility_Tools.IsGameObjectLocked(obj);
                Undo.RecordObject(obj, "Toggle Lock State");
                HierarchyDesigner_Utility_Tools.SetGameObjectLockState(obj, !isCurrentlyLocked);
                Event.current.Use();
            }
        }

        public static bool IsShortcutPressed(KeyCode shortcutKey)
        {
            Event currentEvent = Event.current;

            if (shortcutKey >= KeyCode.Alpha0 && shortcutKey <= KeyCode.Menu)
            {
                return currentEvent.type == EventType.KeyDown && currentEvent.keyCode == shortcutKey;
            }

            int mouseButton = GetMouseButtonFromKeyCode(shortcutKey);
            if (mouseButton != -1)
            {
                return currentEvent.type == EventType.MouseDown && currentEvent.button == mouseButton;
            }

            return false;
        }

        private static int GetMouseButtonFromKeyCode(KeyCode keyCode)
        {
            switch (keyCode)
            {
                case KeyCode.Mouse0: return 0;
                case KeyCode.Mouse1: return 1;
                case KeyCode.Mouse2: return 2;
                case KeyCode.Mouse3: return 3;
                case KeyCode.Mouse4: return 4;
                case KeyCode.Mouse5: return 5;
                case KeyCode.Mouse6: return 6;
                default: return -1;
            }
        }
    }
}   
#endif