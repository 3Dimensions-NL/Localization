using UnityEngine;

namespace _3Dimensions.Localization.Runtime.Scripts
{
    public class SpritePreviewAttribute : PropertyAttribute
    {
        public readonly int Width;
        public readonly int Height;

        public SpritePreviewAttribute(int width = 60, int height = 60)
        {
            Width = width;
            Height = height;
        }
    }
}