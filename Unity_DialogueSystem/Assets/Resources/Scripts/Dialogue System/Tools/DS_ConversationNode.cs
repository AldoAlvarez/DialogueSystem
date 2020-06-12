using System;
using AGAC.DialogueSystem.Base;
using UnityEngine;

namespace AGAC.DialogueSystem.Tools
{
    [Serializable]
    public class DS_ConversationNode
    {
        public DS_Character character;
        public uint sequenceIndex;
        public uint dialogueIndex;

        [SerializeField]
        private Vector2 scrollerPos;
        [SerializeField]
        private bool selected = false;

        public bool isSelected { get { return selected; } }
        public void UnSelect() { selected = false; }

        public DS_Dialogue GetDialogue() 
        {
            return character.Sequences[(int)sequenceIndex][dialogueIndex];
        }
    }
}