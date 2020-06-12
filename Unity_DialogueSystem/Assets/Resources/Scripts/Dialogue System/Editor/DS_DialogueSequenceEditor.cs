using UnityEngine;
using UnityEditor;
using AGAC.DialogueSystem.Base;

namespace AGAC.DialogueSystem.CustomEditors
{
    [CustomEditor(typeof(DS_DialogueSequence))]
    public class DS_DialogueSequenceEditor : Editor
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
            DrawDivision();
            DrawKeywords();
            DrawDialogues();
            EditorGUI.indentLevel = indent;

            serializedObject.ApplyModifiedProperties();
        }
        #endregion

        #region VARIABLES
        private GUIStyle boldLabel;
        private GUIStyle addButtonStyle;
        private GUIStyle deleteButtonStyle;
        private DS_DialogueSequence sequence;
        private SerializedProperty dialogues;
        private bool created_styles = false;
        #endregion

        #region PRIVATE METHODS
        #region draw methods
        private void DrawKeywords() 
        {
            GUIContent label = new GUIContent("ID / Keywords");
            sequence.ID_Keywords = EditorGUILayout.TextField(label, sequence.ID_Keywords);
        }
        private void DrawDialogues() 
        {
            DrawDialoguesHeaderNController();
            DrawDivision();
            EditorGUI.indentLevel++;
            DrawDialoguesAsList();
        }
        private void DrawDialoguesHeaderNController() 
        {
            EditorGUILayout.LabelField("Dialogues", boldLabel);
            GUILayout.Space(8);
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("ADD", addButtonStyle, GUILayout.Width(100)))
                sequence.AddDialogue();
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }
        private void DrawDialoguesAsList()
        {
            for (int i = 0; i < dialogues.arraySize; ++i) 
            {
                SerializedProperty dialogue = dialogues.GetArrayElementAtIndex(i);
                EditorGUILayout.PropertyField(dialogue);
                GUILayout.Space(10);
                DrawDeleteButton(i);
                GUILayout.Space(20);
            }
        }
        private void DrawDeleteButton(int dialogueIndex) 
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(string.Empty);
            if (GUILayout.Button("DELETE", deleteButtonStyle, GUILayout.Width(80))) 
            {
                if (EditorUtility.DisplayDialog(
                    "Delete Dialogue",
                    "Are you sure you want to delete dialogue number "+dialogueIndex.ToString()+"?",
                    "Yes",
                    "NO")) 
                {
                    sequence.RemoveDialogue((uint)dialogueIndex);
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
            sequence = (DS_DialogueSequence)target;
            dialogues = serializedObject.FindProperty("dialogues");

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
            Texture2D tex = new Texture2D(1,1);
            tex.SetPixel(0, 0, color);
            tex.Apply();
            return tex;
        }
        #endregion
    }
}