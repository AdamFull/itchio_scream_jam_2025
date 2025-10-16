using System;
using UnityEngine;

namespace Dialogs
{
    public class Testing : MonoBehaviour
    {
        public DialogueList dialogueList;
        public GameObject testCreateCharacter;

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