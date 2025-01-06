using _3Dimensions.Localization.Runtime.Scripts;
using UnityEditor;
using UnityEngine;

namespace _3Dimensions.Localization.Editor.Scripts
{
    [CustomPropertyDrawer(typeof(SpritePreviewAttribute))]
    public class SpritePreviewDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Zorg ervoor dat dit alleen werkt met object-references zoals Sprite
            if (property.propertyType != SerializedPropertyType.ObjectReference)
            {
                EditorGUI.LabelField(position, label.text, "Use SpritePreview with Sprite fields only.");
                return;
            }

            // Krijg de SpritePreview-attribuut instellingen
            SpritePreviewAttribute spritePreview = (SpritePreviewAttribute)attribute;

            // Bereken de rect voor het sprite preview-veld
            Rect spriteRect = new Rect(position.x, position.y, spritePreview.Width, spritePreview.Height);

            // Controleer of een sprite aanwezig is en teken de afbeelding of een fallback
            if (property.objectReferenceValue != null && property.objectReferenceValue is Sprite sprite)
            {
                // Teken de sprite preview
                EditorGUI.DrawPreviewTexture(spriteRect, AssetPreview.GetAssetPreview(sprite.texture));
            }
            else
            {
                // Geen sprite geselecteerd, teken lege box met fallback tekst
                EditorGUI.HelpBox(spriteRect, "No Sprite Assigned", MessageType.Info);
            }

            // Maak de preview interactief (klikbaar om een sprite-object te selecteren)
            property.objectReferenceValue = EditorGUI.ObjectField(
                spriteRect,
                property.objectReferenceValue,
                typeof(Sprite),
                false
            );
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            SpritePreviewAttribute spritePreview = (SpritePreviewAttribute)attribute;

            // Geef alleen de ruimte voor de sprite-preview (geen extra ruimte voor standaard rendering)
            return spritePreview.Height;
        }
    }
}