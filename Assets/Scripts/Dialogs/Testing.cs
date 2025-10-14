using System;
using UnityEngine;

namespace Dialogs
{
    public class Testing : MonoBehaviour
    {
        private DialogueEvents dialogueEvents;
        public DialogueList dialogueList;
        public GameObject testCreateCharacter; 
        private void Start()
        {
            dialogueEvents = DialogueEvents.Instance;
        }

        private void Update()
        {
            // Будто попали в тригер
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // dialogueEvents.ShowDialogue(dialogueList);
                Instantiate(testCreateCharacter);
            }
        }
    }
}