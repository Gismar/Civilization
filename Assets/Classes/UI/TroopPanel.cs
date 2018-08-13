using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace UI
{
    public class TroopPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _woodText;
        [SerializeField] private TextMeshProUGUI _stoneText;
        [SerializeField] private TextMeshProUGUI _foodText;
        [SerializeField] private TextMeshProUGUI _waterText;

        public void SetUI(Dictionary<Supplies, float> supplies)
        {
            _stoneText.text = supplies[Supplies.Stone].ToString();
            _woodText.text = supplies[Supplies.Wood].ToString();
            _foodText.text = supplies[Supplies.Food].ToString("0.0kc");
            _waterText.text = supplies[Supplies.Water].ToString("0.0L");
        }
    }
}
