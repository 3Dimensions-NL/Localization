using _3Dimensions.Localization.Runtime.Scripts;
using UnityEditor;
using UnityEngine;

namespace _3Dimensions.Localization.Editor.Scripts
{
    [CustomEditor(typeof(LanguageObject))]
    public class LanguageObjectEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            // Haal de target (LanguageObject) op
            LanguageObject languageObject = (LanguageObject)target;

            // Toon een aanpasbare sprite voor 'flag'
            GUILayout.Label("Flag Preview", EditorStyles.boldLabel);

            // Toon het sprite veld (ook als het leeg is)
            languageObject.flag = (Sprite)EditorGUILayout.ObjectField("Flag", languageObject.flag, typeof(Sprite), false);

            // Toon de sprite-preview (indien ingesteld)
            if (languageObject.flag != null)
            {
                Texture2D texture = languageObject.flag.texture;

                if (texture != null)
                {
                    // Bepaal aspect ratio om vervorming te voorkomen
                    float aspectRatio = (float)texture.width / texture.height;

                    // Stel grootte in voor de preview
                    float previewWidth = 200;
                    float previewHeight = previewWidth / aspectRatio;

                    // Toon een texture-preview
                    Rect rect = GUILayoutUtility.GetRect(previewWidth, previewHeight, GUILayout.ExpandWidth(false));
                    EditorGUI.DrawPreviewTexture(rect, texture, null, ScaleMode.ScaleToFit);
                }
            }
            else
            {
                GUILayout.Label("No flag selected.", EditorStyles.centeredGreyMiniLabel);
            }

            GUILayout.Space(15);

            // Toon 'cultures' array
            GUILayout.Label("Cultures", EditorStyles.boldLabel);
            SerializedProperty culturesProperty = serializedObject.FindProperty("cultures");

            // Zorg ervoor dat het editable is via de standaard Unity tools
            EditorGUILayout.PropertyField(culturesProperty, true);

            // Herschrijf het object zodat wijzigingen worden opgeslagen
            serializedObject.ApplyModifiedProperties();

            GUILayout.Space(15);

            // Toon een knop specifiek voor de TestCultureStrings-methode
            if (GUILayout.Button("Test Culture Strings"))
            {
                // Roep de methode TestCultureStrings aan via Reflection
                typeof(LanguageObject).GetMethod("TestCultureStrings",
                        System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                    ?.Invoke(languageObject, null);
            }
        }
    }
}
