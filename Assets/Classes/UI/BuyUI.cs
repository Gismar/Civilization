using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace UI {
    public class BuyUI : MonoBehaviour {

        [SerializeField] private TextMeshProUGUI _itemText;
        [SerializeField] private Image _materialImage;
        [SerializeField] private Sprite[] _icons;

        [SerializeField] private Button BuyButton;
        public Vector3Int Position { get; set; }
        private MapControl _map;

        public void Setup(KeyValuePair<Supplies, float> material, MapControl map)
        {
            _materialImage.sprite = GetSprite(material.Key);
            _itemText.text = FormatMaterial(material.Key, material.Value);
            _map = map;
        }

        public void CallBuy()
        {
            Debug.Log("buy");
            _map.Buy();
        }

        public void DestroySelf()
        {
            Destroy(transform.gameObject);
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
