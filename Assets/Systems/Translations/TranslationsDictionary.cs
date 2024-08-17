using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Translations/Dictionary")]
public class TranslationsDictionary : ScriptableObject
{
    public List<TranslationCategory> translations;

    public string GetTranslation(string translateKey)
    {
        translateKey = translateKey.ToUpper();

        TranslationEntry entry = null;

        foreach (var translationCategory in translations)
        {
            entry = translationCategory.translations.Find(key => key.translateKey.ToUpper() == translateKey);

            if(entry != null)
            {
                switch (TranslationManager.currentLanguage)
                {
                    case Language.English:
                        return entry.english;
                    case Language.Chinese:
                        return entry.chinese;
                    case Language.Spanish:
                        return entry.spanish;
                    case Language.German:
                        return entry.german;
                    case Language.French:
                        return entry.french;
                    case Language.Italian:
                        return entry.italian;
                    case Language.Polish:
                        return entry.polish;
                    case Language.Silesian:
                        return entry.silesian;
                }

                break;
            }
        }

        Debug.LogError("MissingTraslation for key " + translateKey);
        return string.Empty;
    }

    public string GetTranslationWithParameter(string translateKey, string parameter)
    {
        if (GetTranslation(translateKey).Contains("{x}"))
        {
            return GetTranslation(translateKey).Replace("{x}", parameter);
        }

        return GetTranslation(translateKey).Replace("{X}", parameter);
    }
}

[System.Serializable]
public class TranslationCategory
{
    public string categoryName;
    public List<TranslationEntry> translations;
}


[System.Serializable]
public class TranslationEntry
{
    public string translateKey;
    public string english;
    public string chinese;
    public string spanish;
    public string german;
    public string french;
    public string italian;
    public string polish;
    public string silesian;

    public TranslationEntry()
    {

    }

    public TranslationEntry(string translateKey, string english, string polish, string french, string german, string spanish, string chinese, string italian, string silesian)
    {
        this.translateKey = translateKey;
        this.english = english;
        this.polish = polish;
        this.french = french;
        this.german = german;
        this.spanish = spanish;
        this.chinese = chinese;
        this.italian = italian;
        this.silesian = silesian;
    }
}