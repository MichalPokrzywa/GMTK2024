using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SceneLoadingButton : MonoBehaviour
{
    public string sceneName;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(LoadScene);
    }

    private void OnDestroy()
    {
        GetComponent<Button>().onClick.RemoveListener(LoadScene);
    }

    private void LoadScene()
    {
        DependencyManager.sceneLoader.LoadScene(sceneName);
    }
}
