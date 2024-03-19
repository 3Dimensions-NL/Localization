using UnityEngine;
namespace _3Dimensions.Localization.Runtime.Scripts
{
    public class LocalizationCanvas : MonoBehaviour
    {
        [SerializeField] private GameObject languageSelectButtonPrefab;
        [SerializeField] private Transform selectButtonsContainer;

        private void OnEnable()
        {
            while (selectButtonsContainer.childCount > 0)
            {
                DestroyImmediate(selectButtonsContainer.GetChild(0).gameObject);
            }
            
            foreach (LanguageObject language in LocalizationManager.Instance.AvailableLanguage)
            {
                GameObject buttonGo = Instantiate(languageSelectButtonPrefab, selectButtonsContainer);
                buttonGo.GetComponent<LocalizationSelectionButton>().Setup(language);
            }
        }
    }
}