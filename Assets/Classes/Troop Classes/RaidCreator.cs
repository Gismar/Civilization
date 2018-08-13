using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class RaidCreator : MonoBehaviour {

    private Dictionary<Supplies, float> _startingItems;
    private Dictionary<Supplies, float> _withdrawItems;
    private Dictionary<Supplies, float> _depositItems;
    private GameMaster _gameMaster;

    [SerializeField] private TextMeshProUGUI[] _startItemTexts;
    [SerializeField] private TextMeshProUGUI[] _withdrawItemTexts;
    [SerializeField] private TextMeshProUGUI[] _depositItemsTexts;
    [SerializeField] private TextMeshProUGUI _totalTimeText;
    [SerializeField] private GameObject _errorText;

    public Village.VillageComponent StartingVillage { get; set; }
    public ICollect Destination { get; set; }

    public void Setup () {
        _startingItems = new Dictionary<Supplies, float>();
        _withdrawItems = new Dictionary<Supplies, float>();
        _depositItems = new Dictionary<Supplies, float>();
        _gameMaster = GameObject.FindGameObjectWithTag("Master").GetComponent<GameMaster>();
        _totalTimeText.text = (Mathf.FloorToInt(Vector3Int.Distance(Destination.Position, StartingVillage.Position) * 10) * 2).ToString("0s");

        foreach (var data in System.Enum.GetValues(typeof(Supplies)).Cast<Supplies>())
        {
            _startingItems.Add(data, 0);
            _withdrawItems.Add(data, 0);
            _depositItems.Add(data, 0);
        }
	}

    private void Update()
    {
        _startItemTexts[0].text = _startingItems[Supplies.Population].ToString();
        _startItemTexts[1].text = _startingItems[Supplies.Stone].ToString();
        _startItemTexts[2].text = _startingItems[Supplies.Wood].ToString();
        _startItemTexts[3].text = _startingItems[Supplies.Water].ToString();
        _startItemTexts[4].text = _startingItems[Supplies.Food].ToString();

        _withdrawItemTexts[0].text = _withdrawItems[Supplies.Stone].ToString();
        _withdrawItemTexts[1].text = _withdrawItems[Supplies.Wood].ToString();
        _withdrawItemTexts[2].text = _withdrawItems[Supplies.Water].ToString();
        _withdrawItemTexts[3].text = _withdrawItems[Supplies.Food].ToString();

        _depositItemsTexts[0].text = _depositItems[Supplies.Stone].ToString();
        _depositItemsTexts[1].text = _depositItems[Supplies.Wood].ToString();
        _depositItemsTexts[2].text = _depositItems[Supplies.Water].ToString();
        _depositItemsTexts[3].text = _depositItems[Supplies.Food].ToString();
    }

    private float GetFoodRequire(int time, int population)
    {
        var cycle = Mathf.CeilToInt(time / 100f);
        var foodPerPerson = cycle * 3f;
        return foodPerPerson * population;
    }

    private float WaterRequired(int time, int population)
    {
        var cycle = Mathf.CeilToInt(time / 100f);
        var waterPerPeson = cycle * 2.5f;
        return waterPerPeson * population;
    }

    public void StartRaid()
    {
        if(_startingItems[Supplies.Population] == 0)
        {
            ThrowErrorText("Need atleast 1 person");
            return;
        }
        int travelTime = Mathf.CeilToInt(Vector3Int.Distance(StartingVillage.Position, Destination.Position) * 10) * 2;
        var population = (int) _startingItems[Supplies.Population];
        float required = GetFoodRequire(travelTime, population);

        if (_startingItems[Supplies.Food] < required)
        {
            ThrowErrorText($"Not enough food, need atleast {(required - _startingItems[Supplies.Food]).ToString("0.0kc")} more food");
            return;
        }
        required = WaterRequired(travelTime, population);
        if(_startingItems[Supplies.Water] < required)
        {
            ThrowErrorText($"Not enough water, need atleast {(required - _startingItems[Supplies.Water]).ToString("0.0L")} more water");
            return;
        }

        var temp = new List<Troop.TroopEntity>();
        for (int i = 0; i < _startingItems[Supplies.Population]; i++)
        {
            temp.Add(new Troop.TroopEntity(
                _startingItems[Supplies.Food] / population,
                _startingItems[Supplies.Water] / population,
                _startingItems[Supplies.Stone] / population,
                _startingItems[Supplies.Wood] / population));
        }
        _gameMaster.TroopSystem.AddGroup(Mathf.FloorToInt(travelTime / 2f), temp,
            _withdrawItems, _depositItems, Destination, StartingVillage);
        Destroy(this.gameObject);
    }

    private void ThrowErrorText(string message)
    {
        var temp = Instantiate(_errorText, transform.parent);
        temp.GetComponent<TextMeshProUGUI>().text = message;
        Destroy(temp, 1f);
    }

    public void AddToStart(int item)
    {
        if (item == 0)
        {
            _startingItems[(Supplies)item] = IsPopulationCapped(Mathf.FloorToInt(_startingItems[Supplies.Population] + 1f));
            return;
        }
        _startingItems[(Supplies)item] = IsCapped(Mathf.FloorToInt(_startingItems[(Supplies)item] + 1));
    }
    public void SubFromStart(int item)
    {
        _startingItems[(Supplies)item] = IsNegative(Mathf.FloorToInt(_startingItems[(Supplies) item] - 1));
    }

    public void AddToWithdraw(int item)
    {
        _withdrawItems[(Supplies)item] = IsCapped(Mathf.FloorToInt(_withdrawItems[(Supplies)item] + 1));
    }
    public void SubFromWithdraw(int item)
    {
        _withdrawItems[(Supplies)item] = IsNegative(Mathf.FloorToInt(_withdrawItems[(Supplies)item] - 1)); ;
    }

    public void AddToDeposit(int item)
    {
        _depositItems[(Supplies)item] = IsCapped(Mathf.FloorToInt(_depositItems[(Supplies)item] + 1));
    }
    public void SubFromDeposit(int item)
    {
        _depositItems[(Supplies)item] = IsNegative(Mathf.FloorToInt(_depositItems[(Supplies)item] - 1)); ;
    }

    private int IsNegative(int value)
    {
        if (value < 0) value = 0;
        return value;
    }

    private int IsCapped(int value)
    {
        int max = Mathf.FloorToInt(20 * _startingItems[Supplies.Population]);
        if (value > max) value = max;
        return value;
    }

    private int IsPopulationCapped(int value)
    {
        var pop = Mathf.FloorToInt(StartingVillage.GetInfo()[Supplies.Population]);
        if (pop < value) value = pop;
        return value;
    }
}
