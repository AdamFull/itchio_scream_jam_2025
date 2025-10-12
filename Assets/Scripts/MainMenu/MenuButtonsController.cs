using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MainMenu
{
    public class MenuButtonsController : MonoBehaviour
    {
        [SerializeField] private Button _startGameButton;
        [SerializeField] private Button _exitGameButton;

        private void OnEnable()
        {
            _startGameButton.onClick.AddListener(() => SceneManager.LoadScene("SampleScene"));
            _exitGameButton.onClick.AddListener(() => Application.Quit());
        }
        private void Start()
        {
        }
    }
}
