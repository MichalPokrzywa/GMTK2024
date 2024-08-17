using UnityEngine;
using UnityEngine.Events;

public class TranslationManager : MonoBehaviour
{
    public static Language currentLanguage;

    public TranslationsDictionary translationsDictionary;

    [Header("Translation Event")]
    public UnityEvent<Language> onLanguageChanged;

    private void Awake()
    {
        LoadLanguage();
    }

    public void SetLanguage(Language language)
    {
        currentLanguage = language;

        SaveLanguage(language.ToString());
        onLanguageChanged.Invoke(currentLanguage);
    }

    public string GetTranslationForKey(string key)
    {
        return translationsDictionary.GetTranslation(key);
    }

    public string GetTranslationForKeyWithParameter(string key, string parameter)
    {
        return translationsDictionary.GetTranslationWithParameter(key, parameter);
    }

    private void SaveLanguage(string languageKey)
    {
        DataManager.SaveData("Language", languageKey);
    }

    private void LoadLanguage()
    {
        if (DataManager.HasKey("Language"))
        {
            if (DataManager.LoadData<string>("Language") == "English")
            {
                SetLanguage(Language.English);
            }
            else if (DataManager.LoadData<string>("Language") == "Polish")
            {
                SetLanguage(Language.Polish);
            }
            else if (DataManager.LoadData<string>("Language") == "French")
            {
                SetLanguage(Language.French);
            }
            else if (DataManager.LoadData<string>("Language") == "German")
            {
                SetLanguage(Language.German);
            }
            else if (DataManager.LoadData<string>("Language") == "Japanese")
            {
                SetLanguage(Language.Spanish);
            }
            else if (DataManager.LoadData<string>("Language") == "Chinese")
            {
                SetLanguage(Language.Chinese);
            }
        }
    }
}

public enum Language
{
    English,
    Polish,
    French,
    German,
    Spanish,
    Chinese,
    Italian,
    Silesian
}