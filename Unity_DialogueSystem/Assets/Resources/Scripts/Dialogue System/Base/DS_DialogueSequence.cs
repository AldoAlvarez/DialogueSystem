using System;
using System.Collections.Generic;
using UnityEngine;

namespace AGAC.DialogueSystem.Base
{
    public class DS_DialogueSequence : ScriptableObject
    {
        public DS_DialogueSequence()
        {
            id_leywords = "New Sequence";
            dialogues = new List<DS_Dialogue>() { new DS_Dialogue() };
        }
        #region VARIABLES
        private string id_leywords;
        private uint maxKeywordChars = 20;
        private const uint maxDialoguesPerSequence = 20;

        [SerializeField]
        private List<DS_Dialogue> dialogues;

        public DS_Dialogue this[uint i] 
        {
            get 
            {
                return GetDialogue(i);
            }
            set 
            {
                SetDialogue(value, i);
            }
        }

        public string ID_Keywords
        {
            get
            {
                return GetWrapedText(id_leywords, maxKeywordChars);
            }
            set 
            {
                id_leywords = GetWrapedText(value, maxKeywordChars);
            }
        }
        #endregion

        #region PUBLIC METHODS
        public void AddDialogue()
        {
            if (dialogues.Count >= maxDialoguesPerSequence) return;
            dialogues.Add(new DS_Dialogue());
        }
        public void RemoveDialogue(uint index)
        {
            if (index >= dialogues.Count) return;
            if (dialogues.Count == 1) return;
            dialogues.RemoveAt((int)index);
        }
        #endregion

        #region PRIVATE METHODS
        private DS_Dialogue GetDialogue(uint index) 
        {
            uint _index = GetAcceptableDialoguesIndex(index);
            return dialogues[(int)_index];
        }
        private void SetDialogue(DS_Dialogue dialogue, uint index) 
        {
            uint _index = GetAcceptableDialoguesIndex(index);
            dialogues[(int)_index] = dialogue;
        }

        private uint GetAcceptableDialoguesIndex(uint index) 
        {
            if (index >= maxDialoguesPerSequence)
                return maxDialoguesPerSequence - 1;
            else if (index >= dialogues.Count)
                return (uint)(dialogues.Count - 1);
            return index;
        }

        private string GetWrapedText(string text, uint maxLenght) 
        {
            if (text.Length <= maxLenght) return text;
            string wraped_text = string.Empty;
            for (int letter = 0; letter < maxLenght; ++letter)
                wraped_text += text[letter];
            return wraped_text;
        }
        #endregion
    }
}