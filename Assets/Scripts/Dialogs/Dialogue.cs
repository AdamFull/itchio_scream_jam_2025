using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Dialogs
{
    [System.Serializable]
    public class Dialogue
    {
        [FormerlySerializedAs("Text")] public string KeyDB = string.Empty;
        [HideInInspector] public TextMeshProUGUI TextMeshPro = new TextMeshProUGUI();
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