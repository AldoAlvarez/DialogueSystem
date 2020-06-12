using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace AGAC.General.Editor
{
    public static class GeneralEditorMethods 
    {
        public static void DrawDivision(float width) 
        {
            GUILayout.Space(6);
            EditorGUILayout.LabelField(string.Empty, GUI.skin.horizontalSlider, GUILayout.Width(width));
            GUILayout.Space(6);
        }
    }
}