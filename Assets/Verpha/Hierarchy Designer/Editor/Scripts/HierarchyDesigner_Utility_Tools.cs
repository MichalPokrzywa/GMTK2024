#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections.Generic;

namespace Verpha.HierarchyDesigner
{
    public static class HierarchyDesigner_Utility_Tools
    {
        private const string MenuPath = "Hierarchy Designer/Hierarchy Tools/";
        private const int priorityBase = 40;

        #region Counting
        [MenuItem(MenuPath + "Counting/2D/Count All Sprites", false, priorityBase + 11)]
        public static void CountAllSprites() => CountAllOfComponentType<SpriteRenderer>("Sprites");

        [MenuItem(MenuPath + "Counting/2D/Count All 9-Sliced", false, priorityBase + 12)]
        public static void CountAll9SlicedSprites() => CountAll2DSpritesByType("9-Sliced");

        [MenuItem(MenuPath + "Counting/2D/Count All Capsules", false, priorityBase + 12)]
        public static void CountAllCapsuleSprites() => CountAll2DSpritesByType("Capsule");

        [MenuItem(MenuPath + "Counting/2D/Count All Circles", false, priorityBase + 12)]
        public static void CountAllCircleSprites() => CountAll2DSpritesByType("Circle");

        [MenuItem(MenuPath + "Counting/2D/Count All Hexagon Flat-Tops", false, priorityBase + 12)]
        public static void CountAllHexagonFlatTopSprites() => CountAll2DSpritesByType("Hexagon Flat-Top");

        [MenuItem(MenuPath + "Counting/2D/Count All Hexagon Pointed-Tops", false, priorityBase + 12)]
        public static void CountAllHexagonPointedTopSprites() => CountAll2DSpritesByType("Hexagon Pointed-Top");

        [MenuItem(MenuPath + "Counting/2D/Count All Isometric Diamonds", false, priorityBase + 12)]
        public static void CountAllIsometricDiamondSprites() => CountAll2DSpritesByType("Isometric Diamond");

        [MenuItem(MenuPath + "Counting/2D/Count All Squares", false, priorityBase + 12)]
        public static void CountAllSquareSprites() => CountAll2DSpritesByType("Square");

        [MenuItem(MenuPath + "Counting/2D/Count All Triangles", false, priorityBase + 12)]
        public static void CountAllTriangleSprites() => CountAll2DSpritesByType("Triangle");

        [MenuItem(MenuPath + "Counting/3D/Count All Mesh Renderers", false, priorityBase + 11)]
        public static void CountAllMeshRenderers() => CountAllOfComponentType<MeshRenderer>("Mesh Renderers");

        [MenuItem(MenuPath + "Counting/3D/Count All Cubes", false, priorityBase + 12)]
        public static void CountAllCubes() => CountAll3DObjects("Cube");

        [MenuItem(MenuPath + "Counting/3D/Count All Spheres", false, priorityBase + 12)]
        public static void CountAllSpheres() => CountAll3DObjects("Sphere");

        [MenuItem(MenuPath + "Counting/3D/Count All Capsules", false, priorityBase + 12)]
        public static void CountAllCapsules() => CountAll3DObjects("Capsule");

        [MenuItem(MenuPath + "Counting/3D/Count All Cylinders", false, priorityBase + 12)]
        public static void CountAllCylinders() => CountAll3DObjects("Cylinder");

        [MenuItem(MenuPath + "Counting/3D/Count All Planes", false, priorityBase + 12)]
        public static void CountAllPlanes() => CountAll3DObjects("Plane");

        [MenuItem(MenuPath + "Counting/3D/Count All Quads", false, priorityBase + 12)]
        public static void CountAllQuads() => CountAll3DObjects("Quad");

        [MenuItem(MenuPath + "Counting/UI/Count All Canvases", false, priorityBase + 12)]
        public static void CountAllCanvases() => CountAllOfComponentType<Canvas>("Canvases");

        [MenuItem(MenuPath + "Counting/UI/Count All Images", false, priorityBase + 12)]
        public static void CountAllImages() => CountAllOfComponentType<Image>("Images");

        [MenuItem(MenuPath + "Counting/UI/Count All Raw Images", false, priorityBase + 12)]
        public static void CountAllRawImages() => CountAllOfComponentType<RawImage>("Raw Images");

        [MenuItem(MenuPath + "Counting/UI/Count All Toggles", false, priorityBase + 12)]
        public static void CountAllToggles() => CountAllOfComponentType<Toggle>("Toggles");

        [MenuItem(MenuPath + "Counting/UI/Count All Sliders", false, priorityBase + 12)]
        public static void CountAllSliders() => CountAllOfComponentType<Slider>("Sliders");

        [MenuItem(MenuPath + "Counting/UI/Count All Scrollbars", false, priorityBase + 12)]
        public static void CountAllScrollbars() => CountAllOfComponentType<Scrollbar>("Scrollbars");

        [MenuItem(MenuPath + "Counting/UI/Count All Scroll Views", false, priorityBase + 12)]
        public static void CountAllScrollViews() => CountAllOfComponentType<ScrollRect>("Scroll Rects");

        [MenuItem(MenuPath + "Counting/UI/Count All Buttons", false, priorityBase + 12)]
        public static void CountAllButtons() => CountAllOfComponentType<Button>("Buttons");

        #region TMP
        private static void CountTMPComponentIfAvailable<T>() where T : Component
        {
            if (IsTMPAvailable())
            {
                CountAllOfComponentType<T>("TMP Component");
            }
            else
            {
                EditorUtility.DisplayDialog("TMP Not Found", "TMP wasn't found in the project, make sure you have it enabled.", "OK");
            }
        }

        [MenuItem(MenuPath + "Counting/UI/Count All Texts - TextMeshPro", false, priorityBase + 13)]
        public static void CountAllTextMeshProTexts() => CountTMPComponentIfAvailable<TMPro.TMP_Text>();

        [MenuItem(MenuPath + "Counting/UI/Count All Dropdowns - TextMeshPro", false, priorityBase + 13)]
        public static void CountAllTextMeshProDropdowns() => CountTMPComponentIfAvailable<TMPro.TMP_Dropdown>();

        [MenuItem(MenuPath + "Counting/UI/Count All Input Fields - TextMeshPro", false, priorityBase + 13)]
        public static void CountAllTextMeshProInputFields() => CountTMPComponentIfAvailable<TMPro.TMP_InputField>();
        #endregion

        [MenuItem(MenuPath + "Counting/Count All Cameras", false, priorityBase + 13)]
        public static void CountAllCameras() => CountAllOfComponentType<Camera>("Cameras");

        [MenuItem(MenuPath + "Counting/Count All Lights", false, priorityBase + 13)]
        public static void CountAllLights() => CountAllOfComponentType<Light>("Lights");

        [MenuItem(MenuPath + "Counting/Count All Audio Sources", false, priorityBase + 13)]
        public static void CountAllAudioSources() => CountAllOfComponentType<AudioSource>("Audio Sources");

        [MenuItem(MenuPath + "Counting/Count All Particle Systems", false, priorityBase + 13)]
        public static void CountAllParticleSystems() => CountAllOfComponentType<ParticleSystem>("Particle Systems");

        [MenuItem(MenuPath + "Counting/Count All Terrains", false, priorityBase + 13)]
        public static void CountAllTerrains() => CountAllOfComponentType<Terrain>("Terrains");

        [MenuItem(MenuPath + "Counting/Count All Folders", false, priorityBase + 14)]
        public static void CountAllFolders() => CountAllOfComponentType<HierarchyDesignerFolder>("Hierarchy Designer Folders");

        [MenuItem(MenuPath + "Counting/Count All Separators", false, priorityBase + 14)]
        public static void CountAllSeparators() => CountAllSeparatorsInScene();
        #endregion

        #region Locking
        [MenuItem(MenuPath + "Locking/Lock All Folders", false, priorityBase + 11)]
        public static void LockAllFolders() => SetLockStateForAllFolders(true);

        [MenuItem(MenuPath + "Locking/Lock All GameObjects", false, priorityBase + 12)]
        public static void LockAllGameObjects() => SetLockStateForAllGameObjects(true);

        [MenuItem(MenuPath + "Locking/Lock All GameObject Parents", false, priorityBase + 13)]
        public static void LockAllGameObjectParents() => SetLockStateForAllGameObjectParents(true);

        [MenuItem(MenuPath + "Locking/Lock GameObject", false, priorityBase + 14)]
        public static void LockGameObject() => ToggleGameObjectLock(true);

        [MenuItem(MenuPath + "Locking/Unlock All Folders", false, priorityBase + 15)]
        public static void UnlockAllFolders() => SetLockStateForAllFolders(false);

        [MenuItem(MenuPath + "Locking/Unlock All GameObjects", false, priorityBase + 16)]
        public static void UnlockAllGameObjects() => SetLockStateForAllGameObjects(false);

        [MenuItem(MenuPath + "Locking/Unlock All GameObject Parents", false, priorityBase + 17)]
        public static void UnlockAllGameObjectParents() => SetLockStateForAllGameObjectParents(false);

        [MenuItem(MenuPath + "Locking/Unlock GameObject", false, priorityBase + 18)]
        private static void UnlockGameObject() => ToggleGameObjectLock(false);
        #endregion

        #region Renaming
        [MenuItem(MenuPath + "Renaming/Rename Selected GameObjects With Auto Index", false, priorityBase + 12)]
        public static void RenameGameObjectsWithIndex() => RenameSelectedGameObjects("rename with automatic enumeration");
        #endregion

        #region Selecting
        [MenuItem(MenuPath + "Selecting/2D/Select All Sprites", false, priorityBase + 12)]
        public static void SelectAllSprites() => SelectAllOfComponentType<SpriteRenderer>();

        [MenuItem(MenuPath + "Selecting/2D/Select All 9-Sliced", false, priorityBase + 13)]
        public static void SelectAll9Sliced() => SelectAll2DSpritesByType("9-Sliced");

        [MenuItem(MenuPath + "Selecting/2D/Select All Capsules", false, priorityBase + 13)]
        public static void SelectAllCapsuleSprites() => SelectAll2DSpritesByType("Capsule");

        [MenuItem(MenuPath + "Selecting/2D/Select All Circles", false, priorityBase + 13)]
        public static void SelectAllCircleSprites() => SelectAll2DSpritesByType("Circle");

        [MenuItem(MenuPath + "Selecting/2D/Select All Hexagon Flat-Tops", false, priorityBase + 13)]
        public static void SelectAllHexagonFlatTopSprites() => SelectAll2DSpritesByType("Hexagon Flat-Top");

        [MenuItem(MenuPath + "Selecting/2D/Select All Hexagon Pointed-Tops", false, priorityBase + 13)]
        public static void SelectAllHexagonPointedTopSprites() => SelectAll2DSpritesByType("Hexagon Pointed-Top");

        [MenuItem(MenuPath + "Selecting/2D/Select All Isometric Diamonds", false, priorityBase + 13)]
        public static void SelectAllIsometricDiamondSprites() => SelectAll2DSpritesByType("Isometric Diamond");

        [MenuItem(MenuPath + "Selecting/2D/Select All Squares", false, priorityBase + 13)]
        public static void SelectAllSquareSprites() => SelectAll2DSpritesByType("Square");

        [MenuItem(MenuPath + "Selecting/2D/Select All Triangles", false, priorityBase + 13)]
        public static void SelectAllTriangleSprites() => SelectAll2DSpritesByType("Triangle");

        [MenuItem(MenuPath + "Selecting/3D/Select All Mesh Renderers", false, priorityBase + 12)]
        public static void SelectAllMeshRenderers() => SelectAllOfComponentType<MeshRenderer>();

        [MenuItem(MenuPath + "Selecting/3D/Select All Cubes", false, priorityBase + 13)]
        public static void SelectAllCubes() => SelectAll3DObjects("Cube");

        [MenuItem(MenuPath + "Selecting/3D/Select All Spheres", false, priorityBase + 13)]
        public static void SelectAllSpheres() => SelectAll3DObjects("Sphere");

        [MenuItem(MenuPath + "Selecting/3D/Select All Capsules", false, priorityBase + 13)]
        public static void SelectAllCapsules() => SelectAll3DObjects("Capsule");

        [MenuItem(MenuPath + "Selecting/3D/Select All Cylinders", false, priorityBase + 13)]
        public static void SelectAllCylinders() => SelectAll3DObjects("Cylinder");

        [MenuItem(MenuPath + "Selecting/3D/Select All Planes", false, priorityBase + 13)]
        public static void SelectAllPlanes() => SelectAll3DObjects("Plane");

        [MenuItem(MenuPath + "Selecting/3D/Select All Quads", false, priorityBase + 13)]
        public static void SelectAllQuads() => SelectAll3DObjects("Quad");

        [MenuItem(MenuPath + "Selecting/UI/Select All Canvases", false, priorityBase + 13)]
        public static void SelectAllCanvases() => SelectAllOfComponentType<Canvas>();

        [MenuItem(MenuPath + "Selecting/UI/Select All Images", false, priorityBase + 13)]
        public static void SelectAllImages() => SelectAllOfComponentType<Image>();

        [MenuItem(MenuPath + "Selecting/UI/Select All Raw Images", false, priorityBase + 13)]
        public static void SelectAllRawImages() => SelectAllOfComponentType<RawImage>();

        [MenuItem(MenuPath + "Selecting/UI/Select All Toggles", false, priorityBase + 13)]
        public static void SelectAllToggles() => SelectAllOfComponentType<Toggle>();

        [MenuItem(MenuPath + "Selecting/UI/Select All Sliders", false, priorityBase + 13)]
        public static void SelectAllSliders() => SelectAllOfComponentType<Slider>();

        [MenuItem(MenuPath + "Selecting/UI/Select All Scrollbars", false, priorityBase + 13)]
        public static void SelectAllScrollbars() => SelectAllOfComponentType<Scrollbar>();

        [MenuItem(MenuPath + "Selecting/UI/Select All Scroll Views", false, priorityBase + 13)]
        public static void SelectAllScrollViews() => SelectAllOfComponentType<ScrollRect>();

        [MenuItem(MenuPath + "Selecting/UI/Select All Buttons", false, priorityBase + 13)]
        public static void SelectAllButtons() => SelectAllOfComponentType<Button>();

        #region TMP
        private static bool? _isTMPAvailable;
        private static bool IsTMPAvailable()
        {
            if (!_isTMPAvailable.HasValue)
            {
                _isTMPAvailable = AssetDatabase.FindAssets("t:TMP_Settings").Length > 0;
            }
            return _isTMPAvailable.Value;
        }
        private static void SelectTMPComponentIfAvailable<T>() where T : Component
        {
            if (IsTMPAvailable())
            {
                SelectAllOfComponentType<T>();
            }
            else
            {
                EditorUtility.DisplayDialog("TMP Not Found", "TMP wasn't found in the project, make sure you have it enabled.", "OK");
            }
        }

        [MenuItem(MenuPath + "Selecting/UI/Select All Texts - TextMeshPro", false, priorityBase + 14)]
        public static void SelectAllTextMeshProTexts() => SelectTMPComponentIfAvailable<TMPro.TMP_Text>();

        [MenuItem(MenuPath + "Selecting/UI/Select All Dropdowns - TextMeshPro", false, priorityBase + 14)]
        public static void SelectAllTextMeshProDropdowns() => SelectTMPComponentIfAvailable<TMPro.TMP_Dropdown>();

        [MenuItem(MenuPath + "Selecting/UI/Select All Input Fields - TextMeshPro", false, priorityBase + 14)]
        public static void SelectAllTextMeshProInputFields() => SelectTMPComponentIfAvailable<TMPro.TMP_InputField>();
        #endregion

        [MenuItem(MenuPath + "Selecting/Select All Cameras", false, priorityBase + 14)]
        public static void SelectAllCameras() => SelectAllOfComponentType<Camera>();

        [MenuItem(MenuPath + "Selecting/Select All Lights", false, priorityBase + 14)]
        public static void SelectAllLights() => SelectAllOfComponentType<Light>();

        [MenuItem(MenuPath + "Selecting/Select All Audio Sources", false, priorityBase + 14)]
        public static void SelectAllAudioSources() => SelectAllOfComponentType<AudioSource>();

        [MenuItem(MenuPath + "Selecting/Select All Particle Systems", false, priorityBase + 14)]
        public static void SelectAllParticleSystems() => SelectAllOfComponentType<ParticleSystem>();

        [MenuItem(MenuPath + "Selecting/Select All Terrains", false, priorityBase + 14)]
        public static void SelectAllTerrains() => SelectAllOfComponentType<Terrain>();

        [MenuItem(MenuPath + "Selecting/Select All Folders", false, priorityBase + 15)]
        public static void SelectAllFolders() => SelectAllOfComponentType<HierarchyDesignerFolder>();

        [MenuItem(MenuPath + "Selecting/Select All Separators", false, priorityBase + 15)]
        public static void SelectAllSeparators() => SelectAllSeparatorsInScene();
        #endregion

        #region Sorting
        [MenuItem(MenuPath + "Sorting/Sort Alphabetically Ascending", false, priorityBase + 13)]
        private static void SortAlphabeticallyAscending() => SortSelectedGameObjectChildren((a, b) => a.name.CompareTo(b.name), "sort its children alphabetically ascending");

        [MenuItem(MenuPath + "Sorting/Sort Alphabetically Descending", false, priorityBase + 14)]
        private static void SortAlphabeticallyDescending() => SortSelectedGameObjectChildren((a, b) => b.name.CompareTo(a.name), "sort its children alphabetically descending");

        [MenuItem(MenuPath + "Sorting/Sort By Components Amount Ascending", false, priorityBase + 14)]
        private static void SortByComponentsAmountAscending() => SortSelectedGameObjectChildren((a, b) => a.GetComponents<Component>().Length.CompareTo(b.GetComponents<Component>().Length), "sort its children by components amount ascending");

        [MenuItem(MenuPath + "Sorting/Sort By Components Amount Descending", false, priorityBase + 14)]
        private static void SortByComponentsAmountDescending() => SortSelectedGameObjectChildren((a, b) => b.GetComponents<Component>().Length.CompareTo(a.GetComponents<Component>().Length), "sort its children by components amount descending");

        [MenuItem(MenuPath + "Sorting/Sort By Length Ascending", false, priorityBase + 14)]
        private static void SortByLengthAscending() => SortSelectedGameObjectChildren((a, b) => a.name.Length.CompareTo(b.name.Length), "sort its children by length ascending");

        [MenuItem(MenuPath + "Sorting/Sort By Length Descending", false, priorityBase + 14)]
        private static void SortByLengthDescending() => SortSelectedGameObjectChildren((a, b) => b.name.Length.CompareTo(a.name.Length), "sort its children by length descending");

        [MenuItem(MenuPath + "Sorting/Sort Randomly", false, priorityBase + 14)]
        private static void SortRandomly() => SortSelectedGameObjectChildrenRandomly("sort its children randomly");
        #endregion

        private static void CountAllOfComponentType<T>(string componentName) where T : Component
        {
            var allGameObjects = GetAllSceneGameObjects();
            int count = 0;
            List<string> names = new List<string>();

            foreach (GameObject gameObject in allGameObjects)
            {
                if (gameObject.GetComponent<T>() != null)
                {
                    count++;
                    names.Add($"{gameObject.name}");
                }
            }

            string namesString = names.Count > 0 ? string.Join(", ", names) : "none";
            Debug.Log($"Total {componentName} in the scene: {count}.\n{componentName} Found: {namesString}.\n");
        }

        private static void CountAll2DSpritesByType(string spriteType)
        {
            var allGameObjects = GetAllSceneGameObjects();
            int count = 0;
            List<string> names = new List<string>();

            foreach (GameObject gameObject in allGameObjects)
            {
                SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
                if (spriteRenderer != null && spriteRenderer.sprite != null && spriteRenderer.sprite.name.Contains(spriteType))
                {
                    count++;
                    names.Add($"{gameObject.name}");
                }
            }

            string namesString = names.Count > 0 ? string.Join(", ", names) : "none";
            Debug.Log($"Total 2D {spriteType} sprites in the scene: {count}.\n{spriteType} Sprites Found: {namesString}.\n");
        }

        private static void CountAll3DObjects(string primitiveType)
        {
            var allGameObjects = GetAllSceneGameObjects();
            int count = 0;
            List<string> names = new List<string>();

            foreach (GameObject gameObject in allGameObjects)
            {
                MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
                if (meshRenderer != null && gameObject.name.Contains(primitiveType))
                {
                    count++;
                    names.Add($"{gameObject.name}");
                }
            }

            string namesString = names.Count > 0 ? string.Join(", ", names) : "none";
            Debug.Log($"Total 3D {primitiveType} objects in the scene: {count}.\n{primitiveType} Objects Found: {namesString}.\n");
        }

        private static void CountAllSeparatorsInScene()
        {
            var allGameObjects = GetAllSceneGameObjects();
            int separatorCount = 0;
            List<string> names = new List<string>();

            foreach (GameObject gameObject in allGameObjects)
            {
                if (IsSeparator(gameObject))
                {
                    separatorCount++;
                    names.Add($"{gameObject.name}");
                }
            }

            string namesString = names.Count > 0 ? string.Join(", ", names) : "none";
            Debug.Log($"Total separators in the scene: {separatorCount}.\nSeparators Found: {namesString}.\n");
        }

        private static void SetLockStateForAllFolders(bool lockState)
        {
            foreach (GameObject gameObject in GetAllSceneGameObjects())
            {
                if (HierarchyDesigner_Visual_Folder.IsFolder(gameObject))
                {
                    SetGameObjectLockState(gameObject, lockState);
                    SetEditableState(gameObject, !lockState);
                }
            }
            HierarchyDesigner_Visual_Tools.Cleanup();
        }

        public static void SetGameObjectLockState(GameObject gameObject, bool isLocked)
        {
            if (!HierarchyDesigner_Utility_Separator.IsSeparator(gameObject))
            {
                SetEditableState(gameObject, !isLocked);
            }
        }

        public static void SetEditableState(GameObject gameObject, bool editable)
        {
            Undo.RegisterCompleteObjectUndo(gameObject, $"{(editable ? "Unlock" : "Lock")} GameObject");

            foreach (Component component in gameObject.GetComponents<Component>())
            {
                if (component)
                {
                    Undo.RegisterCompleteObjectUndo(component, $"{(editable ? "Unlock" : "Lock")} Component");
                    component.hideFlags = editable ? HideFlags.None : HideFlags.NotEditable;
                }
            }

            gameObject.hideFlags = editable ? HideFlags.None : HideFlags.NotEditable;
            EditorUtility.SetDirty(gameObject);

            if (editable)
            {
                SceneVisibilityManager.instance.EnablePicking(gameObject, true);
            }
            else
            {
                SceneVisibilityManager.instance.DisablePicking(gameObject, true);
            }

            var allEditorWindows = Resources.FindObjectsOfTypeAll<EditorWindow>();
            foreach (var inspector in allEditorWindows)
            {
                if (inspector.GetType().Name == "InspectorWindow")
                {
                    inspector.Repaint();
                }
            }
        }

        private static void SetLockStateForAllGameObjects(bool lockState)
        {
            foreach (GameObject gameObject in GetAllSceneGameObjects())
            {
                if (!HierarchyDesigner_Utility_Separator.IsSeparator(gameObject))
                {
                    SetGameObjectLockState(gameObject, lockState);
                    SetEditableState(gameObject, !lockState);
                }
            }
            HierarchyDesigner_Visual_Tools.Cleanup();
        }

        private static void SetLockStateForAllGameObjectParents(bool lockState)
        {
            foreach (GameObject gameObject in GetAllSceneGameObjects())
            {
                if (IsGameObjectParent(gameObject))
                {
                    SetGameObjectLockState(gameObject, lockState);
                    SetEditableState(gameObject, !lockState);
                }
            }
            HierarchyDesigner_Visual_Tools.Cleanup();
        }

        private static void ToggleGameObjectLock(bool lockState)
        {
            GameObject selectedGameObject = Selection.activeGameObject;
            if (selectedGameObject != null && !HierarchyDesigner_Utility_Separator.IsSeparator(selectedGameObject))
            {
                if (IsGameObjectLocked(selectedGameObject) != lockState)
                {
                    SetGameObjectLockState(selectedGameObject, lockState);
                    HierarchyDesigner_Visual_Tools.Cleanup();
                }
                else
                {
                    Debug.Log(selectedGameObject + (lockState ? " is already locked" : " is already unlocked"));
                }
            }
            else
            {
                EditorUtility.DisplayDialog("Verpha > Hierarchy Designer > Hierarchy Tools > Locking",
                                            $"No GameObject selected. Please select a GameObject to {(lockState ? "lock" : "unlock")}.",
                                            "OK");
            }
        }

        public static bool IsGameObjectLocked(GameObject gameObject)
        {
            if (gameObject == null) { return false; }
            return (gameObject.hideFlags & HideFlags.NotEditable) == HideFlags.NotEditable;
        }

        private static IEnumerable<GameObject> GetAllSceneGameObjects()
        {
            var rootObjects = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
            var allGameObjects = new List<GameObject>();

            var stack = new Stack<GameObject>(rootObjects);
            while (stack.Count > 0)
            {
                var current = stack.Pop();
                allGameObjects.Add(current);

                foreach (Transform child in current.transform)
                {
                    stack.Push(child.gameObject);
                }
            }

            return allGameObjects;
        }

        private static bool IsGameObjectParent(GameObject gameObject)
        {
            return gameObject.transform.childCount > 0;
        }

        public static void RenameSelectedGameObjects(string sortingActionDescription)
        {
            GameObject[] selectedGameObjects = Selection.gameObjects;
            if (selectedGameObjects.Length == 0)
            {
                EditorUtility.DisplayDialog("Verpha > Hierarchy Designer > Hierarchy Tools > Renaming", $"No GameObject selected. Please select one or more GameObjects to {sortingActionDescription}.", "OK");
                return;
            }

            HierarchyDesigner_Window_Renaming.OpenWindow(selectedGameObjects, new Vector2(0, 0));
        }

        private static void SelectAllOfComponentType<T>() where T : Component
        {
            var allGameObjects = GetAllSceneGameObjects();
            var gameObjectsWithComponent = new List<GameObject>();

            foreach (var gameObject in allGameObjects)
            {
                if (gameObject.GetComponent<T>() != null)
                {
                    gameObjectsWithComponent.Add(gameObject);
                }
            }

            if (gameObjectsWithComponent.Count > 0)
            {
                Selection.objects = gameObjectsWithComponent.ToArray();
            }
            else
            {
                EditorUtility.DisplayDialog("Verpha > Hierarchy Designer > Hierarchy Tools > Selection", $"No GameObjects with {typeof(T).Name} found in the current scene.", "OK");
            }
        }

        private static void SelectAll2DSpritesByType(string spriteType)
        {
            var allGameObjects = GetAllSceneGameObjects();
            var selectedSprites = new List<GameObject>();

            foreach (var gameObject in allGameObjects)
            {
                SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
                if (spriteRenderer != null && spriteRenderer.sprite != null && spriteRenderer.sprite.name.Contains(spriteType))
                {
                    selectedSprites.Add(gameObject);
                }
            }

            SelectOrDisplayMessage(selectedSprites, spriteType, "2D");
        }

        private static void SelectAll3DObjects(string primitiveType)
        {
            var allGameObjects = GetAllSceneGameObjects();
            var gameObjectsOfPrimitiveType = new List<GameObject>();

            foreach (var gameObject in allGameObjects)
            {
                MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
                if (meshRenderer != null && gameObject.name.Contains(primitiveType))
                {
                    gameObjectsOfPrimitiveType.Add(gameObject);
                }
            }

            SelectOrDisplayMessage(gameObjectsOfPrimitiveType, primitiveType, "3D");
        }

        private static void SelectOrDisplayMessage(List<GameObject> gameObjects, string type, string dimension)
        {
            if (gameObjects.Count > 0)
            {
                Selection.objects = gameObjects.ToArray();
            }
            else
            {
                EditorUtility.DisplayDialog("Verpha > Hierarchy Designer > Hierarchy Tools > Selection", $"No {dimension} {type} objects found in the current scene.", "OK");
            }
        }

        private static void SelectAllSeparatorsInScene()
        {
            var allGameObjects = GetAllSceneGameObjects();
            var separatorObjects = new List<GameObject>();

            foreach (GameObject gameObject in allGameObjects)
            {
                if (IsSeparator(gameObject))
                {
                    separatorObjects.Add(gameObject);
                }
            }

            if (separatorObjects.Count > 0)
            {
                Selection.objects = separatorObjects.ToArray();
            }
            else
            {
                EditorUtility.DisplayDialog("Verpha > Hierarchy Designer > Hierarchy Tools > Selection", "No Separators found in the current scene.", "OK");
            }
        }

        private static bool IsSeparator(GameObject gameObject)
        {
            return gameObject != null && gameObject.tag == "EditorOnly" && gameObject.name.StartsWith("//");
        }

        private static void SortSelectedGameObjectChildren(System.Comparison<GameObject> comparison, string sortingActionDescription)
        {
            GameObject selectedGameObject = Selection.activeGameObject;
            if (selectedGameObject != null)
            {
                List<GameObject> children = new List<GameObject>();
                foreach (Transform child in selectedGameObject.transform)
                {
                    children.Add(child.gameObject);
                }

                children.Sort(comparison);

                for (int i = 0; i < children.Count; i++)
                {
                    children[i].transform.SetSiblingIndex(i);
                }
            }
            else
            {
                EditorUtility.DisplayDialog("Verpha > Hierarchy Designer > Hierarchy Tools > Sorting", $"No GameObject selected. Please select a parent GameObject to {sortingActionDescription}.", "OK");
            }
        }

        private static void SortSelectedGameObjectChildrenRandomly(string sortingActionDescription)
        {
            GameObject selectedGameObject = Selection.activeGameObject;
            if (selectedGameObject != null)
            {
                List<GameObject> children = new List<GameObject>();
                foreach (Transform child in selectedGameObject.transform)
                {
                    children.Add(child.gameObject);
                }

                System.Random rng = new System.Random();
                int n = children.Count;
                while (n > 1)
                {
                    n--;
                    int k = rng.Next(n + 1);
                    GameObject value = children[k];
                    children[k] = children[n];
                    children[n] = value;
                }

                for (int i = 0; i < children.Count; i++)
                {
                    children[i].transform.SetSiblingIndex(i);
                }
            }
            else
            {
                EditorUtility.DisplayDialog("Verpha > Hierarchy Designer > Hierarchy Tools > Sorting", $"No GameObject selected. Please select a parent GameObject to {sortingActionDescription}.", "OK");
            }
        }
    }
}
#endif