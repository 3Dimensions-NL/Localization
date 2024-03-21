using System;
using Sirenix.OdinInspector;
using UnityEngine;
namespace _3Dimensions.Localization.Runtime.Scripts
{
    [Serializable, InlineEditor]
    public abstract class Translation
    {
        public string Title => language ? language.name : "No language selected";
        [HorizontalGroup("$Title", 102)] [VerticalGroup("$Title/LanguageGroup")] [Title("Language")] [HideLabel] public LanguageObject language;
        [HorizontalGroup("$Title", 102)] [VerticalGroup("$Title/LanguageGroup")] [ShowInInspector] [HideLabel] [PreviewField(100, ObjectFieldAlignment.Left)] public Texture2D Flag => language ? language.Texture : null;
        
        public abstract T GetValue<T>();
    }
}
