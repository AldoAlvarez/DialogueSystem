using UnityEngine;

namespace AGAC.DialogueSystem.CustomEditors
{
    public struct GUIStyleCreationOption
    {
        public GUIStyleCreationOption(Color backgroundColor, int fontSize = 10, FontStyle fontStyle = FontStyle.Normal)
        {
            this.backgroundColor = backgroundColor;
            this.fontSize = fontSize;
            this.fontStyle = fontStyle;
        }
        public readonly Color backgroundColor;
        public readonly int fontSize;
        public readonly FontStyle fontStyle;
    }
}