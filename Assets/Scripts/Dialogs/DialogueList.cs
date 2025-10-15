using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogs
{
    [System.Serializable]
    public class DialogueList : MonoBehaviour
    {
        public string NameNPC = String.Empty;
        public List<Dialogue> dialogues = new ();
    }
}