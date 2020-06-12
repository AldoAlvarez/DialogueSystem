using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using AGAC.DialogueSystem.Base;
using AGAC.General;
using AGAC.DialogueSystem.CustomEditors.Scrollers;

namespace AGAC.DialogueSystem.CustomEditors.Panels
{
    public class DS_MainConversationPanel 
    {
        public DS_MainConversationPanel(OnPanelLeave OnLeave) 
        {
            this.OnLeave = OnLeave;
        }

        public void OnFocus() 
        {
            if (AddCharacterWindow.Active)
                AddCharacterWindow.Refocus();
        }
        public void OnGUI() 
        {
            Initialize();
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(30);
            DrawConversationList();
            GUILayout.Space(30);
            DrawControlButtons();
            EditorGUILayout.EndHorizontal();
        }

        #region VARIABLES
        private DS_Conversation[] projectConversations;
        private SelectionScroller ConversationScroller;
        private CharactersScroller charactersScroller;

        private Texture2D addButton;
        private Texture2D accessButton;
        private OnPanelLeave OnLeave;
        #endregion

        public SerializedObject GetSelectedConversation() 
        {
            int selectedConversation = ConversationScroller.SelectedElement;
            if (selectedConversation < 0 || selectedConversation >= projectConversations.Length) return null;
            return new SerializedObject(projectConversations[selectedConversation]);
        }

        #region PRIVATE METHODS
        private void DrawConversationList() 
        {
            EditorGUILayout.BeginVertical();
            GUILayout.Space(50);
            string[] elements = GetConversationIDs();
            ConversationScroller.DrawElements(elements, GUILayout.Width(260), GUILayout.Height(40));
            EditorGUILayout.EndVertical();
        }
        private void DrawCharactersList() 
        {
            EditorGUILayout.BeginVertical();
            GUILayout.Space(30);
            SerializedObject conversation = GetSelectedConversation();
            charactersScroller.OnGUI(conversation);
            EditorGUILayout.EndVertical();
        }
        private void DrawControlButtons() 
        {
            EditorGUILayout.BeginVertical();
            GUILayout.Space(100);
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(30);
            DrawButton(accessButton,60,60, OnEnterConversation);
            GUILayout.Space(100);
            DrawButton(addButton, 60,60, OnAddCharacter);
            EditorGUILayout.EndHorizontal();
            DrawCharactersList();
            EditorGUILayout.EndVertical();
        }

        private delegate void OnClikAction();
        private void OnAddCharacter() 
        {
            if (AddCharacterWindow.Active) return;
            int selectedConversation = ConversationScroller.SelectedElement;
            if (selectedConversation < 0 || selectedConversation >= projectConversations.Length) return;
            SerializedObject currentConversation = new SerializedObject(projectConversations[selectedConversation]);
            AddCharacterWindow.Open((DS_Conversation)currentConversation.targetObject);
        }
        private void OnEnterConversation() { OnLeave(); }
        private void DrawButton(Texture2D icon, int width, int height, OnClikAction OnClick) 
        {
            GUIStyle style = new GUIStyle(GUI.skin.button);
            Texture2D button = ResizeTexture(icon, width, height);
            style.normal.background = button;
            style.active.background = TintTexture(button,0.7f);
            if (GUILayout.Button(GUIContent.none, style, GUILayout.Width(width), GUILayout.Height(height)))
                OnClick();
        }
        private Texture2D TintTexture(Texture2D texure, float multiple) 
        {
            return GeneralMethods.TintTexture(texure, multiple);
        }
        private Texture2D ResizeTexture(Texture2D texture, int width, int height) 
        {
            return GeneralMethods.ResizeTexture(texture, width, height);
        }

        private string[] GetConversationIDs() 
        {
            int totalConversations = projectConversations.Length;
            string[] conversationIDs = new string[totalConversations];
            for (int i = 0; i < totalConversations; ++i)
                conversationIDs[i] = projectConversations[i].name;
            return conversationIDs;
        }
        private void Initialize() 
        {
            if (ConversationScroller == null)
                ConversationScroller = new SelectionScroller(280, 500);
            if (charactersScroller == null)
                charactersScroller = new CharactersScroller();

            SetGUIStyles();
            SearchForConversationsInFolder(DS_ConversationAdmin.ConversationsPath);
        }
        private void SetGUIStyles() 
        {
            accessButton = Resources.Load<Texture2D>("Dialogue System/Icons/loginButton");
            addButton = Resources.Load<Texture2D>("Dialogue System/Icons/addPersonButton");

            Color normal = new Color(0.8078432f, 0.8392158f, 0.8784314f);
            Color selected = new Color(0.6431373f, 0.6901961f, 0.7450981f);
            Color background = new Color(0.454902f, 0.4901961f, 0.5490196f);
            ConversationScroller.SetColors(background, normal, selected);
        }
        private void SearchForConversationsInFolder(string folder) 
        {
            projectConversations = Resources.LoadAll<DS_Conversation>(folder);
        }
        #endregion
    }
}
