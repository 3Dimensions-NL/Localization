using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace _3Dimensions.Localization.Runtime.Scripts
{
    public class TranslationComponent : MonoBehaviour
    {
        [SerializeField] private TranslationObject translationObject;
        private void Start()
        {
            ApplyTranslation();
        }

        [Button]
        public void ApplyTranslation()
        {
            TMP_Text textMesh = GetComponent<TMP_Text>();
            if (textMesh)
            {
                textMesh.text = translationObject.TranslatedText;
            }
            
            Text legacyText = GetComponent<Text>();
            if (legacyText)
            {
                legacyText.text = translationObject.TranslatedText;
            }

            TMP_InputField textMeshInputField = GetComponent<TMP_InputField>();
            if (textMeshInputField)
            {
                textMeshInputField.text = translationObject.TranslatedText;
            }

            if (translationObject.TranslatedSprite == null) return;
            Image image = GetComponent<Image>();
            if (image)
            {
                image.sprite = translationObject.TranslatedSprite;
            }
        }

        [Button]
        public void SwitchTranslation()
        {
            TMP_Text textMesh = GetComponent<TMP_Text>();

            if (textMesh)
            {
                int currentIndex = 0;

                for (int i = 0; i < translationObject.translations.Length; i++)
                {
                    if (textMesh.text == translationObject.translations[i].text)
                    {
                        //Check for end of array, get next translation index
                        currentIndex = i;
                        currentIndex = currentIndex >= translationObject.translations.Length - 1 ? 0 : currentIndex + 1;
                    }
                }
                
                textMesh.text = translationObject.translations[currentIndex].text;
            }
            
            Text legacyText = GetComponent<Text>();
            if (legacyText)
            {
                int currentIndex = 0;

                for (int i = 0; i < translationObject.translations.Length; i++)
                {
                    if (legacyText.text == translationObject.translations[i].text)
                    {
                        currentIndex = i;
                        currentIndex = currentIndex >= translationObject.translations.Length - 1 ? 0 : currentIndex + 1;
                    }
                }

                legacyText.text = translationObject.translations[currentIndex].text;
            }
            
            TMP_InputField textMeshInputField = GetComponent<TMP_InputField>();
            if (textMeshInputField)
            {
                int currentIndex = 0;

                for (int i = 0; i < translationObject.translations.Length; i++)
                {
                    if (textMeshInputField.text == translationObject.translations[i].text)
                    {
                        currentIndex = i;
                        currentIndex = currentIndex >= translationObject.translations.Length - 1 ? 0 : currentIndex + 1;
                    }
                }

                textMeshInputField.text = translationObject.translations[currentIndex].text;
            }
            
            Image image = GetComponent<Image>();
            if (image)
            {
                int currentIndex = 0;

                for (int i = 0; i < translationObject.translations.Length; i++)
                {
                    if (image.sprite == translationObject.translations[i].sprite)
                    {
                        currentIndex = i;
                        currentIndex = currentIndex >= translationObject.translations.Length - 1 ? 0 : currentIndex + 1;
                    }
                }

                if (translationObject.translations[currentIndex].sprite != null)
                {
                    image.sprite = translationObject.translations[currentIndex].sprite;
                }
            }
        }
    }
}