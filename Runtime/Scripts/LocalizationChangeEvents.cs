using UnityEngine;
using UnityEngine.Events;
namespace _3Dimensions.Localization.Runtime.Scripts
{
    public class LocalizationChangeEvents : MonoBehaviour
    {
        public UnityEvent<LanguageObject> onLanguageSet;
        
        private void OnEnable()
        {
            LocalizationManager.NewLanguageSetEvent += LocalizationManagerOnNewLanguageSetEvent;
        }

        private void OnDisable()
        {
            LocalizationManager.NewLanguageSetEvent += LocalizationManagerOnNewLanguageSetEvent;
        }
        
        private void LocalizationManagerOnNewLanguageSetEvent(LanguageObject newLanguage)
        {
            onLanguageSet?.Invoke(newLanguage);
        }
    }
}
