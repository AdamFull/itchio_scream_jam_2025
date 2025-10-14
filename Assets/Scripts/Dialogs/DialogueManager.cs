using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using Commons;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Dialogs
{
    public class DialogueManager : SingletonMonoBehaviour<DialogueManager>
    {
        [Header("Систменое")] [Tooltip("Ключ менеджера джиалогов для проверки")]
        public string DialogueManagerKey = string.Empty;

        [Header("Диалоги персонажей")] [Tooltip("Записываем все диалоги для персонажей")] [SerializeField]
        public SerializedDictionary<string, DialogueList> characterDialogues = new();

        [Header("Необходимые ресурсы для диалогов")] [SerializeField]
        private GameObject dialoguePanel;

        [SerializeField] private TextMeshProUGUI dialogueText;
        [SerializeField] private GameObject choiceMenu;
        private VerticalLayoutGroup choiceMenuGroup;
        [SerializeField] public Button buttonChoicePrefab;

        private List<Button> buttonAction = new();

        private Queue<Dialogue> dialoguesQueue = new();

        private void Start()
        {
            choiceMenuGroup = choiceMenu.GetComponentInChildren<VerticalLayoutGroup>();
            if (choiceMenuGroup == null)
            {
                Debug.LogError("DialogueManager: choiceMenuGroup is null");
            }
        }

        public void StratDialogue(string dialogueKey)
        {
            Debug.Log("Пробуем начать диалог: " + dialogueKey);
            if (characterDialogues.TryGetValue(dialogueKey, out var dialogueList))
            {
                Debug.Log("Начинаем диалог: " + dialogueKey);

                SetDialogueList(dialogueList);
            }
        }

        /// <summary>
        /// Записываем в очередь диалогов и вызываем первый диалог
        /// </summary>
        public void SetDialogueList(DialogueList dialogueList)
        {
            AddDialogueInQueue(dialogueList);
            NextDialogue();
        }

        private void AddDialogueInQueue(DialogueList dialogueList)
        {
            foreach (Dialogue dialogue in dialogueList.dialogues)
            {
                dialoguesQueue.Enqueue(dialogue);
            }

            Debug.Log("Dialogs count:" + dialoguesQueue.Count);
        }

        /// <summary>
        /// Следующая реплика из диалога
        /// </summary>
        public void NextDialogue()
        {
            Dialogue dialogue;
            if (dialoguesQueue.TryDequeue(out dialogue))
            {
                ClearButtons();
                ShowDialogue(dialogue);
            }
        }

        /// <summary>
        /// Отображаем диалог
        /// </summary>
        private void ShowDialogue(Dialogue dialogue)
        {
            dialoguePanel.SetActive(true);
            dialogueText.SetText(dialogue.Text);
            CreateButtonChoice(dialogue.AnswerСhoice);
            BindMenu();
        }

        /// <summary>
        /// Заполняем меню выбора
        /// </summary>
        private void BindMenu()
        {
            foreach (Button button in buttonAction)
            {
                button.transform.SetParent(choiceMenuGroup.transform, false);
            }

            choiceMenu.gameObject.SetActive(true);
        }

        /// <summary>
        /// Создаем кнопки выбора и подписываем их на события
        /// </summary>
        private void CreateButtonChoice(List<DialogueAnswerChoice> choices)
        {
            foreach (var interact in choices)
            {
                var button = SubscribeButton(CreateButton(interact.Answer), interact.Action);
                buttonAction.Add(button);
            }
        }

        /// <summary>
        /// Создаем кнопки
        /// </summary>
        private Button CreateButton(string text)
        {
            var btn = Instantiate(buttonChoicePrefab);
            var tmp = btn.GetComponentInChildren<TextMeshProUGUI>();
            tmp.text = text;
            return btn;
        }

        /// <summary>
        /// Создаем подписку кнопки на переданный action
        /// </summary>
        private Button SubscribeButton(Button button, ChoiceEvent action)
        {
            if (button && action != null)
            {
                button.onClick.AddListener(() => action?.Invoke());
            }

            return button;
        }

        /// <summary>
        /// Очищаем кнопки выбора
        /// </summary>
        private void ClearButtons()
        {
            foreach (var item in buttonAction)
            {
                Destroy(item.transform.gameObject);
            }

            buttonAction.Clear();
        }

        /// <summary>
        /// Закрываем диалог
        /// </summary>
        public void CloseDialogue()
        {
            dialoguePanel.SetActive(false);
        }
    }
}