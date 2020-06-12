using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AGAC.DialogueSystem.Tools;
using UnityEditor.Experimental.UIElements.GraphView;
using System.Linq;

namespace AGAC.DialogueSystem.Base
{
    public class DS_Conversation : ScriptableObject
    {
        public DS_Conversation()
        {
            characters = new List<DS_Character>();
            node_sequence = new List<DS_ConversationNode>();
        }

        [SerializeField]
        private List<DS_Character> characters;

        [SerializeField]
        private List<DS_ConversationNode> node_sequence;

        private static DS_DialogueData dialogue = new DS_DialogueData();
        public DS_DialogueData this[uint dialogueIndex]
        {
            get
            {
                if (dialogueIndex >= node_sequence.Count) return null;

                dialogue.Character = node_sequence[(int)dialogueIndex].character;
                dialogue.Dialogue = node_sequence[(int)dialogueIndex].GetDialogue();
                return dialogue;
            }
        }
        public int Count { get { return node_sequence.Count; } }

        #region PUBLIC METHODS
        public void AddCharacter(DS_Character character)
        {
            if (character == null) return;
            if (characters.Count >= 10) return;
            if (characters.Contains(character)) return;
            characters.Add(character);
        }
        public void RemoveCharacter(uint index)
        {
            if (index >= characters.Count) return;

            RemoveNodesWithCharacter(characters[(int)index]);
            characters.RemoveAt((int)index);
        }
        public void RemoveSelectedDialogues()
        {
            Stack<int> selectedDialogues = GetSelectedNodes();
            while (selectedDialogues.Count > 0)
                node_sequence.RemoveAt(selectedDialogues.Pop());
        }
        public void MoveUpSelectedDialogues()
        {
            List<int> selectedDialogues = GetSelectedNodes().ToList<int>();
            selectedDialogues.Reverse();
            foreach (int node in selectedDialogues)
                if (node >= 1)
                    ExchangeNodes(node, node - 1);
        }
        public void MoveDownSelectedDialogues()
        {
            List<int> selectedDialogues = GetSelectedNodes().ToList<int>();
            selectedDialogues.Reverse();
            foreach (int node in selectedDialogues)
                if (node < node_sequence.Count - 1)
                    ExchangeNodes(node, node + 1);
        }
        public void UnSelectAllDialogues()
        {
            foreach (DS_ConversationNode node in node_sequence)
                node.UnSelect();
        }
        #endregion

        #region PRIVATE METHODS
        private Stack<int> GetSelectedNodes()
        {
            Stack<int> selected_nodes = new Stack<int>();
            for (int node = 0; node < node_sequence.Count; ++node)
                if (node_sequence[node].isSelected)
                    selected_nodes.Push(node);
            return selected_nodes;
        }
        private void ExchangeNodes(int nodeA, int nodeB)
        {
            DS_ConversationNode tempNode = node_sequence[nodeA];
            node_sequence[nodeA] = node_sequence[nodeB];
            node_sequence[nodeB] = tempNode;
        }

        private void RemoveNodesWithCharacter(DS_Character character)
        {
            Stack<int> nodes_to_remove = GetNodesToRemove(character);
            foreach (int node in nodes_to_remove)
                node_sequence.RemoveAt(node);
        }

        private Stack<int> GetNodesToRemove(DS_Character character)
        {
            Stack<int> nodes = new Stack<int>();
            DS_ConversationNode node;

            for (int _node = 0; _node < node_sequence.Count; ++_node)
            {
                node = node_sequence[_node];
                if (node.character == character)
                    nodes.Push(_node);
            }
            return nodes;
        }

        #endregion
    }
}