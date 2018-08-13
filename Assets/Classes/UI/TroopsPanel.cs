using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace UI
{
    public class TroopsPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _woodText;
        [SerializeField] private TextMeshProUGUI _stoneText;
        [SerializeField] private TextMeshProUGUI _foodText;
        [SerializeField] private TextMeshProUGUI _waterText;
        [SerializeField] private TextMeshProUGUI _populationText;

        public void SetUI(Dictionary<Supplies, float> items)
        {
            _woodText.text = items[Supplies.Water].ToString();
            _stoneText.text = items[Supplies.Stone].ToString();
            _populationText.text = items[Supplies.Population].ToString();
            _foodText.text = items[Supplies.Food].ToString("0.0kc");
            _waterText.text = items[Supplies.Water].ToString("0.0L");
        }
    }
}
