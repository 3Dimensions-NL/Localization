using System.Collections.Generic;
using System.IO;
using System.Linq;
using _3Dimensions.Localization.Runtime.Scripts;
using _3Dimensions.Localization.Runtime.Scripts.Translations;
using UnityEditor;
using UnityEngine;

namespace _3Dimensions.Localization.Editor.Scripts
{
    public class TranslationObjectToAssetConverter : EditorWindow
    {
        public static TranslationAsset newAsset;

        [MenuItem("3Dimensions/Localization/Translation Converter")]
        private static void OpenWindow()
        {
            var window = GetWindow<TranslationObjectToAssetConverter>();
            window.titleContent = new GUIContent("Translation Converter");
            window.position = new Rect((Screen.currentResolution.width - 700) / 2,
                (Screen.currentResolution.height - 700) / 2,
                700, 700);
        }

        private void OnGUI()
        {
            // Info box replacement
            EditorGUILayout.HelpBox("Converts TranslationObjects into TranslationAssets.\n" +
                "Unfortunately, the asset references are not replaced in scenes, etc. This is a manual process until resolved.", MessageType.Info);

            // Search button replacement
            if (GUILayout.Button("Search for Translation Objects"))
            {
                SearchForTranslationObjects();
            }

            // Convert button replacement
            if (GUILayout.Button("Convert Translation Objects"))
            {
                ConvertTranslationObjects();
            }
        }

        private static void SearchForTranslationObjects()
        {
            List<TranslationObject> tempList = new(FindAllScriptableObjectsOfType<TranslationObject>("t:TranslationObject"));
            Debug.Log("Found " + tempList.Count + " TranslationObjects that can be converted");
        }

        private static void ConvertTranslationObjects()
        {
            List<TranslationObject> tempList = new(FindAllScriptableObjectsOfType<TranslationObject>("t:TranslationObject"));

            foreach (TranslationObject to in tempList)
            {
                string path = AssetDatabase.GetAssetPath(to);
                Debug.Log("Path of " + to.name + " is: " + path);

                string fileName = Path.GetFileNameWithoutExtension(path);
                string fileExtension = Path.GetExtension(path);
                string newFileName = "string_" + fileName;
                string newPath = Path.Combine(Path.GetDirectoryName(path), newFileName + fileExtension);

                Debug.Log("New path is " + newPath);

                // Check type
                if (!string.IsNullOrEmpty(to.TranslatedText))
                {
                    Debug.Log("Asset was a string translation");

                    TranslationAssetString stringAsset = CreateInstance<TranslationAssetString>();
                    stringAsset.translations = new TranslationAssetString.TranslationString[to.translations.Length];

                    for (int i = 0; i < to.translations.Length; i++)
                    {
                        stringAsset.translations[i] = new TranslationAssetString.TranslationString
                        {
                            language = to.translations[i].language,
                            text = to.translations[i].text
                        };
                    }

                    newAsset = stringAsset;
                    AssetDatabase.CreateAsset(stringAsset, newPath);
                }
                else if (to.TranslatedSprite != null)
                {
                    Debug.Log("Asset was a sprite type");

                    TranslationAssetSprite spriteAsset = CreateInstance<TranslationAssetSprite>();
                    spriteAsset.translations = new TranslationAssetSprite.TranslationSprite[to.translations.Length];

                    for (int i = 0; i < to.translations.Length; i++)
                    {
                        spriteAsset.translations[i] = new TranslationAssetSprite.TranslationSprite
                        {
                            language = to.translations[i].language,
                            sprite = to.translations[i].sprite
                        };
                    }

                    newAsset = spriteAsset;
                    AssetDatabase.CreateAsset(spriteAsset, newPath);
                }
            }
        }

        public static List<T> FindAllScriptableObjectsOfType<T>(string filter) where T : ScriptableObject
        {
            return AssetDatabase.FindAssets(filter, new[]
                {
                    "Assets/"
                })
                .Select(guid => AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(guid)))
                .ToList();
        }
    }
}
