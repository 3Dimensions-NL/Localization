using System;
using UnityEngine;
namespace _3Dimensions.Localization.Runtime.Scripts
{
    [Serializable, Obsolete("Use TranslationAsset instead")]
    public class TranslationObject : ScriptableObject
    {
        public string Title => this.name;

        public Translation[] translations;
        
        [Serializable]
        public struct Translation
        {
            public string Title => language ? language.name : "No language selected";
            public LanguageObject language;
            public Texture2D Flag => language ? language.Texture : null;
            public string text;
            public Sprite sprite;
        }

        public string TranslatedText
        {
            get
            {
                foreach (Translation translation in translations)
                {
                    if (translation.language == LocalizationManager.CurrentLanguage) return translation.text;
                }

                return null;
            }
        }

        public Sprite TranslatedSprite
        {
            get
            {
                foreach (Translation translation in translations)
                {
                    if (translation.language == LocalizationManager.CurrentLanguage) return translation.sprite;
                }

                return null;
            }
        }
    }
}