using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using AGAC.DialogueSystem.Base;

namespace AGAC.DialogueSystem.CustomEditors
{
    [CustomEditor(typeof(DS_Conversation))]
    public class DS_ConversationEditor : Editor
    {
        public void OnEnable()
        {
            DS_ConversationInspector.Open();
        }
        public override void OnInspectorGUI()
        {
        }
    }
}