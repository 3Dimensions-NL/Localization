using System;
using UnityEngine;
namespace _3Dimensions.Localization.Runtime.Scripts
{
    [Serializable]
    public abstract class TranslationAsset : ScriptableObject
    {
        public abstract T GetValue<T>();

    }
}