using System;
using Sirenix.OdinInspector;
using UnityEngine;
namespace _3Dimensions.Localization.Runtime.Scripts
{
    [Serializable, InlineEditor]
    public abstract class TranslationAsset : ScriptableObject
    {
        public string Title => this.name;

        [Title("$Title", TitleAlignment = TitleAlignments.Centered, HorizontalLine = false), ListDrawerSettings(ShowFoldout = false)]
        
        public abstract T GetValue<T>();

    }
}