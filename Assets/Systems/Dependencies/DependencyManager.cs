using UnityEngine;
using UnityEngine.InputSystem;

public class DependencyManager : MonoBehaviour
{
    public static AudioManager audioManager;
    public static DataManager dataManager = new DataManager();
    public static LinkOpener linkOpener = new LinkOpener();
    public static SceneLoader sceneLoader = new SceneLoader();
    public static TranslationManager translationManager;
    public static GMTKjam24 inputActions;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        audioManager = GetComponentInChildren<AudioManager>();
        translationManager = GetComponentInChildren<TranslationManager>();
        sceneLoader.LoadScene(GameScene.Menu);
        inputActions = new();
        inputActions.Enable();
    }
}