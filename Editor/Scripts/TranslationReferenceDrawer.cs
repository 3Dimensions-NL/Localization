using _3Dimensions.Localization.Runtime.Scripts;
using UnityEditor;
using UnityEngine;
namespace _3Dimensions.Localization.Editor.Scripts
{
    [CustomPropertyDrawer(typeof(TranslationReferenceAttribute))]
    public class TranslationReferenceDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Draw the property field
            EditorGUI.BeginProperty(position, label, property);

            var newValue = EditorGUI.ObjectField(position, label, property.objectReferenceValue, typeof(TranslationAsset), false);

            if (newValue != property.objectReferenceValue)
            {
                property.objectReferenceValue = newValue;
                property.serializedObject.ApplyModifiedProperties();

                // Check if we're editing a GameObject with a TranslationComponent
                if (property.serializedObject.targetObject is Component component)
                {
                    var go = component.gameObject;
                    var tc = go.GetComponent<TranslationComponent>();
                    if (tc == null)
                    {
                        tc = go.AddComponent<TranslationComponent>();
                    }

                    tc.translationAsset = newValue as TranslationAsset;
                    EditorUtility.SetDirty(tc);
                }
            }

            EditorGUI.EndProperty();
        }
    }
}
