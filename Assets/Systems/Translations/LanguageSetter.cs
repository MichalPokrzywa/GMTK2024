using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanguageSetter : MonoBehaviour
{
    [Header("Language Buttons Icons")]
    public Image englishButton;
    public Image polishButton;
    public Image frenchButton;
    public Image germanButton;
    public Image spanishButton;
    public Image chineseButton;
    public Image italianButton;
    public Image silesianButton;

    [Header("State Colors")]
    Color enableColor = Color.white;
    Color disableColor = new Color(1f, 1f, 1f, 0.4f);

    private void OnEnable()
    {
        AdjustLanguageButtons(TranslationManager.currentLanguage);

        DependencyManager.translationManager.onLanguageChanged.AddListener(AdjustLanguageButtons);
    }

    private void OnDisable()
    {
        DependencyManager.translationManager.onLanguageChanged.RemoveListener(AdjustLanguageButtons);
    }

    public void SetEnglishLanguage()
    {
        DependencyManager.translationManager.SetLanguage(Language.English);
    }

    public void SetPolishLanguage()
    {
        DependencyManager.translationManager.SetLanguage(Language.Polish);
    }

    public void SetFrenchLanguage()
    {
        DependencyManager.translationManager.SetLanguage(Language.French);
    }

    public void SetGermanLanguage()
    {
        DependencyManager.translationManager.SetLanguage(Language.German);
    }

    public void SetSpanishLanguage()
    {
        DependencyManager.translationManager.SetLanguage(Language.Spanish);
    }

    public void SetChineseLanguage()
    {
        DependencyManager.translationManager.SetLanguage(Language.Chinese);
    }

    public void SetItalianLanguage()
    {
        DependencyManager.translationManager.SetLanguage(Language.Italian);
    }

    public void SetSilesianLanguage()
    {
        DependencyManager.translationManager.SetLanguage(Language.Silesian);
    }

    private void AdjustLanguageButtons(Language language)
    {
        englishButton.color = language == Language.English ? enableColor : disableColor;
        polishButton.color = language == Language.Polish ? enableColor : disableColor;
        frenchButton.color = language == Language.French ? enableColor : disableColor;
        germanButton.color = language == Language.German ? enableColor : disableColor;
        spanishButton.color = language == Language.Spanish ? enableColor : disableColor;
        chineseButton.color = language == Language.Chinese ? enableColor : disableColor;
        chineseButton.color = language == Language.Italian ? enableColor : disableColor;
        chineseButton.color = language == Language.Silesian ? enableColor : disableColor;
    }
}
