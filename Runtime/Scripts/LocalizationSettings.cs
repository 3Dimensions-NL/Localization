using System;
using System.Collections.Generic;
using UnityEngine;
namespace _3Dimensions.Localization.Runtime.Scripts
{
    [CreateAssetMenu(fileName = "Localization Editor Settings", menuName = "3Dimensions/Localization/New Settings")] [Serializable]
    public class LocalizationSettings : ScriptableObject
    {
        public string pathToStoreLanguages = Application.dataPath + "/Localization/Resources/Languages";
        public string pathToStoreTranslations = Application.dataPath + "/Localization/Translations";
        public LanguageObject defaultLanguage;
        public List<LanguageObject> languageSet = new();
    }
}
