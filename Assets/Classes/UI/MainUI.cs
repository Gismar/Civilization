using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace UI
{
    public class MainUI : MonoBehaviour
    {
        [SerializeField] private Button _troopsButton;
        [SerializeField] private Button _villageButton;
        [SerializeField] private Button _pauseButton;
        [SerializeField] private MusicControl _music;
        [SerializeField] private TextMeshProUGUI _stone;
        [SerializeField] private TextMeshProUGUI _wood;
        [SerializeField] private TextMeshProUGUI _food;
        [SerializeField] private TextMeshProUGUI _water;
        [SerializeField] private TextMeshProUGUI _population;

        private GameMaster _gameMaster;
        private int _timeScale = 0;

        public bool IsGamePaused { get; private set; }
        public Dictionary<Supplies, float> TotalItems { get; private set; }

        private void Start()
        {
            IsGamePaused = true;
            Time.timeScale = 0;
            _troopsButton.interactable = false;
            _villageButton.interactable = false;
            _pauseButton.interactable = false;
            _gameMaster = GameObject.FindGameObjectWithTag("Master").GetComponent<GameMaster>();
        }

        public void StartGame()
        {
            Time.timeScale = 1;
            IsGamePaused = false;
            _troopsButton.interactable = !IsGamePaused;
            _villageButton.interactable = !IsGamePaused;
            _pauseButton.interactable = true;
        }

        public void DeleteButton(GameObject button)
        {
            Destroy(button);
        }

        public void ResetState(Button button)
        {
            button.interactable = false;
            button.interactable = true;
        }

        public void PauseGame()
        {
            IsGamePaused = !IsGamePaused;
            _music.PauseMusic(IsGamePaused);
            _troopsButton.interactable = !IsGamePaused;
            _villageButton.interactable = !IsGamePaused;
            _timeScale = 1 - _timeScale;
            Time.timeScale = _timeScale;
        }

        private void Update()
        {
            TotalItems = _gameMaster.VillageSystem.GetAllVillageInfo();
            _population.text = TotalItems[Supplies.Population].ToString();
            _wood.text = TotalItems[Supplies.Wood].ToString();
            _stone.text = TotalItems[Supplies.Stone].ToString();
            _food.text = TotalItems[Supplies.Food].ToString("0.0kc");
            _water.text = TotalItems[Supplies.Water].ToString("0.0L");
        }
    }
}
