using _3Dimensions.Localization.Runtime.Scripts;
using TMPro;
using UnityEngine;
using UnityEditor;
namespace _3Dimensions.Localization.Editor.Scripts
{
    public static class TranslationContextMenu
    {
        [MenuItem("CONTEXT/TextMeshProUGUI/Add Translation Component")]
        private static void AddTranslationComponentToUGUI(MenuCommand command)
        {
            AddTranslationComponent((TMP_Text)command.context);
        }

        [MenuItem("CONTEXT/TextMeshPro/Add Translation Component")]
        private static void AddTranslationComponentTo3D(MenuCommand command)
        {
            AddTranslationComponent((TMP_Text)command.context);
        }

        private static void AddTranslationComponent(TMP_Text tmpText)
        {
            GameObject go = tmpText.gameObject;

            if (go.GetComponent<TranslationComponent>() == null)
            {
                var component = go.AddComponent<TranslationComponent>();
                EditorUtility.SetDirty(component);
                Debug.Log("TranslationComponent added to GameObject: " + go.name);
            }
            else
            {
                Debug.LogWarning("TranslationComponent already exists on GameObject: " + go.name);
            }
        }

        [MenuItem("GameObject/Localization/Add TranslationComponent", false, 10)]
        private static void AddTranslationComponentFromMenu(MenuCommand command)
        {
            var go = Selection.activeGameObject;
            if (go == null) return;

            if (go.GetComponent<TranslationComponent>() == null)
            {
                var tc = go.AddComponent<TranslationComponent>();
                EditorUtility.SetDirty(tc);
            }
            else
            {
                Debug.LogWarning("TranslationComponent already exists on GameObject.");
            }
        }

    }
}
