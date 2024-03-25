using System;
using Sirenix.OdinInspector;
using UnityEngine;
namespace _3Dimensions.Localization.Runtime.Scripts
{
    [Serializable, InlineEditor]
    public class TranslationObject : ScriptableObject
    {
        public string Title => this.name;

        [Title("$Title", TitleAlignment = TitleAlignments.Centered, HorizontalLine = false), ListDrawerSettings(ShowFoldout = false)]
        public Translation[] translations;
        
        [Serializable, InlineEditor]
        public struct Translation
        {
            public string Title => language ? language.name : "No language selected";
            [HorizontalGroup("$Title", 102)] [VerticalGroup("$Title/LanguageGroup")] [Title("Language")] [HideLabel] public LanguageObject language;
            [HorizontalGroup("$Title", 102)] [VerticalGroup("$Title/LanguageGroup")] [ShowInInspector] [HideLabel] [PreviewField(100, ObjectFieldAlignment.Left)] public Texture2D Flag => language ? language.Texture : null;
            [HorizontalGroup("$Title")] [Title("Text")] [TextArea(3,10)] [HideLabel]public string text;
            [HorizontalGroup("$Title", 100)] [Title("Sprite")] [HideLabel] [PreviewField(100, ObjectFieldAlignment.Right)] public Sprite sprite;
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