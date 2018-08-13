using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

namespace UI
{
    public class TroopUI : MonoBehaviour
    {
        [SerializeField] private GameObject _raidListUIPrefab;
        [SerializeField] private GameObject _troopsListUIPrefab;
        [SerializeField] private GameObject _raidGroupPanelPrefab;
        [SerializeField] private GameObject _troopPanelPrefab;

        public bool IsOpen { get; private set; }

        private GameObject _troopUI;
        private GameObject _raidUI;
        private List<GameObject> _raidGroupPanels;
        private List<GameObject> _troopPanels;
        private GameMaster _gameMaster;

        private void Start()
        {
            _gameMaster = GameObject.FindGameObjectWithTag("Master").GetComponent<GameMaster>();
            _raidGroupPanels = new List<GameObject>();
            _troopPanels = new List<GameObject>();
        }

        public void OpenRaidUI()
        {
            if (_raidUI != null)
            {
                IsOpen = false;
                Destroy(_troopUI);
                Destroy(_raidUI);
                return;
            }
            IsOpen = true;
            _raidUI = Instantiate(_raidListUIPrefab, transform);

            if (_raidGroupPanels.Count > 0)
                for (int i = _raidGroupPanels.Count - 1; i >= 0; i--)
                {
                    Destroy(_raidGroupPanels[i]);
                }

            _raidGroupPanels = new List<GameObject>();
            foreach (var raid in _gameMaster.TroopSystem.TroopGroup)
            {
                var temp = Instantiate(_raidGroupPanelPrefab, _raidUI.transform.GetChild(0).GetChild(0));
                temp.GetComponent<TroopsPanel>().SetUI(raid.GetInfo(), raid.ElapsedTime);
                temp.GetComponent<Button>().onClick.AddListener(delegate { OpenRaidTroopsUI(raid.RaidID); });
                _raidGroupPanels.Add(temp);
            }
        }

        public void OpenRaidTroopsUI(int id)
        {
            if(_troopUI != null) Destroy(_troopUI);
            _troopUI = Instantiate(_troopsListUIPrefab, transform);

            if (_troopPanels.Count > 0)
                for (int i = _troopPanels.Count - 1; i >= 0; i--)
                {
                    Debug.Log(i);
                    Destroy(_troopPanels[i], 0.5f);
                }

            _troopPanels = new List<GameObject>();

            var raidGroup = _gameMaster.TroopSystem.TroopGroup.First(x => x.RaidID == id);
            Debug.Log(raidGroup);
            foreach (var troop in raidGroup.Troops)
            {
                var temp = Instantiate(_troopPanelPrefab, _troopUI.transform.GetChild(0).GetChild(0));
                var items = new Dictionary<Supplies, float>()
                {
                    { Supplies.Food, troop.Food },
                    { Supplies.Water, troop.Water },
                    { Supplies.Stone, troop.Stone },
                    { Supplies.Wood, troop.Wood }
                };
                temp.GetComponent<TroopPanel>().SetUI(items);
                _troopPanels.Add(temp);
            }
        }
    }
}
