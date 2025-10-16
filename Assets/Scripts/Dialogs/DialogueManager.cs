using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using Commons;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Dialogs
{
    public class DialogueManager : SingletonMonoBehaviour<DialogueManager>
    {
        [Header("Систменое")] [Tooltip("Ключ менеджера джиалогов для проверки")]
        public string DialogueManagerKey = string.Empty;

        [Range(0.01f, 0.1f)]
        [Tooltip("Скорость печати текста")] public float SpeedTyping = 0.1f;

        [Header("Диалоги персонажей")] [Tooltip("Записываем все диалоги для персонажей")] [SerializeField]
        public SerializedDictionary<string, DialogueList> characterDialogues = new();

        [Header("Необходимые ресурсы для диалогов")] [SerializeField]
        private GameObject dialoguePanel;

        [SerializeField] private TextMeshProUGUI dialogueText;
        [SerializeField] private GameObject choiceMenu;
        private VerticalLayoutGroup choiceMenuGroup;
        [SerializeField] public Button buttonChoicePrefab;
        [SerializeField] public ScrollRect scrollRect;
        private List<Button> buttonAction = new();

        private Queue<Dialogue> dialoguesQueue = new();

        private DialoguesDataBase dialoguesDataBase = new();
        private Coroutine myCoroutine;
        private bool isTyping = false;
        private void Start()
        {
            choiceMenuGroup = choiceMenu.GetComponentInChildren<VerticalLayoutGroup>();
            if (choiceMenuGroup == null)
            {
                Debug.LogError("DialogueManager: choiceMenuGroup is null");
            }
        }

        public void StratDialogue(string dialogueKey, Transform dialogueInitiator)
        {
            Debug.Log("Пробуем начать диалог: " + dialogueKey);
            if (characterDialogues.TryGetValue(dialogueKey, out var dialogueList))
            {
                Debug.Log("Начинаем диалог: " + dialogueKey);

                DialogueEvents.Instance.StopPlayerMove(dialogueInitiator);

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

        private void AddDialogueInQueue(DialogueList dialogueListKey)
        {
            foreach (Dialogue dialogue in dialogueListKey.dialogues)
            {
                if (dialoguesDataBase.dialogues.TryGetValue(dialogue.KeyDB, out var textMeshProUGUI)) ;
                dialogue.TextMeshPro = textMeshProUGUI;
                dialoguesQueue.Enqueue(dialogue);
            }
        }

        /// <summary>
        /// Следующая реплика из диалога
        /// </summary>
        public void NextDialogue()
        {
            if (myCoroutine != null)
            {
                StopCoroutine(myCoroutine);
                Debug.Log("Корутина остановлена по ссылке");
            }
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
            dialogueText.maxVisibleCharacters = 0;
            dialogueText.text = (dialogue.TextMeshPro.text);
            choiceMenu.gameObject.SetActive(false);
            myCoroutine = StartCoroutine(TextVisible(dialogue.AnswerСhoice));
            // CreateButtonChoice(dialogue.AnswerСhoice);
            // BindMenu();
        }

        /// <summary>
        /// Печать текста по буквам
        /// </summary>
        private IEnumerator TextVisible(List<DialogueAnswerChoice> choices)
        {
            isTyping = true;
            int totalVisibleCharacters = dialogueText.text.Length;
            int counter = 0;
            float stepScroll = 1f / (totalVisibleCharacters / 90f);
            float scrolLine = 1f;
            int stepSrolled = 1;

            while (true)
            {
                int visibleCount = counter % (totalVisibleCharacters + 1);
                dialogueText.maxVisibleCharacters = visibleCount;

                if (scrollRect != null && (visibleCount / 90f) > (2 + stepSrolled))
                {
                    scrolLine -= stepScroll;
                    scrollRect.verticalNormalizedPosition = scrolLine;
                    stepSrolled++;
                }

                if (visibleCount >= totalVisibleCharacters || Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
                {
                    dialogueText.maxVisibleCharacters = totalVisibleCharacters;
                    break;
                }

                // Roll dices
                counter += Random.Range(1, 5);
                float waitInSec = Random.Range(0.005f, SpeedTyping);
                yield return new WaitForSeconds(waitInSec);
            }

            isTyping = false;
            CreateButtonChoice(choices);
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