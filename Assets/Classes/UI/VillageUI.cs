using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageUI : MonoBehaviour {
    public bool IsOpen { get; private set; }

    [SerializeField] private GameObject _villageListUIPrefab;
    private GameObject _villageListUI;

    public void OpenUI()
    {
        if (_villageListUI != null)
        {
            IsOpen = false;
            Destroy(_villageListUI);
            return;
        }
        IsOpen = true;
        _villageListUI = Instantiate(_villageListUIPrefab, transform);
    }
}
