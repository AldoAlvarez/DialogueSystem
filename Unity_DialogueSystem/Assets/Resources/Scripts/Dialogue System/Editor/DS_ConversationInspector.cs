using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using AGAC.General;
using AGAC.DialogueSystem.CustomEditors.Panels;

namespace AGAC.DialogueSystem.CustomEditors
{
    public delegate void OnPanelLeave();

    public class DS_ConversationInspector : EditorWindow
    {
        [MenuItem("Window/Dialogue System/Conversation Inspector")]
        public static void Open() 
        {
            DS_ConversationInspector window = GetWindow<DS_ConversationInspector>();
            Vector2 windowSize = new Vector2(700, 600);
            window.minSize = windowSize;
            window.maxSize = windowSize;
            window.Show();
        }
        public void OnGUI()
        {
            Initialize();
            DrawBackground();
            switch (panel) {
                case Panels.MAIN: mainPanel.OnGUI(); break;
                case Panels.EDIT: editablePanel.OnGUI();break;
            }
        }
        public void OnFocus()
        {
            if (panel == 0)
            {
                if (mainPanel == null)
                    mainPanel = new DS_MainConversationPanel(LeavePanel);
                mainPanel.OnFocus();
            }
        }

        #region VARIABLES
        private GUIStyle backgroundStyle;
        private DS_MainConversationPanel mainPanel;
        private DS_EditableConversationPanel editablePanel;
        private enum Panels { MAIN, EDIT }
        private Panels panel = Panels.MAIN;
        #endregion

        #region PRIVATE METHODS
        private void LeavePanel()
        {
            Panels nextPanel = GetNextPanel();
            switch (nextPanel)
            {
                case Panels.EDIT:
                    editablePanel.Initialize(mainPanel.GetSelectedConversation());
                    break;

                default: break;
            }
            panel = nextPanel;
        }
        private Panels GetNextPanel()
        {
            switch (panel)
            {
                case Panels.MAIN: return Panels.EDIT;
                case Panels.EDIT: return Panels.MAIN;
                default: return default;
            }
        }

        private void DrawBackground() 
        {
            GUI.Box(new Rect(0, 0, position.width, position.height), GUIContent.none, backgroundStyle);
        }
        private void Initialize() 
        {
            if(mainPanel==null)
                mainPanel = new DS_MainConversationPanel(LeavePanel);
            if (editablePanel == null)
                editablePanel = new DS_EditableConversationPanel(LeavePanel);
            SetBackgroundColor();
        }
        private void SetBackgroundColor() 
        {
            backgroundStyle = new GUIStyle(GUI.skin.box);
            Color color = GeneralMethods.GetGray(.25f);
            //Color color = GeneralMethods.GetGray(0.3607f);
            Texture2D texture = GeneralMethods.GetNewTexture(color);
            backgroundStyle.normal.background = texture;
        }
        #endregion
    }
}