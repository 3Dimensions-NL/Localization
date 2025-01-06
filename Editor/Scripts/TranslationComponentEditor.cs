using _3Dimensions.Localization.Runtime.Scripts;
using UnityEditor;
using UnityEngine;

namespace _3Dimensions.Localization.Editor.Scripts
{
    [CustomEditor(typeof(TranslationComponent))]
    public class TranslationComponentEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            // Draw the default inspector layout
            DrawDefaultInspector();

            // Reference to the target TranslationComponent
            TranslationComponent translationComponent = (TranslationComponent)target;

            // Add a space between default inspector and buttons
            EditorGUILayout.Space();

            // Add a button to call ApplyTranslation method
            if (GUILayout.Button("Apply Translation"))
            {
                translationComponent.ApplyTranslation(); // Calls the method
            }

            // Add another button to call SwitchTranslation method
            if (GUILayout.Button("Switch Translation"))
            {
                translationComponent.SwitchTranslation(); // Calls the method
            }
        }
    }
}
