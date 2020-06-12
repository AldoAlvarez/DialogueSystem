using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AGAC.DialogueSystem.Base
{
    public class DS_Character : ScriptableObject   
    {
        public DS_Character() 
        {
            Name = "New Character";
            Sequences = new List<DS_DialogueSequence>() { null };
            id_color = Color.white;
        }

        public string Name;
        public List<DS_DialogueSequence> Sequences;

        [SerializeField]
        private Color id_color;
        [SerializeField]
        private Sprite icon;
        public Color IDcolor 
        {
            get { return id_color; }
            set { SetIDcolor(value); }
        }
        public Sprite Icon
        {
            get
            {
                return GetIcon();
            }
            set 
            {
                SetIcon(value);
            }
        }

        #region PUBLIC METHODS
        public void AddSequence() 
        {
            if (hasEmptySequenceSlot()) return;
            Sequences.Add(null);
        }
        public void RemoveSequence(uint index) 
        {
            if (index >= Sequences.Count) return;
            if (Sequences.Count == 1) Sequences[0] = null;
            else Sequences.RemoveAt((int)index);
        }
        #endregion

        #region PRIVATE METHODS
        private bool hasEmptySequenceSlot() 
        {
            foreach (DS_DialogueSequence sequence in Sequences)
                if (sequence == null) return true;
            return false;
        }

        private void SetIDcolor(Color col) 
        {
            //id_color = GetClampedColor(col);
            id_color = col;
            id_color.a = 1;
        }
        private Color GetClampedColor(Color col) 
        {
            for (int i = 0; i < 3; ++i)
                if (col[i] < 0.2f)
                    col[i] = 0.2f;
            return col;
        }

        private Sprite GetIcon() 
        {
            if (icon == null)
                icon = GetDefaultIcon();
            return icon;
        }
        private void SetIcon(Sprite _icon) 
        {
            if (_icon == null)
                icon = GetDefaultIcon();
            else
                icon = _icon;
        }
        private Sprite GetDefaultIcon() 
        {
            return Resources.Load<Sprite>("Dialogue System/Icons/defaultPerson");
        }
        #endregion
    }
}