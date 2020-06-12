using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using AGAC.DialogueSystem.Base;

namespace AGAC.DialogueSystem.CustomEditors
{
    [CustomPropertyDrawer(typeof(DS_Dialogue))]
    public class DS_DialogueDrawer : PropertyDrawer
    {
        #region DRAWER METHODS
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            fieldPosition = position;
            EditorGUI.BeginProperty(fieldPosition, label, property);
            EditorGUIUtility.labelWidth = 100;
            int indent = EditorGUI.indentLevel;

            SetVariables(property);
            fieldPosition = position;
            fieldPosition.height = 20;

            DrawVariableHeader(label);
            EditorGUI.indentLevel++;
            Space(0);
            DrawVariables();

            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (property == null)
                return 30;

            SerializedProperty _displayed = property.FindPropertyRelative("displayed");
            if (_displayed == null)
                return 30;
            else if (_displayed.boolValue)
                return GetHeightForDisplayedVariables();
            return 30;
        }
        #endregion

        #region VARIABLES
        private GUIStyle TextAreaStyle;
        private SerializedProperty displayed;
        private SerializedProperty type;
        private SerializedProperty text;
        private SerializedProperty audioClip;
        private Rect fieldPosition;
        #endregion

        #region PRIVATE METHODS
        #region draw methods
        private void DrawVariables() 
        {
            if (!displayed.boolValue) return;
            DrawTypeOptions();
            Space(6);
            DrawOptionAccordingly();
            Space(12);
        }
        private void DrawVariableHeader(GUIContent label)
        {
            displayed.boolValue = EditorGUI.Foldout(fieldPosition, displayed.boolValue, label);
        }
        private void DrawTypeOptions() 
        {
            int _type = type.intValue;
            _type = (int)(DialogueTypeOption)EditorGUI.EnumPopup(fieldPosition, new GUIContent("Type"), (DialogueTypeOption)_type);
            type.intValue = _type;
        }
        private void DrawOptionAccordingly() 
        {
            DialogueTypeOption selectedOption = (DialogueTypeOption)type.intValue;
            switch (selectedOption) 
            {
                case DialogueTypeOption.TEXT:
                    DrawOptionText(); 
                    return;
                case DialogueTypeOption.AUDIO: 
                    DrawOptionAudio(); 
                    return;
                case DialogueTypeOption.TEXT_n_AUDIO: 
                    DrawOptionText();
                    Space(8);
                    DrawOptionAudio(); 
                    return;
                default:return;
            }
        }
        private void DrawOptionText() 
        {
            int visibleLines = 4;
            fieldPosition.height = 14 * visibleLines;
            GUIContent label = new GUIContent("Text");
            DrawTextArea(fieldPosition, text, label);
        }
        private void DrawOptionAudio() 
        {
            fieldPosition.height = 20;
            GUIContent label = new GUIContent("Audio");
            EditorGUI.PropertyField(fieldPosition, audioClip, label);
        }
        private void DrawTextArea(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.LabelField(position, label);
            position.x += (100- (14*EditorGUI.indentLevel));
            position.width -= 100;

            property.stringValue = EditorGUI.TextArea(position, property.stringValue, TextAreaStyle);
        }
        #endregion

        private float GetHeightForDisplayedVariables() 
        {
            if (type == null)
                return 100;

            switch ((DialogueTypeOption)type.intValue)
            {
                case DialogueTypeOption.TEXT: return 110;
                case DialogueTypeOption.AUDIO: return 80;
                case DialogueTypeOption.TEXT_n_AUDIO: return 140;
                default: return 100;
            }
        }
        private void Space(int pixels)
        {
            fieldPosition.y += fieldPosition.height + pixels;
        }
        private void SetVariables(SerializedProperty dialogue) 
        {
            displayed = dialogue.FindPropertyRelative("displayed");
            type = dialogue.FindPropertyRelative("type");
            text = dialogue.FindPropertyRelative("text");
            audioClip = dialogue.FindPropertyRelative("audioClip");

            TextAreaStyle = new GUIStyle(EditorStyles.textArea);
            TextAreaStyle.wordWrap = true;
        }
        #endregion
    }
}