using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour {
    [SerializeField] private GameObject _nightPanel;
    [SerializeField] private CameraControl _mainCamera;
    [SerializeField] private Image _bar;
    public Village.VillageSystem VillageSystem { get; private set; }
    public Troop.TroopSystem TroopSystem { get; private set; }
    public Collector.CollectorSystem CollectorSystem { get; private set; }
    
    private ITime[] _systems;
    private int _tick;
    private int _count;

	void Awake () {
        VillageSystem = new Village.VillageSystem();
        TroopSystem = new Troop.TroopSystem();
        CollectorSystem = new Collector.CollectorSystem();
        _systems = new ITime[] { VillageSystem, TroopSystem, CollectorSystem, _mainCamera};
        _tick = 0;
	}
	
	void Update () {
        _bar.rectTransform.sizeDelta = new Vector2(-((Time.timeSinceLevelLoad % 10) / 10f * (1920f / Screen.width) * Screen.width) , 32);
        Debug.Log(Screen.width);
        var time = Mathf.FloorToInt(Time.timeSinceLevelLoad / 10f);
        if (_tick == time) return;
        _tick = time;

        _count = _tick % 20;

        if (_count == 0) //Cycle
        {
            Debug.Log("Cycle Ended");
            for(int i = 0; i < _systems.Length; i++)
            {
                _systems[i].PerCycle();
            }
        }
        else if(_count == 19) //Night
        {
            Debug.Log("Night Ended");
            _nightPanel.SetActive(false);
            for (int i = 0; i < _systems.Length; i++)
            {
                _systems[i].PerNight();
            }
        }
        else if(_count == 10) // Day
        {
            Debug.Log("Day Ended");
            _nightPanel.SetActive(true);
            for (int i = 0; i < _systems.Length; i++)
            {
                _systems[i].PerDay();
            }
        }
        Debug.Log("Tick Ended");
        //Tick
        for (int i = 0; i < _systems.Length; i++)
        {
            _systems[i].PerTick();
        }
	}
}
