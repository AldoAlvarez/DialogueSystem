using UnityEngine;
using UnityEditor;
using AGAC.General;

namespace AGAC.DialogueSystem.CustomEditors.Scrollers
{
    public delegate void OnDrawScroller();

    abstract class VerticalScroller
    {
        #region VARIABLES
        protected Vector2 scrollerPosition;
        private GUIStyle scrollerBackground;
        protected Color BackgroundColor;
        protected int width = 200;
        protected int height = 200;
        #endregion

        #region PROTECTED METHODS
        protected void DrawList(OnDrawScroller OnDraw, bool horizontal = false)
        {
            CreateGUIStyles();
            scrollerPosition =
                EditorGUILayout.BeginScrollView(
                  scrollerPosition,
                  horizontal,
                  true,
                  horizontal ? GUI.skin.horizontalScrollbar : GUIStyle.none,
                  GUI.skin.verticalScrollbar,
                  scrollerBackground,
                  GUILayout.Width(width),
                  GUILayout.Height(height));
            OnDraw();
            EditorGUILayout.EndScrollView();
        }
        #endregion

        #region PRIVATE METHODS
        private void CreateGUIStyles()
        {
            if (scrollerBackground == null)
            {
                scrollerBackground = new GUIStyle(GUI.skin.box);
                Texture2D background = CreateTexture(BackgroundColor);
                scrollerBackground.normal.background = background;
            }
        }
        private Texture2D CreateTexture(Color color)
        {
            return GeneralMethods.GetNewTexture(color);
        }
        #endregion
    }
}