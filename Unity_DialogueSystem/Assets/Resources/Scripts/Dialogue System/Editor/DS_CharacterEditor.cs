using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using AGAC.DialogueSystem.Base;

namespace AGAC.DialogueSystem.CustomEditors
{
    [CustomEditor(typeof(DS_Character))]
    public class DS_CharacterEditor : Editor
    {
        #region EDITOR METHODS
        private void OnEnable()
        {
            SetVariables();
        }
        public override void OnInspectorGUI()
        {
            serializedObject.UpdateIfRequiredOrScript();
            int indent = EditorGUI.indentLevel;
            if (!created_styles)
            {
                CreateGUIStyles();
                return;
            }

            GUILayout.Space(10);
            DrawIconInfo();
            GUILayout.Space(6);
            DrawIDColor();
            DrawDivision();
            DrawName();
            DrawSequences();

            EditorGUI.indentLevel = indent;
            serializedObject.ApplyModifiedProperties();
        }
        #endregion

        #region VARIABLES
        private GUIStyle boldLabel;
        private GUIStyle addButtonStyle;
        private GUIStyle deleteButtonStyle;
        private bool created_styles = false;

        private SerializedProperty Name;
        private SerializedProperty Sequences;
        private SerializedProperty icon;
        private SerializedProperty id_color;
        private DS_Character character;
        #endregion

        #region PRIVATE METHODS
        #region draw methods
        private void DrawIconInfo()
        {
            DrawIconSprite();
            GUILayout.Space(10);
            DrawIconProperty();
        }
        private void DrawIconSprite()
        {
            if (character.Icon == null) return;
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            Texture2D tex = AssetPreview.GetAssetPreview(character.Icon);
            GUILayout.Label(tex, GUILayout.Height(80), GUILayout.Width(80));

            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }
        private void DrawIconProperty() 
        {
            GUIContent label = new GUIContent("Icon");
            EditorGUILayout.PropertyField(icon, label);
        }

        private void DrawIDColor() 
        {
            GUIContent label = new GUIContent("Color ID");
            EditorGUILayout.PropertyField(id_color, label);
            //character.IDcolor = EditorGUILayout.ColorField(label, character.IDcolor, true, false, false);
        }

        private void DrawName()
        {
            GUIContent label = new GUIContent("Name");
            EditorGUILayout.PropertyField(Name, label);
        }
        private void DrawSequences()
        {
            DrawSequencesHeaderNController();
            DrawDivision();
            EditorGUI.indentLevel++;
            DrawSequencesAsList();
        }
        private void DrawSequencesHeaderNController()
        {
            EditorGUILayout.LabelField("Sequences", boldLabel);
            GUILayout.Space(8);
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("ADD", addButtonStyle, GUILayout.Width(100)))
                character.AddSequence();
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }
        private void DrawSequencesAsList()
        {
            for (int i = 0; i < Sequences.arraySize; ++i)
            {
                SerializedProperty sequence = Sequences.GetArrayElementAtIndex(i);
                EditorGUILayout.PropertyField(sequence);
                GUILayout.Space(10);
                DrawDeleteButton(i);
                GUILayout.Space(20);
            }
        }
        private void DrawDeleteButton(int sequenceIndex)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(string.Empty);
            if (GUILayout.Button("DELETE", deleteButtonStyle, GUILayout.Width(80)))
            {
                if (EditorUtility.DisplayDialog(
                    "Delete Dialogue",
                    "Are you sure you want to delete dialogue number $dialogueIndex ?",
                    "Yes",
                    "NO"))
                {
                    character.RemoveSequence((uint)sequenceIndex);
                }
            }
            GUILayout.Space(10);
            EditorGUILayout.EndHorizontal();
        }
        private void DrawDivision()
        {
            GUILayout.Space(8);
            EditorGUILayout.LabelField(string.Empty, GUI.skin.horizontalSlider);
            GUILayout.Space(8);
        }
        #endregion

        private void SetVariables()
        {
            Name = serializedObject.FindProperty("Name");
            Sequences = serializedObject.FindProperty("Sequences");
            icon = serializedObject.FindProperty("icon");
            id_color = serializedObject.FindProperty("id_color");
            character = (DS_Character)target;
            CreateGUIStyles();
        }
        private void CreateGUIStyles()
        {
            if (created_styles) return;
            try
            {
                boldLabel = new GUIStyle(EditorStyles.boldLabel);
                boldLabel.fontStyle = FontStyle.Bold;

                Color text = new Color(0.15f, 0.71f, 0.11f);
                Color background = new Color(0.4f, 1f, 0.31f);
                addButtonStyle = GetBoldButtonStyle(text, background);

                text = new Color(0.85f, 0.1f, 0.1f);
                background = new Color(1f, 0.4f, 0.4f);
                deleteButtonStyle = GetBoldButtonStyle(text, background);

                created_styles = true;
            }
            catch (System.Exception)
            {
                created_styles = false;
            }
        }
        private GUIStyle GetBoldButtonStyle(Color text, Color background)
        {
            GUIStyle buttonStyle = new GUIStyle(EditorStyles.toolbarButton);
            buttonStyle.fontSize = 12;
            buttonStyle.fontStyle = FontStyle.Bold;
            buttonStyle.normal.textColor = text;
            buttonStyle.normal.background = GetTexture(background);
            return buttonStyle;
        }
        private Texture2D GetTexture(Color color)
        {
            Texture2D tex = new Texture2D(1, 1);
            tex.SetPixel(0, 0, color);
            tex.Apply();
            return tex;
        }
        #endregion
    }
}