using System.Globalization;
using UnityEngine;
namespace _3Dimensions.Localization.Runtime.Scripts
{
    [CreateAssetMenu(fileName = "New Language", menuName = "3Dimensions/Localization/New Language")]
    public class LanguageObject : ScriptableObject
    {
        public Sprite flag;
        public Texture2D Texture => ConvertSpriteToTexture(flag); 
        public string[] cultures;
        public CultureInfo[] Test;
        
        private static Texture2D ConvertSpriteToTexture(Sprite sprite)
        {
            try
            {
                if (sprite.rect.width != sprite.texture.width)
                {
                    Texture2D newText = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
                    Color[] colors = newText.GetPixels();
                    Color[] newColors = sprite.texture.GetPixels((int)System.Math.Ceiling(sprite.textureRect.x),
                        (int)System.Math.Ceiling(sprite.textureRect.y),
                        (int)System.Math.Ceiling(sprite.textureRect.width),
                        (int)System.Math.Ceiling(sprite.textureRect.height));
                    Debug.Log(colors.Length+"_"+ newColors.Length);
                    newText.SetPixels(newColors);
                    newText.Apply();
                    return newText;
                }
                return sprite.texture;
            }
            catch
            {
                return sprite.texture;
            }
        }

        /// <summary>
        /// Method to test the culture info strings used by the editor script.
        /// </summary>
        private void TestCultureStrings()
        {
            foreach (string culture in cultures)
            {
                Debug.Log(CultureInfo.GetCultureInfo(culture).EnglishName);
            }
        }
    }
}
