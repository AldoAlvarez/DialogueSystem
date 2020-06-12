using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace AGAC.DialogueSystem.Base
{
    [Serializable]
    public class DS_Dialogue
    {
        public DS_Dialogue() 
        {
            displayed = true;
            type = DialogueTypeOption.TEXT;
            text = string.Empty;
            audioClip = null;
        }

        #region VARIABLES
        [SerializeField]
        private bool displayed = true;

        [SerializeField]
        private DialogueTypeOption type = DialogueTypeOption.TEXT;
        public DialogueTypeOption Type { get { return type; } } 

        [SerializeField]
        private string text;
        [SerializeField]
        private AudioClip audioClip;
        #endregion

        #region PUBLIC METHODS
        public AudioClip GetAudioClip() 
        {
            if (Type == DialogueTypeOption.TEXT) 
                return null;
            return audioClip;
        }
        public string GetText()
        {
            if (Type == DialogueTypeOption.AUDIO) return string.Empty;
            return text;
        }
        #endregion
    }
}