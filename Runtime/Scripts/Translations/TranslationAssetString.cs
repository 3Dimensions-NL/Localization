using System;
using Sirenix.OdinInspector;
using UnityEngine;
namespace _3Dimensions.Localization.Runtime.Scripts.Translations
{
    [CreateAssetMenu(fileName = "New String Translation", menuName = "3Dimensions/Localization/New Translation String")]
    [Serializable, InlineEditor]
    public class TranslationAssetString : TranslationAsset
    {
        public TranslationString[] translations;
        
        [Serializable, InlineEditor]
        public class TranslationString : Translation
        {
            [TextArea] public string text;
        
            public override T GetValue<T>()
            {
                return (T) Convert.ChangeType(text, typeof(T));
            }
        }
        
        public override T GetValue<T>()
        {
            foreach (TranslationString translation in translations)
            {
                if (translation.language == LocalizationManager.CurrentLanguage) return translation.GetValue<T>();
            }

            Debug.LogWarning("No translation found", this);
            return (T) Convert.ChangeType(null, typeof(T));
        }

        #if UNITY_EDITOR
        private void Reset()
        {
            LoadLanguages();
        }

        private void LoadLanguages()
        {
            if (translations != null)
            {
                if (translations.Length != 0) return;
            }

            LanguageObject[] languages = Resources.Load<LocalizationSettings>("LocalizationSettings").defaultLanguageSet.ToArray();
            translations = new TranslationString[languages.Length];
                
            for (int i = 0; i < languages.Length; i++)
            {
                translations[i] = new TranslationString();
                translations[i].language = languages[i];
            }
            
            UnityEditor.EditorUtility.SetDirty(this);
        }
        #endif
    }
}
