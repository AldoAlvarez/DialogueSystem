using AGAC.DialogueSystem.Base;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AGAC.DialogueSystem {
    public static class DS_ConversationAdmin
    {
        #region VARIABLES
        public static bool hasActiveConversation { get; private set; }
        private static DS_Conversation current_conversation;
        private static uint current_dialogue = 0;

        public delegate void OnFinishActions();
        public static OnFinishActions OnFinish;

#if UNITY_EDITOR
        public static string ConversationsPath = "Dialogue System/Conversations";
        public static string IconsPath = "Dialogue System/Icons";
#endif
        #endregion

        #region PUBLIC METHODS
        public static void StartConversation(DS_Conversation conversation) 
        {
            if (hasActiveConversation) return;
            hasActiveConversation = true;
            current_conversation = conversation;
            current_dialogue = 0;
        }

        public static void FinishConversation() 
        {
            hasActiveConversation = false;
            current_conversation = null;
            if (OnFinish != null)
                OnFinish();
        }

        public static DS_DialogueData GetNextDialogue() 
        {
            if (!hasActiveConversation) return null;

            DS_DialogueData dialogue = current_conversation[current_dialogue];
            ++current_dialogue;

            if (current_dialogue >= current_conversation.Count)
                FinishConversation();

            return dialogue;
        }
        #endregion

    }
}
