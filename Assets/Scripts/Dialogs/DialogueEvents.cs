using System;
using Commons;
using UnityEngine;

namespace Dialogs
{
    public class DialogueEvents : SingletonMonoBehaviour<DialogueEvents>
    {
        private DialogueManager dialogueManager;


        private void Start()
        {
            dialogueManager = DialogueManager.Instance;
        }

        public void ShowDialogue(DialogueList dialogueList)
        {
            dialogueManager.SetDialogueList(dialogueList);
        }
        
        /// <summary>
        /// Включаем следующий диалог с НПС
        /// </summary>
        public void NextReplica()
        {
            dialogueManager.NextDialogue();
        }
        
        /// <summary>
        /// Закрываем диалог с НПС
        /// </summary>
        public void CloseDialogue()
        {
            dialogueManager.CloseDialogue();
        }   
        
        /// <summary>
        /// Допустим умер
        /// </summary>
        public void EventDiePlayer()
        {
            Debug.Log("Игрок умер");
            CloseDialogue();
        }


    }
}