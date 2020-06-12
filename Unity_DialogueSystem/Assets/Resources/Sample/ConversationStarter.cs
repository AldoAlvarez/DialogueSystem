using AGAC.DialogueSystem;
using AGAC.DialogueSystem.Base;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConversationStarter : MonoBehaviour
{
    private void Start()
    {
        DS_ConversationAdmin.OnFinish += HideUI;
        HideUI();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)) 
        {
            if (!DS_ConversationAdmin.hasActiveConversation) 
            {
                DS_ConversationAdmin.StartConversation(conversation);
                ShowUI();
            }

            DS_DialogueData dialogue_data = DS_ConversationAdmin.GetNextDialogue();
            characterIcon.sprite = dialogue_data.Character.Icon;
            dialogueDisplay.text = dialogue_data.Dialogue.GetText();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            DS_ConversationAdmin.FinishConversation();
        }
    }

    #region VARIABLES
    [SerializeField] private Text dialogueDisplay;
    [SerializeField] private Image characterIcon;
    [SerializeField] private DS_Conversation conversation;
    #endregion

    #region PRIVATE METHODS
    private void HideUI() { SetUI(false); }
    private void ShowUI() { SetUI(true); }

    private void SetUI(bool state) 
    {
        dialogueDisplay.gameObject.SetActive(state);
        characterIcon.gameObject.SetActive(state);
    }
    #endregion
}
