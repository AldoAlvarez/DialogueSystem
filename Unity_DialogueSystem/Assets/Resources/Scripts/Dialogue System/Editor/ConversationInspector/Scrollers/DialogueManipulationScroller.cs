using UnityEngine;
using UnityEditor;
using AGAC.General;
using AGAC.DialogueSystem.Base;
using System;

namespace AGAC.DialogueSystem.CustomEditors.Scrollers
{
    class DialogueManipulationScroller : VerticalScroller
    {
        #region PUBLIC METHODS
        public DialogueManipulationScroller(int width, int height) 
        {
            this.width = width;
            this.height = height;
            this.BackgroundColor = new Color(.54f,.54f,.54f);
        }
        public void Initialize(SerializedObject conversation)
        {
            Conversation = conversation;
            node_sequence = Conversation.FindProperty("node_sequence");
        }
        public void OnGUI() 
        {
            if (Conversation == null) return;
            Conversation.UpdateIfRequiredOrScript();
            DrawList(DrawDialogues, true);
            Conversation.ApplyModifiedProperties();
        }
        public void SetScrollerToEnd()
        {
            scrollerPosition.y = Mathf.Infinity;
        }
        #endregion

        #region VARIABLES
        private SerializedObject Conversation;
        private SerializedProperty node_sequence;
        #endregion

        #region PRIVATE METHODS
        private void DrawDialogues() 
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(8);
            EditorGUILayout.BeginVertical();
            for (int node = 0; node < node_sequence.arraySize; ++node)
            {
                GUILayout.Space(8);
                SerializedProperty _node = node_sequence.GetArrayElementAtIndex(node);
                EditorGUILayout.PropertyField(_node, GUILayout.Width(320));
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
            GUILayout.Space(15);
        }
        #endregion
    }
}