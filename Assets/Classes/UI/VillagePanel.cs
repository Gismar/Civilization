using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace UI
{
    public class VillagePanel : MonoBehaviour
    {

        [SerializeField] private TextMeshProUGUI _heatText;
        [SerializeField] private TextMeshProUGUI _woodText;
        [SerializeField] private TextMeshProUGUI _stoneText;
        [SerializeField] private TextMeshProUGUI _foodText;
        [SerializeField] private TextMeshProUGUI _waterText;
        [SerializeField] private TextMeshProUGUI _populationText;
        [SerializeField] private TextMeshProUGUI _nameText;

        public void UpdateInfo(Dictionary<Supplies, float> items, string name)
        {
            _woodText.text = items[Supplies.Water].ToString();
            _stoneText.text = items[Supplies.Stone].ToString();
            _populationText.text = items[Supplies.Population].ToString();
            _foodText.text = items[Supplies.Food].ToString("0.0kc");
            _heatText.text = items[Supplies.Heat].ToString("0.0°C");
            _waterText.text = items[Supplies.Water].ToString("0.0L");
            _nameText.text = name;
        }
    }
}