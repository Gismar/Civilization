using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UI;
using TMPro;

public class MapControl : MonoBehaviour {
    [SerializeField] private GameObject _miniUIPrefab;
    [SerializeField] private GameObject _collectorUIPrefab;
    [SerializeField] private GameObject _errorText;
    [SerializeField] private GameObject _buyUIPrefab;
    [SerializeField] private GameObject _raidCreatorPrefab;
    [SerializeField] private TileBase _villageTile;
    [SerializeField] private TileBase _farmTile;
    [SerializeField] private TileBase _mineTile;
    [SerializeField] private TileBase _lumberMillTile;
    [SerializeField] private TileBase _pumpTile;
    [SerializeField] private TileBase _grassTile;
    [SerializeField] private TroopUI _troopUI;
    [SerializeField] private MainUI _main;

    private MainUI _mainUI;
    private GameMaster _gameMaster;
    private Canvas _headupDisplay;
    private GameObject _miniUI;
    private GameObject _collectorUI;
    private GameObject _buyUI;
    private GameObject _raidCreator;
    private Tilemap _map;
    private Vector3Int _oldPos;
    private KeyValuePair<Supplies, float> buyingMaterial;
    private ICollect _collector;
    private bool _didBuy;
    private bool _isSelecting;

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
        if (_buyUI != null) return;
        if (_raidCreator != null) return;
        if (Input.GetMouseButtonDown(1))
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
            if (_isSelecting)
            {
                if (tile != _villageTile) return;
                FinishRaidSetup(pos);
                return;
            }
            if (!_map.cellBounds.Contains(pos)) return;
            if (tile != _grassTile) return;
            OpenBuyUI(new KeyValuePair<Supplies, float>(Supplies.Population, 2), pos);
        }
        if (_collectorUI != null) return;
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
                    OpenBuyUI(new KeyValuePair<Supplies, float>(Supplies.Food, 1), pos);
                    break;
                }
            case "Mountain":
                {
                    OpenBuyUI(new KeyValuePair<Supplies, float>(Supplies.Wood, 1), pos);
                    break;
                }
            case "Forest":
                {
                    OpenBuyUI(new KeyValuePair<Supplies, float>(Supplies.Water, 1), pos);
                    break;
                }
            case "Lake":
                {
                    OpenBuyUI(new KeyValuePair<Supplies, float>(Supplies.Stone, 1), pos);
                    break;
                }
            default:
                {
                    var collector = _gameMaster.CollectorSystem.GetCollector(pos);
                    _collector = collector;
                    var produce = collector.GetProduce();
                    var consume = collector.GetConsume();


                    _collectorUI = Instantiate(_collectorUIPrefab, _headupDisplay.transform);
                    _collectorUI.GetComponent<CollectorUI>().UpdateUI(produce, consume, this);
                    _collectorUI.transform.position = newPos;
                    break;
                }
        }
    }

    public void SetupRaid()
    {
        Destroy(_collectorUI);
        _isSelecting = true;
    }

    void FinishRaidSetup(Vector3Int pos)
    {
        Debug.Log("Creating Raid");
        if (_raidCreator != null)
        {
            Destroy(_raidCreator);
        }
        _raidCreator = Instantiate(_raidCreatorPrefab, _mainUI.transform);
        _raidCreator.GetComponent<RaidCreator>().StartingVillage = _gameMaster.VillageSystem.GetVillage(pos);
        _raidCreator.GetComponent<RaidCreator>().Destination = _collector;
        _raidCreator.GetComponent<RaidCreator>().Setup();
        _isSelecting = false;
    }

    private void OpenBuyUI(KeyValuePair<Supplies, float> material, Vector3Int position)
    {
        if (_buyUI != null)
        {
            Destroy(_buyUI);
            buyingMaterial = new KeyValuePair<Supplies, float>();
            return;
        }
        var newPos = CheckPos(new Vector2(position.x + 0.5f, position.y + 2f));
        buyingMaterial = material;
        _buyUI = Instantiate(_buyUIPrefab, _headupDisplay.transform);
        _buyUI.GetComponent<BuyUI>().Position = position;
        _buyUI.transform.position = newPos;
        _buyUI.GetComponent<BuyUI>().Setup(material, this);
    }

    private void ThrowErrorText(string message)
    {
        var temp = Instantiate(_errorText, _headupDisplay.transform);
        temp.GetComponent<TextMeshProUGUI>().text = message;
        Destroy(temp, 1f);
    }

    public void Buy()
    {
        Debug.Log("buying");
        if (!CheckCost(buyingMaterial))
        {
            if (buyingMaterial.Key == Supplies.Population)
                ThrowErrorText("Cannot leave a village alone");
            else 
                ThrowErrorText($"Not Enough {buyingMaterial.Key.ToString()} to build");
            return;
        }
        TileBase temp = null;

        switch (buyingMaterial.Key)
        {
            case Supplies.Stone:
                temp = _pumpTile;
                _gameMaster.CollectorSystem.AddCollector(new Collector.CollectorComponent(Supplies.Water, Supplies.Stone, _buyUI.GetComponent<BuyUI>().Position));
                break;
            case Supplies.Water:
                temp = _lumberMillTile;
                _gameMaster.CollectorSystem.AddCollector(new Collector.CollectorComponent(Supplies.Wood, Supplies.Water, _buyUI.GetComponent<BuyUI>().Position));
                break;
            case Supplies.Wood:
                temp = _mineTile;
                _gameMaster.CollectorSystem.AddCollector(new Collector.CollectorComponent(Supplies.Stone, Supplies.Wood, _buyUI.GetComponent<BuyUI>().Position));
                break;
            case Supplies.Population:
                temp =  _villageTile;
                _gameMaster.VillageSystem.AddVillage(new Village.VillageComponent(_buyUI.GetComponent<BuyUI>().Position, 2, new Village.VillageEntity(5, 5, 5, 5, "Base")));
                break;
            case Supplies.Food:
                temp = _farmTile;
                _gameMaster.CollectorSystem.AddCollector(new Collector.CollectorComponent(Supplies.Food, Supplies.Water, _buyUI.GetComponent<BuyUI>().Position));
                break;
        }

        _map.SetTile(_buyUI.GetComponent<BuyUI>().Position, temp);
        Destroy(_buyUI);
    }

    private bool CheckCost(KeyValuePair<Supplies, float> material)
    {
        bool canBuy = true;
        if (material.Key == Supplies.Population)
            if (_mainUI.TotalItems[material.Key] < material.Value + 1) return false;
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
