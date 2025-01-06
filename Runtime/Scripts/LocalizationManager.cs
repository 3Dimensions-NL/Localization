using System;
using System.Globalization;
using UnityEngine;
namespace _3Dimensions.Localization.Runtime.Scripts
{
    public class LocalizationManager : MonoBehaviour
    {
        public static LocalizationManager Instance
        {
            get
            {
                if (_instance == null) _instance = FindObjectOfType<LocalizationManager>(true);
                return _instance;
            }
        }

        private static LocalizationManager _instance;

        public static LanguageObject CurrentLanguage
        {
            get
            {
                if (!Instance)
                {
                    _currentLanguage = Resources.Load<LocalizationSettings>("LocalizationSettings").defaultLanguage;
                }
                else if (!_currentLanguage)
                {
                    _currentLanguage = Instance.DefaultLanguage;
                }
                return _currentLanguage;
            }
            private set => _currentLanguage = value;
        }
        private static LanguageObject _currentLanguage;

        public LocalizationSettings settings;

        public LanguageObject DefaultLanguage => settings.defaultLanguage;
        public LanguageObject[] AvailableLanguage => settings.languageSet.ToArray();

        public static event Action<LanguageObject> NewLanguageSetEvent;
        
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);

            LoadLanguage();
        }

        private void LoadLanguage()
        {
            //Try and get Language Player Preset
            string playerPrefsLanguage = PlayerPrefs.GetString("Language");
            
            //A preset is found, try and load the preset
            if (!string.IsNullOrEmpty(playerPrefsLanguage))
            {
                foreach (LanguageObject languageObject in settings.languageSet)
                {
                    if (playerPrefsLanguage == languageObject.name)
                    {
                        SetCurrentLanguage(languageObject);
                        return;
                    }
                }
            }
            
            //When no preset is set by player, try and load the language associated with the local culture
            string currentCulture = CultureInfo.CurrentCulture.Name;
                
            foreach (LanguageObject languageObject in settings.languageSet)
            {
                foreach (string culture in languageObject.cultures)
                {
                    if (currentCulture == culture)
                    {
                        SetCurrentLanguage(languageObject);
                        return;
                    }
                }
            }

            //No language found matching local culture, load english
            ApplyDefaultLanguage();
        }

        public void ApplyDefaultLanguage()
        {
            CurrentLanguage = DefaultLanguage;
            ApplyCurrentLanguage();
        }
        
        public void ShowLocalCultureInfo()
        {
            Debug.Log(CultureInfo.CurrentCulture.Name);
        }

        public void DeleteLanguagePlayerPref()
        {
            PlayerPrefs.DeleteKey("Language");
        }
        
        public static void SetCurrentLanguage(LanguageObject newCurrentLanguage)
        {
            CurrentLanguage = newCurrentLanguage;
            PlayerPrefs.SetString("Language", CurrentLanguage.name);
            NewLanguageSetEvent?.Invoke(newCurrentLanguage);
            ApplyCurrentLanguage();
        }
        
        private static void ApplyCurrentLanguage()
        {
            TranslationComponent[] translationsInScene = FindObjectsOfType<TranslationComponent>(true);
            foreach (TranslationComponent translationComponent in translationsInScene)
            {
                translationComponent.ApplyTranslation();
#if UNITY_EDITOR
                UnityEditor.EditorUtility.SetDirty(translationComponent);
#endif
            }
        }
    }
}