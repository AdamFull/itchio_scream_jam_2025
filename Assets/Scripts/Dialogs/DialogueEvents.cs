using System;
using Commons;
using UnityEngine;

namespace Dialogs
{
    public class DialogueEvents : MonoBehaviour
    {
        public DialogueManager dialogueManager;

        /// <summary>
        /// Включаем следующий диалог с НПС
        /// </summary>
        public void NextReplica()
        {
            StopPlayerMove(null);
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
        public void EventDiePlayer(string keyDialogueNegative)
        {
            Debug.Log("Игрок умер");
            CloseDialogue();
        }
        
        /// <summary>
        /// Обнуляем движение игрока
        /// </summary>
        public void StopPlayerMove(Transform dialogueInitiator)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                DoomLikeCharacterController controller = playerObj.GetComponent<DoomLikeCharacterController>();
                controller.SetBlockAllInput(true);
                controller.LockOnTarget(dialogueInitiator);
            }
        }
        
        /// <summary>
        /// Возвращаем движение игрока
        /// </summary>
        public void StartPlayerMove()
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                DoomLikeCharacterController controller = playerObj.GetComponent<DoomLikeCharacterController>();
                controller.SetBlockAllInput(false);
                controller.UnlockTarget();
            }
        }
    }
}