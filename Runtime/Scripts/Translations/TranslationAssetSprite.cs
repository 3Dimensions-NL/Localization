using System;
using Sirenix.OdinInspector;
using UnityEngine;
namespace _3Dimensions.Localization.Runtime.Scripts.Translations
{
    [CreateAssetMenu(fileName = "New Sprite Translation", menuName = "3Dimensions/Localization/New Sprite Translation")]
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
    }
}
