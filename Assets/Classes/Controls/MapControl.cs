using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UI;

public class MapControl : MonoBehaviour {
    [SerializeField] private GameObject _miniUIPrefab;
    [SerializeField] private GameObject _collectorUIPrefab;
    [SerializeField] private TileBase _villageTile;
    [SerializeField] private TileBase _farmTile;
    [SerializeField] private TileBase _mineTile;
    [SerializeField] private TileBase _lumberMillTile;
    [SerializeField] private TileBase _pumpTile;
    [SerializeField] private TroopUI _troopUI;
    [SerializeField] private MainUI _main;

    private MainUI _mainUI;
    private GameMaster _gameMaster;
    private Canvas _headupDisplay;
    private GameObject _miniUI;
    private GameObject _collectorUI;
    private Tilemap _map;
    private Vector3Int _oldPos;

    private void Start()
    {
        _map = GetComponentInChildren<Tilemap>();
        _map.CompressBounds();
        _gameMaster = GameObject.FindGameObjectWithTag("Master").GetComponent<GameMaster>();
        _headupDisplay = GameObject.FindGameObjectWithTag("HUD").GetComponent<Canvas>();
        _mainUI = GameObject.FindGameObjectWithTag("MainUI").GetComponent<MainUI>();
    }

    void Update () {
        if (_main.IsGamePaused) return;
        if (_troopUI.IsOpen) return;
        if (Input.GetMouseButtonDown(0))
        {
            var pos = Vector3Int.FloorToInt(_map.transform.TransformVector(Camera.main.ScreenToWorldPoint(Input.mousePosition)));
            pos.z = 0;
            Destroy(_miniUI);
            Destroy(_collectorUI);
            if (pos == _oldPos)
            {
                _oldPos = Vector3Int.up * 1000;
                return;
            }
            _oldPos = pos;

            TileBase tile = _map.GetTile(pos);
            if (!_map.cellBounds.Contains(pos)) return;
            CheckTile(tile.name, pos);
        }
	}

    private void CheckTile(string name, Vector3Int pos)
    {
        var newPos = CheckPos(new Vector2(pos.x + 0.5f, pos.y + 2f));
        switch (name)
        {
            case "Village":
                {
                    var village = _gameMaster.VillageSystem.GetVillage(pos);
                    var info = village.GetInfo();
                    _miniUI = Instantiate(_miniUIPrefab, _headupDisplay.transform);
                    _miniUI.GetComponent<UI.MinyUI>().UpdateUI(info);
                    _miniUI.transform.position = newPos;
                    break;
                }
            case "Grass":
                {
                    _map.SetTile(pos, _farmTile);
                    _gameMaster.CollectorSystem.AddCollector(new Collector.CollectorComponent(Supplies.Food, Supplies.Water, pos));
                    break;
                }
            case "Mountain":
                {
                    if (!CheckCost(new KeyValuePair<Supplies, float>(Supplies.Wood, 10))) return;
                    _map.SetTile(pos, _mineTile);
                    _gameMaster.CollectorSystem.AddCollector(new Collector.CollectorComponent(Supplies.Stone, Supplies.Wood, pos));
                    break;
                }
            case "Forest":
                {
                    if (!CheckCost(new KeyValuePair<Supplies, float>(Supplies.Water, 10))) return;
                    _map.SetTile(pos, _lumberMillTile);
                    _gameMaster.CollectorSystem.AddCollector(new Collector.CollectorComponent(Supplies.Wood, Supplies.Water, pos));
                    break;
                }
            case "Lake":
                {
                    if (!CheckCost(new KeyValuePair<Supplies, float>(Supplies.Stone, 10))) return;
                    _map.SetTile(pos, _pumpTile);
                    _gameMaster.CollectorSystem.AddCollector(new Collector.CollectorComponent(Supplies.Water, Supplies.Stone, pos));
                    break;
                }
            default:
                {
                    var collector = _gameMaster.CollectorSystem.GetCollector(pos);
                    var produce = collector.GetProduce();
                    var consume = collector.GetConsume();


                    _collectorUI = Instantiate(_collectorUIPrefab, _headupDisplay.transform);
                    _collectorUI.GetComponent<UI.CollectorUI>().UpdateUI(produce, consume);
                    _collectorUI.transform.position = newPos;
                    break;
                }
        }
    }

    private bool CheckCost(KeyValuePair<Supplies, float> material)
    {
        bool canBuy = true;
        if (_mainUI.TotalItems[material.Key] < material.Value) return false;
        _gameMaster.VillageSystem.ReduceMaterial(material);
        return canBuy;
    }

    private Vector2 CheckPos(Vector2 pos)
    {
        var posX = pos.x;
        if (posX > _map.cellBounds.xMax) posX = (_map.cellBounds.xMax - 1.5f);
        if (posX < _map.cellBounds.xMin) posX = (_map.cellBounds.xMin + 1.5f);
        var posY = pos.y + 1;
        if (posY > _map.cellBounds.yMax) posY = (_map.cellBounds.yMax - 1.5f);
        else posY -= 1;
        return new Vector2(posX, posY);
    }
}
