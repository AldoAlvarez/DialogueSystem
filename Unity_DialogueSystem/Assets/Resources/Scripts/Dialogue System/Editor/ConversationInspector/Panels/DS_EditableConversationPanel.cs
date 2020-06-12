using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using AGAC.General;
using AGAC.DialogueSystem.CustomEditors.Scrollers;
using AGAC.DialogueSystem.Base;
using System.Linq;

namespace AGAC.DialogueSystem.CustomEditors.Panels
{
    public class DS_EditableConversationPanel
    {
        public DS_EditableConversationPanel(OnPanelLeave OnLeave) 
        {
            this.OnLeave = OnLeave;
            dialoguesDisplay = new CharactersDialoguesDisplay(240, 500);
            dialogueManipulation = new DialogueManipulationScroller(300, 500);
        }

        public void Initialize(SerializedObject conversation)
        {
            Setvariables(conversation);
            dialoguesDisplay.Initialize(conversation, dialogueManipulation.SetScrollerToEnd);
            dialogueManipulation.Initialize(conversation);
        }

        public void OnGUI()
        {
            CreateGUIStyles();
            if(ScriptableConversation !=null)
                ScriptableConversation.UpdateIfRequiredOrScript();

            GUILayout.Space(60);
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(20);
            dialoguesDisplay.OnGUI();
            GUILayout.Space(20);
            dialogueManipulation.OnGUI();
            GUILayout.Space(20);
            DrawControllerButtons();
            EditorGUILayout.EndHorizontal();
            if (ScriptableConversation != null)
                ScriptableConversation.ApplyModifiedProperties();
        }

        #region VARIABLES
        private Vector2 dialoguesScroller;
        private GUIStyle dialoguesBackground;

        private OnPanelLeave OnLeave;
        private CharactersDialoguesDisplay dialoguesDisplay;
        private DialogueManipulationScroller dialogueManipulation;

        private SerializedObject ScriptableConversation;
        private DS_Conversation ClassConversation;
        private SerializedProperty node_sequence;

        private delegate void OnButtonClick();
        private GUIStyle ControllersBackground;
        private GUIStyle LeaveButton;
        private GUIStyle MoveUpButton;
        private GUIStyle MoveDownButton;
        private GUIStyle RemoveButton;
        private GUIStyle separationLine;
        private const int separationSpace = 20;
        #endregion

        #region PRIVATE METHODS
        private void DrawControllerButtons() 
        {
            EditorGUILayout.BeginVertical(ControllersBackground, GUILayout.Width(60));
            GUILayout.Space(separationSpace);
            DrawButton(LeaveConversation, LeaveButton, GUILayout.Width(38), GUILayout.Height(38));
            DrawSeparation(58);

            DrawButton(MoveUpSelectedElements, MoveUpButton, GUILayout.Width(38), GUILayout.Height(38));
            GUILayout.Space(separationSpace);
            DrawButton(MoveDownSelectedElements, MoveDownButton, GUILayout.Width(38), GUILayout.Height(38));
            DrawSeparation(58);

            DrawButton(RemoveSelectedElements, RemoveButton, GUILayout.Width(38), GUILayout.Height(38));
            GUILayout.Space(separationSpace);
            EditorGUILayout.EndVertical();
        }
        private void DrawButton(OnButtonClick OnClick, GUIStyle style = null, params GUILayoutOption[] options) 
        {
            if (style == null)
                style = new GUIStyle(GUI.skin.button);

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(11);
            if (GUILayout.Button(GUIContent.none, style, options))
                OnClick();
            EditorGUILayout.EndHorizontal();
        }
        private void DrawSeparation(int width) 
        {
            GUILayout.Space(separationSpace/2);
            GUILayout.Label(GUIContent.none, separationLine, GUILayout.Width(width), GUILayout.Height(5));
            GUILayout.Space(separationSpace/2);
        }
        private void LeaveConversation() 
        {
            if (ClassConversation != null)
                ClassConversation.UnSelectAllDialogues();
            OnLeave(); 
        }

        private void RemoveSelectedElements() 
        {
            ClassConversation.RemoveSelectedDialogues();
        }
        private void MoveUpSelectedElements() 
        {
            ClassConversation.MoveUpSelectedDialogues();
        }
        private void MoveDownSelectedElements() 
        {
            ClassConversation.MoveDownSelectedDialogues();
        }

        #region initialization
        private void Setvariables(SerializedObject conversation)
        {
            ScriptableConversation = conversation;
            ClassConversation = (DS_Conversation)ScriptableConversation.targetObject;
            node_sequence = ScriptableConversation.FindProperty("node_sequence");
        }
        private void CreateGUIStyles()
        {
            if (ControllersBackground == null)
            {
                ControllersBackground = new GUIStyle(GUI.skin.box);
                Color backgroundColor = new Color(0.45f, 0.49f, 0.55f);
                ControllersBackground.normal.background = GeneralMethods.GetNewTexture(backgroundColor);
            }

            if (separationLine == null)
            {
                separationLine = new GUIStyle();
                separationLine.normal.background = GeneralMethods.GetNewTexture(GeneralMethods.GetGray(0.2f));
            }
            if (LeaveButton == null) { LeaveButton = CreateGUIStyleButton("Dialogue System/Icons/exitButton"); }
            if (MoveUpButton == null) { MoveUpButton = CreateGUIStyleButton("Dialogue System/Icons/MoveUpButton"); }
            if (MoveDownButton == null) { MoveDownButton = CreateGUIStyleButton("Dialogue System/Icons/MoveDownButton"); }
            if (RemoveButton == null) { RemoveButton = CreateGUIStyleButton("Dialogue System/Icons/deleteButton"); }
        }
        private GUIStyle CreateGUIStyleButton(string iconPath) 
        {
            GUIStyle buttonSyule = new GUIStyle(GUI.skin.button);
            Texture2D background = Resources.Load<Texture2D>(iconPath);
            Texture2D tintedBackground = GeneralMethods.TintTexture(background, 0.8f);

            buttonSyule.normal.background = background;
            buttonSyule.active.background = tintedBackground;
            buttonSyule.onActive.background = tintedBackground;

            return buttonSyule;
        }
        #endregion
        #endregion
    }
}

