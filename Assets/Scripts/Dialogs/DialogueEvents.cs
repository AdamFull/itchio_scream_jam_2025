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

        /// <summary>
        /// Включаем следующий диалог с НПС
        /// </summary>
        public void NextReplica()
        {
            StopPlayerMove();
            dialogueManager.NextDialogue();
        }

        /// <summary>
        /// Закрываем диалог с НПС
        /// </summary>
        public void CloseDialogue()
        {
            StartPlayerMove();
            dialogueManager.CloseDialogue();
        }

        /// <summary>
        /// Допустим умер
        /// </summary>
        public void EventDiePlayer(string keyDialogyeNegative)
        {
            Debug.Log("Игрок умер");
            CloseDialogue();
        }
        
        /// <summary>
        /// Обнуляем движение игрока
        /// </summary>
        public void StopPlayerMove()
        {
        }
        
        /// <summary>
        /// Возвращаем движение игрока
        /// </summary>
        public void StartPlayerMove()
        {
        }
    }
}