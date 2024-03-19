using System.Globalization;
using Sirenix.OdinInspector;
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
            private set
            {
                _currentLanguage = value;
            }
        }
        private static LanguageObject _currentLanguage;

        [SerializeField] private LocalizationSettings settings;

        public LanguageObject DefaultLanguage => settings.defaultLanguage;
        public LanguageObject[] AvailableLanguage => settings.defaultLanguageSet.ToArray();
        
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
                Debug.Log("Loading Language: " + playerPrefsLanguage);
                
                foreach (LanguageObject languageObject in settings.defaultLanguageSet)
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
            Debug.Log("No player preferred Language, try local to find culture: " + currentCulture);

                
            foreach (LanguageObject languageObject in settings.defaultLanguageSet)
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

        [Button]
        private void ApplyDefaultLanguage()
        {
            CurrentLanguage = DefaultLanguage;
            ApplyCurrentLanguage();
        }
        
        [Button]
        private void ShowLocalCultureInfo()
        {
            Debug.Log(CultureInfo.CurrentCulture.Name);
        }

        [Button]
        private void DeleteLanguagePlayerPref()
        {
            PlayerPrefs.DeleteKey("Language");
        }
        
        public static void SetCurrentLanguage(LanguageObject newCurrentLanguage)
        {
            CurrentLanguage = newCurrentLanguage;
            PlayerPrefs.SetString("Language", CurrentLanguage.name);
            Debug.Log("Set language to: " + CurrentLanguage.name);

            ApplyCurrentLanguage();
        }
        
        private static void ApplyCurrentLanguage()
        {
            Debug.Log("Applied Language: " + CurrentLanguage.name);
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