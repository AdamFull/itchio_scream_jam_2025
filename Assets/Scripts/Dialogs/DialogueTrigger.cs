using System;
using UnityEngine;

namespace Dialogs
{
    public class DialogueTrigger : MonoBehaviour
    {
        private DialogueManager dialogueManager;

        [SerializeField] private string KeyDialogue = string.Empty;
        private void Start()
        {
            dialogueManager = DialogueManager.Instance;
            Debug.Log($"Получен диалог менеджер : {dialogueManager.DialogueManagerKey}");
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                dialogueManager.StratDialogue(KeyDialogue);
            }
        }
    }
}