using System;
using _3Dimensions.Localization.Runtime.Scripts.Translations;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace _3Dimensions.Localization.Runtime.Scripts
{
    public class TranslationComponent : MonoBehaviour
    {
        [SerializeField] private TranslationAsset translationAsset;
        private void Start()
        {
            ApplyTranslation();
        }

        [Button]
        public void ApplyTranslation()
        {
            Type type = translationAsset.GetType();
            
            if (type == typeof(TranslationAssetString))
            {
                TMP_Text textMesh = GetComponent<TMP_Text>();
                if (textMesh)
                {
                    textMesh.text = translationAsset.GetValue<string>();
                }
            
                Text legacyText = GetComponent<Text>();
                if (legacyText)
                {
                    legacyText.text = translationAsset.GetValue<string>();
                }

                TMP_InputField textMeshInputField = GetComponent<TMP_InputField>();
                if (textMeshInputField)
                {
                    textMeshInputField.text = translationAsset.GetValue<string>();
                }

                return;
            }

            if (type == typeof(TranslationAssetSprite))
            {
                Sprite sprite = translationAsset.GetValue<Sprite>();
                if (sprite == null) return;
                Image image = GetComponent<Image>();
                if (image)
                {
                    image.sprite = sprite;
                }
            }
            
            
        }

        [Button]
        public void SwitchTranslation()
        {
            Type type = translationAsset.GetType();

            if (type == typeof(TranslationAssetString))
            {
                TranslationAssetString translationString = translationAsset as TranslationAssetString;
                if (translationAsset == null) return;
                
                TMP_Text textMesh = GetComponent<TMP_Text>();
                if (textMesh)
                {
                    
                    
                    int currentIndex = 0;

                    for (int i = 0; i < translationString.translations.Length; i++)
                    {
                        if (textMesh.text == translationString.translations[i].GetValue<string>())
                        {
                            //Check for end of array, get next translation index
                            currentIndex = i;
                            currentIndex = currentIndex >= translationString.translations.Length - 1 ? 0 : currentIndex + 1;
                        }
                    }
                
                    textMesh.text = translationString.translations[currentIndex].GetValue<string>();                }
            
                Text legacyText = GetComponent<Text>();
                if (legacyText)
                {
                    int currentIndex = 0;

                    for (int i = 0; i < translationString.translations.Length; i++)
                    {
                        if (legacyText.text == translationString.translations[i].GetValue<string>())
                        {
                            currentIndex = i;
                            currentIndex = currentIndex >= translationString.translations.Length - 1 ? 0 : currentIndex + 1;
                        }
                    }

                    legacyText.text = translationString.translations[currentIndex].GetValue<string>();
                }

                TMP_InputField textMeshInputField = GetComponent<TMP_InputField>();
                if (textMeshInputField)
                {
                    int currentIndex = 0;

                    for (int i = 0; i < translationString.translations.Length; i++)
                    {
                        if (textMeshInputField.text == translationString.translations[i].GetValue<string>())
                        {
                            currentIndex = i;
                            currentIndex = currentIndex >= translationString.translations.Length - 1 ? 0 : currentIndex + 1;
                        }
                    }

                    textMeshInputField.text = translationString.translations[currentIndex].GetValue<string>();
                }

                return;
            }

            if (type == typeof(TranslationAssetSprite))
            {
                TranslationAssetSprite translationSprite = translationAsset as TranslationAssetSprite;
                if (translationSprite == null) return;
                
                Sprite sprite = translationAsset.GetValue<Sprite>();
                if (sprite == null) return;
                
                Image image = GetComponent<Image>();
                if (image)
                {
                    int currentIndex = 0;

                    for (int i = 0; i < translationSprite.translations.Length; i++)
                    {
                        if (image.sprite == translationSprite.translations[i].GetValue<Sprite>())
                        {
                            currentIndex = i;
                            currentIndex = currentIndex >= translationSprite.translations.Length - 1 ? 0 : currentIndex + 1;
                        }
                    }

                    if (translationSprite.translations[currentIndex].GetValue<Sprite>() != null)
                    {
                        image.sprite = translationSprite.translations[currentIndex].GetValue<Sprite>();
                    }
                }
            }
        }
    }
}