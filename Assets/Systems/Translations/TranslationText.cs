using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class TranslationText : MonoBehaviour
{
    public string translationKey;
    private TMP_Text label;

    public bool upperText;

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        if (label == null)
        {
            label = GetComponent<TMP_Text>();
        }
    }

    private void OnEnable()
    {
        UpdateTextTranslation(TranslationManager.currentLanguage);
        DependencyManager.translationManager.onLanguageChanged.AddListener(UpdateTextTranslation);
    }

    private void OnDisable()
    {
        DependencyManager.translationManager.onLanguageChanged.RemoveListener(UpdateTextTranslation);
    }

    protected virtual void UpdateTextTranslation(Language language)
    {
        string textToDisplay = DependencyManager.translationManager.GetTranslationForKey(translationKey);

        if(upperText)
        {
            textToDisplay = textToDisplay.ToUpper();
        }

        if(label == null)
        {
            label = GetComponent<TMP_Text>();
        }

        if (textToDisplay != "")
        {
            label.text = textToDisplay;
        }
    }

    protected virtual void UpdateTextTranslationWithParameter(string parameter)
    {
        Initialize();

        string textToDisplay = DependencyManager.translationManager.GetTranslationForKeyWithParameter(translationKey, parameter);

        if (upperText == true)
        {
            textToDisplay = textToDisplay.ToUpper();
        }

        if (textToDisplay != string.Empty)
        {
            label.text = textToDisplay;
        }
    }

    public void SetTranslationKey(string key)
    {
        translationKey = key.ToUpper();
        UpdateTextTranslation(TranslationManager.currentLanguage);
    }
}
