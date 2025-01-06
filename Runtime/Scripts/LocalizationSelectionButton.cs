using UnityEngine;
using UnityEngine.UI;

namespace _3Dimensions.Localization.Runtime.Scripts
{
    public class LocalizationSelectionButton : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private Image image;
        public LanguageObject language;

        private void OnEnable()
        {
            button.onClick.AddListener(Clicked);
        }

        private void Clicked()
        {
            LocalizationManager.SetCurrentLanguage(language);
        }

        public void Setup(LanguageObject newLanguage)
        {
            language = newLanguage;
            image.sprite = language.flag;
        }
    }
}
