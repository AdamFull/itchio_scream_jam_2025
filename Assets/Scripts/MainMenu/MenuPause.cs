using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MainMenu
{
    public class MenuPause : MonoBehaviour
    {
        [SerializeField] private Button startGameButton;
        [SerializeField] private Button exitGameButton;
        [SerializeField] private GameObject pauseMenu;

        private void OnEnable()
        {
            startGameButton.onClick.AddListener(() => StartGame());
            exitGameButton.onClick.AddListener(() => ExitGame());
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                pauseMenu.SetActive(true);
                PauseGame();
            }
        }


        public void ExitGame()
        {
            SceneManager.LoadScene("00_MainMenu");
        }

        private void PauseGame()
        {
            Time.timeScale = 0;
        }

        private void StartGame()
        {
            Time.timeScale = 1;
            pauseMenu.SetActive(false);
        }
    }
}