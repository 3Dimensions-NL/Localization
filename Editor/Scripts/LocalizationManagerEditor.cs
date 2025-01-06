using _3Dimensions.Localization.Runtime.Scripts;
using UnityEditor;
using UnityEngine;
namespace _3Dimensions.Localization.Editor.Scripts
{
    [CustomEditor(typeof(LocalizationManager))]
    public class LocalizationManagerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            // Toon de standaard Inspector (voor velden zoals settings)
            LocalizationManager manager = (LocalizationManager)target;

            // Knop voor ApplyDefaultLanguage
            if (GUILayout.Button("Apply Default Language"))
            {
                manager.ApplyDefaultLanguage();
            }

            // Knop voor ShowLocalCultureInfo
            if (GUILayout.Button("Show Local Culture Info"))
            {
                manager.ShowLocalCultureInfo();
            }

            // Knop voor DeleteLanguagePlayerPref
            if (GUILayout.Button("Delete Language PlayerPref"))
            {
                manager.DeleteLanguagePlayerPref();
            }

            // Toon het LocalizationSettings-object voor overzicht
            if (manager && manager.settings)
            {
                EditorGUILayout.LabelField("Localization Settings", EditorStyles.boldLabel);
                EditorGUILayout.ObjectField("Settings", manager.settings, typeof(LocalizationSettings), false);
            }
            else
            {
                EditorGUILayout.HelpBox("No LocalizationSettings assigned.", MessageType.Warning);
            }
        }
    }
}
