using System;
using UnityEngine;

namespace Dialogs
{
    public class Testing : MonoBehaviour
    {
        private DialogueEvents dialogueEvents;
        public DialogueList dialogueList;
        private void Start()
        {
            dialogueEvents = DialogueEvents.Instance;
        }

        private void Update()
        {
            // Будто попали в тригер
            if (Input.GetKeyDown(KeyCode.Space))
            {
                dialogueEvents.ShowDialogue(dialogueList);
            }
        }
    }
}