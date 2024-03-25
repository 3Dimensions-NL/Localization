using System;
using Sirenix.OdinInspector;
using UnityEngine;
namespace _3Dimensions.Localization.Runtime.Scripts.Translations
{
    [CreateAssetMenu(fileName = "New Audio Clip Translation", menuName = "3Dimensions/Localization/New Translation Audio Clip")]
    [Serializable, InlineEditor]
    public class TranslationAssetAudioClip : TranslationAsset
    {
        public TranslationAudioClip[] translations;

        
        [Serializable, InlineEditor]
        public class TranslationAudioClip: Translation
        {
            public AudioClip clip;
            
            public override T GetValue<T>()
            {
                return (T) Convert.ChangeType(clip, typeof(T));
            }
        }
        
        public override T GetValue<T>()
        {
            foreach (TranslationAudioClip translation in translations)
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
            translations = new TranslationAudioClip[languages.Length];
                
            for (int i = 0; i < languages.Length; i++)
            {
                translations[i] = new TranslationAudioClip();
                translations[i].language = languages[i];
            }
            
            UnityEditor.EditorUtility.SetDirty(this);
        }
        #endif
    }
}
