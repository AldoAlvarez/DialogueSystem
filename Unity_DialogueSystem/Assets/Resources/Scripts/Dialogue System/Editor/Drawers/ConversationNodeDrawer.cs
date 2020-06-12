using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using AGAC.DialogueSystem.Base;
using AGAC.DialogueSystem.Tools;
using AGAC.General;

namespace AGAC.DialogueSystem.CustomEditors
{
    [CustomPropertyDrawer(typeof(DS_ConversationNode))]
    public class ConversationNodeDrawer : PropertyDrawer
    {
        #region EDITOR METHODS
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            SetNodeVariables(property);
            CreateGUIStyles();

            DrawBackground(position);
            position.y += 10;
            DrawIcon(position);
            DrawDialogueText(position);
            DrawSelectionButton(position);
            EditorGUI.EndProperty();
        }
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return height;
        }
        #endregion

        #region VARIABLES
        private const int height = 80;
        private SerializedProperty character;
        private SerializedProperty sequenceIndex;
        private SerializedProperty dialogueIndex;
        private SerializedProperty scrollerPos;
        private SerializedProperty selected;

        private SerializedObject ScriptableCharacter;
        private DS_Character ClassCharacter;

        private GUIStyle backgroundStyle;
        private GUIStyle dialogueTextStyle;
        private GUIStyle selectButton;
        #endregion

        #region PRIVATE METHODS
        #region draw methods
        private void DrawBackground(Rect position) 
        {
            position.width = 320;
            position.height = height;
            GUI.Box(position, GUIContent.none, backgroundStyle);
        }
        private void DrawIcon(Rect position) 
        {
            Texture icon = AssetPreview.GetAssetPreview(ClassCharacter.Icon);
            position.x += 10;
            position.width = 40;
            position.height = 40;
            GUI.Label(position, icon);
        }
        private void DrawDialogueText(Rect position) 
        {
            int iconLenght = 46;
            position.x += 10 + iconLenght;
            Rect scrollPosition = new Rect(position.x, position.y, 215, 60);
            GUIContent dialogue = new GUIContent(GetTextDialogue());
            DrawDialogueAsScrollView(scrollPosition, dialogue);
        }
        private void DrawDialogueAsScrollView(Rect position, GUIContent dialogue) 
        {
            float labelHeight = dialogueTextStyle.CalcHeight(dialogue, 230);
            Rect viewRect = new Rect(position.x, position.y, 210, labelHeight);
            viewRect.y -= scrollerPos.vector2Value.y;

            scrollerPos.vector2Value =
                GUI.BeginScrollView(
                    position,
                    scrollerPos.vector2Value,
                    viewRect,
                    false, false,
                    GUIStyle.none,
                    GUI.skin.verticalSlider);
            GUI.Label(viewRect, dialogue, dialogueTextStyle);
            GUI.EndScrollView(false);
        }
        private void DrawSelectionButton(Rect position) 
        {
            position.x += 280;
            position.y += 15;
            position.width = 24;
            position.height = 24;
            selected.boolValue = GUI.Toggle(position, selected.boolValue, GUIContent.none, selectButton);
        }
        #endregion

        private string GetTextDialogue() 
        {
            DS_Dialogue dialogue = ClassCharacter.Sequences[sequenceIndex.intValue][(uint)dialogueIndex.intValue];
            return dialogue.GetText();
        }

        #region initialization
        private void SetNodeVariables(SerializedProperty node) 
        {
            character = node.FindPropertyRelative("character");
            sequenceIndex = node.FindPropertyRelative("sequenceIndex");
            dialogueIndex = node.FindPropertyRelative("dialogueIndex");
            scrollerPos = node.FindPropertyRelative("scrollerPos");
            selected = node.FindPropertyRelative("selected");

            ScriptableCharacter = new SerializedObject(character.objectReferenceValue);
            ClassCharacter = (DS_Character)ScriptableCharacter.targetObject;
        }
        private void CreateGUIStyles()
        {
            backgroundStyle = new GUIStyle(GUI.skin.box);
            Texture2D backgroundTex = GeneralMethods.GetNewTexture(ClassCharacter.IDcolor);
            backgroundStyle.normal.background = backgroundTex;

            if (dialogueTextStyle == null) 
            {
                dialogueTextStyle = new GUIStyle(GUI.skin.label);
                dialogueTextStyle.wordWrap = true;
                dialogueTextStyle.fontSize = 16;
                dialogueTextStyle.fontStyle = FontStyle.Normal;
            }
            if (selectButton == null) 
            {
                selectButton = new GUIStyle(GUI.skin.toggle);
                Texture2D off = Resources.Load<Texture2D>("Dialogue System/Icons/radio_button_off");
                Texture2D on = Resources.Load<Texture2D>("Dialogue System/Icons/radio_button_on");

                selectButton.normal.background = off;
                selectButton.onNormal.background = on;
                selectButton.active.background = on;
                selectButton.onActive.background = on;
            }
        }
        #endregion
        #endregion
    }
}