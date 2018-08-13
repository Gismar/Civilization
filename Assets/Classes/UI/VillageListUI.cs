using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class VillageListUI : MonoBehaviour
    {
        [SerializeField] private GameObject _villagePanelPrefab;

        private GameObject _villageUI;
        private List<GameObject> _panels;
        private GameMaster _gameMaster;

        void Start()
        {
            _gameMaster = GameObject.FindGameObjectWithTag("Master").GetComponent<GameMaster>();
            _panels = new List<GameObject>();
            UpdateUI();
        }

        public void UpdateUI()
        {
            if (_panels.Count > 0)
                for (int i = _panels.Count - 1; i >= 0; i--)
                    Destroy(_panels[i]);

            _panels = new List<GameObject>();

            foreach (var village in _gameMaster.VillageSystem.Villages)
            {
                var temp = Instantiate(_villagePanelPrefab, transform.GetChild(0).GetChild(0));
                temp.GetComponent<VillagePanel>().UpdateInfo(village.GetInfo(), village.GetName());
                _panels.Add(temp);
            }
        }
    }
}