using UnityEngine;
using UnityEditor;
using AGAC.General;
using AGAC.DialogueSystem.Base;

namespace AGAC.DialogueSystem.CustomEditors.Scrollers
{
    public class CharactersScroller
    {
        public void OnGUI(SerializedObject conversation)
        {
            if (conversation == null) return;
            Initialize();
            Characters = conversation.FindProperty("characters");

            GUILayout.Space(5);
            GUIStyle bkg = new GUIStyle(GUI.skin.box);
            bkg.normal.background = GeneralMethods.GetNewTexture(new Color(.36f, .36f, .36f));
            scrollerPosition =
                EditorGUILayout.BeginScrollView(
                    scrollerPosition,
                    false, false,
                    GUI.skin.horizontalScrollbar,
                    GUI.skin.verticalScrollbar,
                    bkg,
                    GUILayout.Width(280),
                    GUILayout.Height(350));
            for (int character = 0; character < Characters.arraySize; ++character)
            {
                DrawCharacterElement(character);
                GUILayout.Space(5);
            }
            EditorGUILayout.EndScrollView();

            if (hasToRemoveCharacter)
            {
                ((DS_Conversation)conversation.targetObject).RemoveCharacter((uint)characterToRemove);
                hasToRemoveCharacter = false;
            }
        }

        #region VARIABLES
        private SerializedObject Character;
        private SerializedProperty Characters;

        private Vector2 scrollerPosition;

        private SerializedProperty Name;
        private SerializedProperty icon;
        private SerializedProperty id_color;

        private GUIStyle deleteButton;
        private GUIStyle NameLabelStyle;
        private int BoxHeight = 40;

        private int characterToRemove = -1;
        private bool hasToRemoveCharacter = false;
        #endregion

        #region PRIVATE METHODS
        #region draw
        private void DrawCharacterElement(int characterIndex)
        {
            SetCharacterVariables(characterIndex);
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(10);
            DrawIcon(36);
            DrawCharacterName();
            GUILayout.Space(6);
            DrawRemoveButton(26, characterIndex);
            EditorGUILayout.EndHorizontal();
        }

        private void DrawCharacterName()
        {
            EditorGUILayout.LabelField(Name.stringValue, NameLabelStyle, GUILayout.Width(220), GUILayout.Height(BoxHeight));
        }
        private void DrawIcon(int width)
        {
            EditorGUILayout.BeginVertical(GUILayout.Width(width));
            GUILayout.Space((BoxHeight / 2) - (width / 2));
            DrawSprite(GetCharacterIcon(), width, width);
            EditorGUILayout.EndVertical();
        }

        private void DrawSprite(Sprite sprite, int width, int height)
        {
            Texture2D tex = AssetPreview.GetAssetPreview(sprite);
            GUILayout.Label(tex, GUILayout.Height(height), GUILayout.Width(width));
        }

        private void DrawRemoveButton(int sideLenght, int character)
        {
            EditorGUILayout.BeginVertical(GUILayout.Width(sideLenght));
            GUILayout.Space((BoxHeight / 2) - (sideLenght / 2));
            if (GUILayout.Button(GUIContent.none, deleteButton, GUILayout.Width(sideLenght), GUILayout.Height(sideLenght - 2)))
                DisplayRemoveDialogFor(character);
            EditorGUILayout.EndVertical();
        }
        #endregion

        #region auxiliary
        private void DisplayRemoveDialogFor(int character)
        {
            if (EditorUtility.DisplayDialog(
                "Remove Character",
                "Are you sure you want to remove " + Name.stringValue + " from this conversation?",
                "Yes", "No"))
                RemoveCharacterFromConversation(character);

        }
        private void RemoveCharacterFromConversation(int character)
        {
            characterToRemove = character;
            hasToRemoveCharacter = true;
        }
        private Sprite GetCharacterIcon()
        {
            return (Sprite)Character.FindProperty("icon").objectReferenceValue;
        }
        private Texture2D TintTexture(Texture2D texture, float multiple)
        {
            return AGAC.General.GeneralMethods.TintTexture(texture, multiple);
        }
        #endregion

        #region initialization
        private void Initialize()
        {
            if (NameLabelStyle == null)
            {
                NameLabelStyle = new GUIStyle(EditorStyles.label);
                NameLabelStyle.alignment = TextAnchor.MiddleLeft;
                NameLabelStyle.fontSize = 16;

            }
            if (deleteButton == null)
            {
                deleteButton = new GUIStyle(EditorStyles.miniButton);
                deleteButton.alignment = TextAnchor.LowerCenter;
                Texture2D background = Resources.Load<Texture2D>("Dialogue System/Icons/removeButton");
                deleteButton.normal.background = background;
                deleteButton.active.background = TintTexture(background, 0.7f);
                deleteButton.focused.background = TintTexture(background, 0.7f);
            }
        }

        private void SetCharacterVariables(int character)
        {
            if (character < 0 ||
                character >= Characters.arraySize) return;

            Object characterObj = Characters.GetArrayElementAtIndex(character).objectReferenceValue;
            Character = new SerializedObject(characterObj);

            Name = Character.FindProperty("Name");
            icon = Character.FindProperty("icon");
            id_color = Character.FindProperty("id_color");
            Texture2D background = GeneralMethods.GetNewTexture(id_color.colorValue);
            NameLabelStyle.normal.background = background;
        }
        #endregion
        #endregion
    }
}