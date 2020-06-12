using UnityEngine;
using AGAC.General;
using UnityEditor;

namespace AGAC.DialogueSystem.CustomEditors.Scrollers
{
    class CharactersDialoguesDisplay : VerticalScroller
    {
        #region PUBLIC METHODS
        public CharactersDialoguesDisplay(int width, int height) 
        {
            this.width = width;
            this.height = height;
            this.BackgroundColor = new Color(0.454902f, 0.4901961f, 0.5490196f);
        }
        public void Initialize(SerializedObject conversation, OnPanelLeave OnAddDialogue)
        {
            Conversation = conversation;
            SetConversationVariables();
            int depth = (int)ListDepth.counter;
            SelectedElementsPerDepth = new int[depth];
            ResetIndexesFor(ListDepth.CHARACTERS);
            this.OnAddDialogue = OnAddDialogue;
        }

        public void OnGUI() 
        {
            if (Conversation == null) return;
            CreateGUIStyles();
            Conversation.UpdateIfRequiredOrScript();
            DrawList(DrawCharactersList);
            Conversation.ApplyModifiedProperties();
        }
        #endregion

        #region VARIABLES
        private SerializedObject Conversation;
        private SerializedProperty characters;
        private SerializedProperty node_sequence;


        private SerializedObject Character;
        private SerializedProperty Name;
        private SerializedProperty Sequences;

        private SerializedProperty last_node;
        private SerializedProperty node_character;
        private SerializedProperty node_sequenceIndex;
        private SerializedProperty node_dialogueIndex;

        private SerializedProperty dialogues;
        private OnPanelLeave OnAddDialogue;

        private enum ListDepth { CHARACTERS, SEQUENCES, DIALOGUES, counter }
        private int[] SelectedElementsPerDepth;
        private GUIStyle[] ListElementsStyles;
        private GUIStyle AddDialogueButton;
        private GUIStyle SelectionBackgroundBox;

        private const int indentSpace = 20;
        #endregion

        #region PRIVTAE METHODS
        #region draw methods
        private void DrawCharactersList() 
        {
            for (int character = 0; character < characters.arraySize; ++character)
            {
                GUILayout.Space(5);
                DrawCharacter(character);
            }
            GUILayout.Space(5);
        }
        private void DrawCharacter(int index)
        {
            SetCharacterVariables(index);

            int selectedCharacter = GetSelectetElementIndex(ListDepth.CHARACTERS);
            if (index == selectedCharacter)
            {
                GUILayout.BeginVertical(SelectionBackgroundBox, GUILayout.Width(width-14));
                GUILayout.Space(6);
            }

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(indentSpace * 0.1f);
            GUIStyle style = ListElementsStyles[(int)ListDepth.CHARACTERS];
            if (GUILayout.Button(Name.stringValue, style, GUILayout.Width(width- 25), GUILayout.Height(38)))
                SetHierarchyIndexes(ListDepth.CHARACTERS, index);
            EditorGUILayout.EndHorizontal();

            if (index == selectedCharacter)
            {
                DrawCharacterSequences();
                GUILayout.Space(5);
                GUILayout.EndVertical();
            }
        }
        private void DrawCharacterSequences()
        {
            GUILayout.Space(5);
            int currentSequence = GetSelectetElementIndex(ListDepth.SEQUENCES);
            for (int sequence = 0; sequence < Sequences.arraySize; ++sequence)
            {
                DrawSequence(sequence);
                if (sequence == currentSequence)
                    DrawDialogues();
                GUILayout.Space(5);
            }
        }
        private void DrawSequence(int sequenceIndex) 
        {
            SerializedProperty sequence = Sequences.GetArrayElementAtIndex(sequenceIndex);
            if (sequence.objectReferenceValue == null) return;
            EditorGUILayout.BeginHorizontal();
            int depthLevel = (int)ListDepth.SEQUENCES;
            GUILayout.Space(indentSpace * depthLevel);
            GUIStyle style = ListElementsStyles[depthLevel];
            string sequenceName = sequence.objectReferenceValue.name;
            if (GUILayout.Button(sequenceName, style, GUILayout.Width(width - 45), GUILayout.Height(32)))
            {
                SetHierarchyIndexes(ListDepth.SEQUENCES, sequenceIndex);
                SetDialogueVariables();
            }
            EditorGUILayout.EndHorizontal();
        }
        private void DrawDialogues()
        {
            for (int dialogue = 0; dialogue < dialogues.arraySize; ++dialogue)
                DrawDialogue(dialogue);
        }
        private void DrawDialogue(int dialogue)
        {
            SetSelecterElementOn(ListDepth.DIALOGUES, dialogue);
            GUILayout.Space(5);
            DrawAddDialogueButton();
            EditorGUILayout.BeginHorizontal();
            int depthLevel = (int)ListDepth.DIALOGUES;
            GUILayout.Space(indentSpace * depthLevel);
            string label = GetShortenTextOfDialogue(dialogue);
            GUIStyle style = ListElementsStyles[depthLevel];
            EditorGUILayout.LabelField(label, style, GUILayout.Width(width-65), GUILayout.Height(26));
            EditorGUILayout.EndHorizontal();
        }
        private void DrawAddDialogueButton()
        {
            Rect rect = GUILayoutUtility.GetRect(0, 0);
            rect.x += 10;
            rect.y +=2;
            rect.width = 24;
            rect.height = 24;
            if(GUI.Button(rect, GUIContent.none, AddDialogueButton))
            {
                scrollerPosition.y = 1;
                AddConversationNode();
                OnAddDialogue();
            }    
        }
        #endregion

        private void SetHierarchyIndexes(ListDepth current, int value) 
        {
            SetSelecterElementOn(current, value);
            ListDepth deeperHierarchy = current + 1;
            ResetIndexesFor(deeperHierarchy);
        }
        private void AddConversationNode() 
        {
            int sequence_index = node_sequence.arraySize;
            node_sequence.InsertArrayElementAtIndex(sequence_index);
            last_node = node_sequence.GetArrayElementAtIndex(sequence_index);
            SetNodeVariables();
            AssingNodeVariablesValues();
        }

        private string GetShortenTextOfDialogue(int dialogueIndex) 
        {
            SerializedProperty dialogue = dialogues.GetArrayElementAtIndex(dialogueIndex);
            string dialogueText = dialogue.FindPropertyRelative("text").stringValue;
            return GetShortenText(dialogueText, 20);
        }
        private string GetShortenText(string original, int count)
        {
            string shortenText = string.Empty;
            if (count > original.Length)
                count = original.Length;
            for (int letter = 0; letter < count; ++letter)
                shortenText += original[letter];
            return shortenText;
        }

        #region depth elements indexes
        private void ResetIndexesFor(ListDepth depth) 
        {
            for (int _depth = (int)depth; _depth < (int)ListDepth.counter; ++_depth)
                SelectedElementsPerDepth[_depth] = -1;
        }
        private void SetSelecterElementOn(ListDepth depth, int value) 
        {
            SelectedElementsPerDepth[(int)depth] = value;
        }
        private int GetSelectetElementIndex(ListDepth depth)
        {
            return SelectedElementsPerDepth[(int)depth];
        }
        #endregion

        #region initialization
        #region variable setters
        private void SetConversationVariables()
        {
            characters = Conversation.FindProperty("characters");
            node_sequence = Conversation.FindProperty("node_sequence");
        }
        private void SetCharacterVariables(int character)
        {
            Object ScriptableCharacter = characters.GetArrayElementAtIndex(character).objectReferenceValue;
            Character = new SerializedObject(ScriptableCharacter);
            Name = Character.FindProperty("Name");
            Sequences = Character.FindProperty("Sequences");
        }
        private void SetDialogueVariables() 
        {
            int currentSequence = GetSelectetElementIndex(ListDepth.SEQUENCES);
            Object ScriptableSequence = Sequences.GetArrayElementAtIndex(currentSequence).objectReferenceValue;
            SerializedObject Sequence = new SerializedObject(ScriptableSequence);
            dialogues = Sequence.FindProperty("dialogues");
        }
        private void SetNodeVariables() 
        {
            node_character = last_node.FindPropertyRelative("character");
            node_sequenceIndex = last_node.FindPropertyRelative("sequenceIndex");
            node_dialogueIndex = last_node.FindPropertyRelative("dialogueIndex");
        }
        private void AssingNodeVariablesValues() 
        {
            node_dialogueIndex.intValue = GetSelectetElementIndex(ListDepth.DIALOGUES);
            node_sequenceIndex.intValue = GetSelectetElementIndex(ListDepth.SEQUENCES);
            int currentCharacter = GetSelectetElementIndex(ListDepth.CHARACTERS);
            node_character.objectReferenceValue = characters.GetArrayElementAtIndex(currentCharacter).objectReferenceValue;
        }
        #endregion

        #region gui style creation
        private void CreateGUIStyles() 
        {
            if (ListElementsStyles == null)
            {
                int total = (int)ListDepth.counter;
                ListElementsStyles = new GUIStyle[total];

                for (int depth = 0; depth < total; ++depth)
                    ListElementsStyles[depth] = CreateGUIStyleFor(depth);
            }

            if (AddDialogueButton == null)
            {
                AddDialogueButton = new GUIStyle(GUI.skin.button);
                Texture2D background = Resources.Load<Texture2D>("Dialogue System/Icons/addButton");
                Texture2D tinted = TintTexture(background, new Color(0.7f, 0.7f, 0.7f));
                AddDialogueButton.normal.background = background;
                AddDialogueButton.active.background = tinted;
                AddDialogueButton.focused.background = tinted;
            }

            if (SelectionBackgroundBox == null) 
            {
                SelectionBackgroundBox = new GUIStyle(GUI.skin.box);
                Color color = GeneralMethods.GetGray(0.3607843f);
                SelectionBackgroundBox.normal.background = GeneralMethods.GetNewTexture(color);
            }
        }
        private GUIStyle CreateGUIStyleFor(int depth) 
        {
            ListDepth Ldepth = (ListDepth)depth;

            GUIStyleCreationOption creationOptions = GetGUIStyleCreationOption(Ldepth);
            Color backgroundColor = creationOptions.backgroundColor;
            Texture2D backgroundTexture = Resources.Load<Texture2D>("Dialogue System/Icons/roundedBox");
            Texture2D tintedTexture = TintTexture(backgroundTexture, backgroundColor);

            GUIStyle style = new GUIStyle(GUI.skin.button);
            style.alignment = TextAnchor.MiddleLeft;
            style.fontSize = creationOptions.fontSize;
            style.fontStyle = creationOptions.fontStyle;
            style.normal.background = tintedTexture;

            return style;
        }
        private GUIStyleCreationOption GetGUIStyleCreationOption(ListDepth depth)
        {
            switch (depth) 
            {
                case ListDepth.CHARACTERS:
                    return new GUIStyleCreationOption(new Color(0.945f, 0.949f, 0.964f), 18, FontStyle.Bold);
                case ListDepth.SEQUENCES:
                    return new GUIStyleCreationOption(new Color(0.643f, 0.690f, 0.745f), 16, FontStyle.Italic);
                case ListDepth.DIALOGUES:
                    return new GUIStyleCreationOption(new Color(0.807f, 0.839f, 0.878f), 12, FontStyle.Normal);
                default:
                    return new GUIStyleCreationOption(Color.white);
            }
        }
        private Texture2D TintTexture(Texture2D texture, Color tint)
        {
            return GeneralMethods.TintTexture(texture, tint);
        }
        #endregion
        #endregion
        #endregion
    }
}