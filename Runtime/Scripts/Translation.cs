using System;
using UnityEngine;
namespace _3Dimensions.Localization.Runtime.Scripts
{
    [Serializable]
    public abstract class Translation
    {
        public LanguageObject language;
        public Texture2D Flag => language ? language.Texture : null;
        
        public abstract T GetValue<T>();
    }
}
