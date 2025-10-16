using System;
using UnityEngine;

namespace Dialogs
{
    public class DialogueTrigger : MonoBehaviour
    {
        private DialogueManager dialogueManager;
        private bool dialogueTriggered = false;
        
        [SerializeField] private string KeyDialogue = string.Empty;
        [SerializeField] private Transform dialogueInitiator;
        private void Start()
        {
            dialogueManager = FindFirstObjectByType<DialogueManager>();
            Debug.Log($"Получен диалог менеджер : {dialogueManager.DialogueManagerKey}");
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log($"Старт диалога: {KeyDialogue}");
            if (other.tag == "Player" && !dialogueTriggered)
            {
                dialogueManager.StratDialogue(KeyDialogue, dialogueInitiator);
                dialogueTriggered = true;
            }
        }
    }
}