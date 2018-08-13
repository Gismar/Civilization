using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace UI
{
    public class MinyUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _foodText;
        [SerializeField] private TextMeshProUGUI _populationText;
        [SerializeField] private TextMeshProUGUI _waterText;
        [SerializeField] private TextMeshProUGUI _woodText;
        [SerializeField] private TextMeshProUGUI _stoneText;
        [SerializeField] private TextMeshProUGUI _heatText;

        public void UpdateUI(Dictionary<Supplies, float> dict)
        {
            _stoneText.text = dict[Supplies.Stone].ToString();
            _populationText.text = dict[Supplies.Population].ToString();
            _woodText.text = dict[Supplies.Wood].ToString();
            _foodText.text = dict[Supplies.Food].ToString("0kc");
            _waterText.text = dict[Supplies.Water].ToString("0L");
            _heatText.text = dict[Supplies.Heat].ToString("0.0°C");
        }
    }
}