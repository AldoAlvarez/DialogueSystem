using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using AGAC.General;
using AGAC.DialogueSystem.Base;

public class FieldDialog<T> : EditorWindow where T : Object
{
    #region EDITOR METHODS
    public void OnDestroy()
    {
        Active = false;
    }
    public void OnGUI()
    {
        Active = true;
        InitializeGUIStyles();
        DrawBackground();
        GUILayout.Space(15);
        DrawWindowInfo();
        DrawElementField();
        GUILayout.Space(25);
        DrawButtons();
    }
    #endregion

    #region VARIABLES
    public bool Active { private set; get; } = false;

    public delegate void OnClickActions();
    public OnClickActions OnOK;
    public OnClickActions OnCancel;

    private static string Title = "Title";
    private static string SupportText = "Support text";

    private GUIStyle backgroundStyle;
    private GUIStyle TitleStyle;
    private GUIStyle SupportTextStyle;
    private static Texture2D okButton;
    private static Texture2D cancelButton;

    private const int windowwidth = 270;
    private const int window_height = 220;
    private const int indent = 20;

    private DS_Character ElementToSelect;
    #endregion

    #region public METHODS
    public DS_Character GetSelectedElement() 
    {
        if (!Active) return default(DS_Character);
        return ElementToSelect;
    }
    public void ReFocus()
    {
        if (!Active) return;
        FieldDialog<T> window = GetWindow<FieldDialog<T>>();
        PositionWindow(window);
        window.ShowPopup();
    }
    public static FieldDialog<T> Open(string title, string supportText, Texture2D ok = null, Texture2D cancel = null)
    {
        Title = title;
        SupportText = supportText;
        okButton = ok;
        cancelButton = cancel;

        //FieldDialog<DS_Character> window = ScriptableObject.CreateInstance<FieldDialog<DS_Character>>();
        FieldDialog<T> window = EditorWindow.GetWindow<FieldDialog<T>>();
        if (window == null) 
        {
            Debug.Log("Cant create window");
            return null;
        }
        Vector2 windowSize = new Vector2(windowwidth, window_height);
        PositionWindow(window);
        window.minSize = windowSize;
        window.maxSize = windowSize;
        window.ShowPopup();
        return window;
    }

    #endregion

    #region private METHODS
    #region draw methods
    private void DrawBackground() 
    {
        GUI.Box(new Rect(0, 0, windowwidth, window_height), GUIContent.none, backgroundStyle);
    }
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
    private void DrawElementField()
    {
        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(indent * 1.5f);
        int width = windowwidth - (indent * 3);
        GUILayoutOption[] options = new GUILayoutOption[] { GUILayout.Width(width), GUILayout.Height(20) };
        ElementToSelect = (DS_Character)EditorGUILayout.ObjectField(ElementToSelect, typeof(DS_Character), false, options);
        EditorGUILayout.EndHorizontal();
    }
    private void DrawButtons()
    {
        GUILayoutOption[] options = new GUILayoutOption[] { GUILayout.Width(32), GUILayout.Height(32) };
        EditorGUILayout.BeginHorizontal();
        int margin = 114 + indent;
        GUILayout.Space(windowwidth - margin);
        DrawButton(cancelButton, OnCancel, options);
        GUILayout.Space(30);
        DrawButton(okButton, OnOK, options);
        EditorGUILayout.EndHorizontal();
    }
    private void DrawButton(Texture2D background, OnClickActions actions, GUILayoutOption[] options)
    {
        GUIStyle style = new GUIStyle(GUI.skin.button);
        style.normal.background = background;
        if (GUILayout.Button(GUIContent.none, style, options))
        {
            actions?.Invoke();
            FieldDialog<T> window = GetWindow<FieldDialog<T>>();
            window.Close();
        }
    }
    #endregion

    #region initialization
    private static FieldDialog<T> DisplayWindow() 
    {
        FieldDialog<T> window = ScriptableObject.CreateInstance<FieldDialog<T>>();
        Vector2 windowSize = new Vector2(windowwidth, window_height);
        window.minSize = windowSize;
        window.maxSize = windowSize;
        PositionWindow(window);
        window.ShowPopup();
        return window;
    }
    private static void PositionWindow(EditorWindow window)
    {
        Rect pos = new Rect(0, 0, windowwidth, window_height);
        pos.x = (Screen.width / 2) + (windowwidth / 2);
        pos.y = (Screen.height / 2) - (window_height / 2);
        window.position = pos;
    }
    private void InitializeGUIStyles()
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
        if (backgroundStyle == null)
        {
            backgroundStyle = new GUIStyle(GUI.skin.box);
            backgroundStyle.normal.background = GeneralMethods.GetNewTexture(Color.white);
        }
    }
    #endregion

    #region auxiliary
    private GUIStyle CreateButtonLikeStyle(Texture2D backgroundIcon) 
    {
        GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
        if (backgroundIcon != null)
        {
            buttonStyle.normal.background = backgroundIcon;
            Texture2D darkenIcon = TintTexture(backgroundIcon, 0.65f);
            buttonStyle.active.background = darkenIcon;
            buttonStyle.normal.background = darkenIcon;
        }
        return buttonStyle;
    }

    private Texture2D TintTexture(Texture2D texture, float multiple) 
    {
        return GeneralMethods.TintTexture(texture, multiple);
    }
    #endregion
    #endregion
}
