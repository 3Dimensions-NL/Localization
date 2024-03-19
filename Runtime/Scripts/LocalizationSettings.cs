using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
namespace _3Dimensions.Localization.Runtime.Scripts
{
    [CreateAssetMenu(fileName = "Localization Editor Settings", menuName = "3Dimensions/Localization/New Settings")] [InlineEditor]
    public class LocalizationSettings : ScriptableObject
    {
        public string pathToStoreTranslations; 
        public LanguageObject defaultLanguage;
        public List<LanguageObject> defaultLanguageSet = new List<LanguageObject>();
        public string prefix;

        [Button]
        private void ApplyDefaultLanguage()
        {
            TranslationComponent[] translationComponents = FindObjectsOfType<TranslationComponent>(true);
            foreach (var tc in translationComponents)
            {
                tc.ApplyTranslation();
            }
        }
    }
}
