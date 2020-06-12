using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using AGAC.DialogueSystem.Base;

namespace AGAC.DialogueSystem.CustomEditors
{
    public class AddCharacterWindow : EditorWindow
    {
        public static void Refocus()
        {
            if (!Active) return;
            AddCharacterWindow window = GetWindow<AddCharacterWindow>();
            PositionWindow(window);
            window.ShowPopup();
        }
        public static void Open(DS_Conversation conversation)
        {
            if (conversation == null) return;
            Conversation = conversation;
            Active = true;
            AddCharacterWindow window = ScriptableObject.CreateInstance<AddCharacterWindow>();
            Vector2 windowSize = new Vector2(windowwidth, window_height);
            window.minSize = windowSize;
            window.maxSize = windowSize;
            PositionWindow(window);
            window.ShowPopup();
        }

        #region EDITOR METHODS
        public void OnDestroy()
        {
            Active = false;
        }
        private void OnGUI()
        {
            Initialize();
            GUIStyle backgroundStyle = new GUIStyle(GUI.skin.box);
            backgroundStyle.normal.background = AGAC.General.GeneralMethods.GetNewTexture(Color.white);
            GUI.Box(new Rect(0, 0, windowwidth, window_height), GUIContent.none, backgroundStyle);

            GUILayout.Space(15);
            DrawWindowInfo();
            DrawCharacterField();
            GUILayout.Space(25);
            DrawButtons();
        }
        #endregion

        #region VARIABLES
        public static bool Active { private set; get; } = false;

        private static DS_Conversation Conversation;
        private DS_Character newCharacter = null;

        private const int windowwidth = 270;
        private const int window_height = 220;
        private const int indent = 20;

        private GUIStyle TitleStyle;
        private GUIStyle SupportTextStyle;
        private Texture2D addButton;
        private Texture2D cancelButton;
        #endregion

        #region PRIVATE METHODS
        #region draw methods
        private void DrawWindowInfo()
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(indent);
            int width = windowwidth - (indent * 2);
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("Add Character", TitleStyle, GUILayout.Width(width), GUILayout.Height(40));
            EditorGUILayout.LabelField("Select a <DS_Character> to be added to the conversation.", SupportTextStyle, GUILayout.Width(width), GUILayout.Height(60));
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }
        private void DrawCharacterField()
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(indent * 1.5f);
            int width = windowwidth - (indent * 3);
            GUILayoutOption[] options = new GUILayoutOption[] { GUILayout.Width(width), GUILayout.Height(20) };
            newCharacter = (DS_Character)EditorGUILayout.ObjectField(newCharacter, typeof(DS_Character), false, options);
            EditorGUILayout.EndHorizontal();
        }
        private void DrawButtons()
        {
            GUILayoutOption[] options = new GUILayoutOption[] { GUILayout.Width(32), GUILayout.Height(32) };
            EditorGUILayout.BeginHorizontal();
            int margin = 114 + indent;
            GUILayout.Space(windowwidth - margin);
            DrawButton(cancelButton, Cancel, options);
            GUILayout.Space(30);
            DrawButton(addButton, AddCharacter, options);
            EditorGUILayout.EndHorizontal();
        }
        private void DrawButton(Texture2D background, OnClickActions actions, GUILayoutOption[] options)
        {
            GUIStyle style = new GUIStyle(GUI.skin.button);
            style.normal.background = background;
            Texture2D tinted = General.GeneralMethods.TintTexture(background, 0.85f);
            style.onActive.background = tinted;
            style.active.background = tinted;
            if (GUILayout.Button(GUIContent.none, style, options))
                actions();
        }
        #endregion

        #region button actions
        private delegate void OnClickActions();
        private void AddCharacter() { Conversation.AddCharacter(newCharacter); Close(); }
        private void Cancel() { Close(); }
        #endregion

        #region initialization
        private static void PositionWindow(EditorWindow window)
        {
            Rect pos = new Rect(0, 0, windowwidth, window_height);
            pos.x = (Screen.width / 2) + (windowwidth / 2);
            pos.y = (Screen.height / 2) - (window_height / 2);
            window.position = pos;
        }
        private void Initialize()
        {
            if (TitleStyle == null)
            {
                TitleStyle = new GUIStyle(EditorStyles.label);
                TitleStyle.fontSize = 20;
                TitleStyle.fontStyle = FontStyle.Bold;
                TitleStyle.alignment = TextAnchor.MiddleCenter;
            }
            if (SupportTextStyle == null)
            {
                SupportTextStyle = new GUIStyle(EditorStyles.label);
                SupportTextStyle.fontSize = 14;
                SupportTextStyle.fontStyle = FontStyle.Normal;
                SupportTextStyle.normal.textColor = new Color(0.2735849f, 0.2735849f, 0.2735849f, 0.65f);
                SupportTextStyle.alignment = TextAnchor.UpperLeft;
                SupportTextStyle.wordWrap = true;
            }
            addButton = LoadTexture("Dialogue System/Icons/confirmButton");
            cancelButton = LoadTexture("Dialogue System/Icons/cancelButton");
        }
        private Texture2D LoadTexture(string path)
        {
            return Resources.Load<Texture2D>(path);
        }
        #endregion
        #endregion
    }
}

