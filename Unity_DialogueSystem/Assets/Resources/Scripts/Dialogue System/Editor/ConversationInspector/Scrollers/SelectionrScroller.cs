using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using AGAC.General;

namespace AGAC.DialogueSystem.CustomEditors.Scrollers
{
    public sealed class SelectionScroller
    {
        public SelectionScroller(int width, int height) 
        {
            Initialize();
            scrollViewOptions.Add(GUILayout.Width(width));
            scrollViewOptions.Add(GUILayout.Height(height));
            SetColors(Color.white, Color.gray, Color.black);
        }

        #region VARIABLES
        public int SelectedElement { get; private set; }
        private Vector2 scrollerPosition;
        private GUIStyle scrollerBackground;

        private GUIStyle ElementStyle;
        private Texture2D SelectdState;
        private Texture2D NormalState;

        List<GUILayoutOption> scrollViewOptions;
        #endregion

        #region PUBLIC METHODS
        public void SetColors(Color background, Color normalElement, Color selectedElement) 
        {
            NormalState = GetTexture(normalElement);
            SelectdState = GetTexture(selectedElement);
            scrollerBackground.normal.background = GetTexture(background);
        }

        public void DrawElements(string[] elements, params GUILayoutOption[] options)
        {
            scrollerPosition = 
                EditorGUILayout.BeginScrollView(
                    scrollerPosition,
                    false,
                    true,
                    GUIStyle.none,
                    GUI.skin.verticalScrollbar,
                    scrollerBackground,
                    scrollViewOptions.ToArray());

            DrawScrollViewElements(elements, options);
            EditorGUILayout.EndScrollView();
        }
        #endregion

        #region PRIVATE METHODS

        private void DrawScrollViewElements(string[] elements, params GUILayoutOption[] options) 
        {
            for (int element = 0; element < elements.Length; ++element)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(10);
                GUIStyle style = GetStyleForElement(element);
                if (GUILayout.Button(elements[element], style, options))
                    SelectedElement = element;
                EditorGUILayout.EndHorizontal();
            }
        }
        private GUIStyle GetStyleForElement(int elementIndex) 
        {
            Texture2D styleBackground = NormalState;
            if (elementIndex == SelectedElement)
                styleBackground = SelectdState;
            ElementStyle.normal.background = styleBackground;
            ElementStyle.active.background = SelectdState;
            return ElementStyle;
        }
        private void Initialize() 
        {
            ElementStyle = new GUIStyle(GUI.skin.button);
            ElementStyle.fontSize = 20;
            ElementStyle.alignment = TextAnchor.MiddleLeft;

            scrollerBackground = new GUIStyle(GUI.skin.box);
            scrollViewOptions = new List<GUILayoutOption>();
        }
        private Texture2D GetTexture(Color color) 
        {
            return GeneralMethods.GetNewTexture(color);
        }
        #endregion
    }
}