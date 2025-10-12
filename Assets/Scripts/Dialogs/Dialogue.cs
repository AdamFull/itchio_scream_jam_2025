using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Dialogs
{
    [System.Serializable]
    public class Dialogue
    {
        public string NameNPC = string.Empty;
        public string Text = string.Empty;
        public List<DialogueAnswerChoice> AnswerСhoice = new();
    }
    [System.Serializable]
    public class ChoiceEvent : UnityEvent { }
    
    [System.Serializable]
    public class DialogueAnswerChoice
    {
        public string Answer = string.Empty;
        public ChoiceEvent Action;
    }
}