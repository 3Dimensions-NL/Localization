using System;
using Sirenix.OdinInspector;
using UnityEngine;
namespace _3Dimensions.Localization.Runtime.Scripts.Translations
{
    [CreateAssetMenu(fileName = "New Sprite Translation", menuName = "3Dimensions/Localization/New Translation Sprite")]
    [Serializable, InlineEditor]
    public class TranslationAssetSprite : TranslationAsset
    {
        public TranslationSprite[] translations;
        
        [Serializable, InlineEditor]
        public class TranslationSprite : Translation
        {
            public Sprite sprite;
        
            public override T GetValue<T>()
            {
                return (T) Convert.ChangeType(sprite, typeof(T));
            }
        }
        
        public override T GetValue<T>()
        {
            foreach (TranslationSprite translation in translations)
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
            translations = new TranslationSprite[languages.Length];
                
            for (int i = 0; i < languages.Length; i++)
            {
                translations[i] = new TranslationSprite();
                translations[i].language = languages[i];
            }
            
            UnityEditor.EditorUtility.SetDirty(this);
        }
        #endif
    }
}
