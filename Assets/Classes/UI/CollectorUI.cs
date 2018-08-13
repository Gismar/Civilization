using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace UI
{
    public class CollectorUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _produceText;
        [SerializeField] private TextMeshProUGUI _consumeText;
        [SerializeField] private Image _produceImage;
        [SerializeField] private Image _consumeImage;
        [SerializeField] private Sprite[] _icons;

        public void UpdateUI(KeyValuePair<Supplies, float> produce, KeyValuePair<Supplies, float> consume)
        {
            _produceText.text = FormatMaterial(produce.Key, produce.Value);
            _consumeText.text = FormatMaterial(consume.Key, consume.Value);

            _produceImage.sprite = GetSprite(produce.Key);
            _consumeImage.sprite = GetSprite(consume.Key);
        }

        private string FormatMaterial(Supplies material, float amount)
        {
            switch (material)
            {
                case Supplies.Stone:
                case Supplies.Wood:
                case Supplies.Population:
                default:
                    return amount.ToString();
                case Supplies.Water:
                    return amount.ToString("0L");
                case Supplies.Food:
                    return amount.ToString("0kc");
                case Supplies.Heat:
                    return amount.ToString("0°C");
            }
        }

        private Sprite GetSprite(Supplies index)
        {
            return _icons[(int)index];
        }
    }
}
