using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Troop
{
    public class TroopComponent
    {
        public List<TroopEntity> Troops { get; private set; } = new List<TroopEntity>();
        public int TravelTime { get; set; }
        public int ElapsedTime { get; set; }
        public int DistanceFromStart { get; set; }
        public int RaidID { get; set; }
        public Vector3Int Location { get; set; }
        private bool _reachedDestination;
        private Dictionary<Supplies, float> _totalInfo;
        private Dictionary<Supplies, float> _widthrawAmount;
        private Dictionary<Supplies, float> _depositAmount;
        private ICollect _interactingObject;
        private ICollect _startingLocation;

        public TroopComponent(int travelTime, List<TroopEntity> troops, int id, Dictionary<Supplies, float> wAmount, Dictionary<Supplies,float> dAmount, ICollect interactingObject, ICollect village)
        {
            Troops = troops;
            TravelTime = travelTime;
            RaidID = id;
            _widthrawAmount = wAmount;
            _depositAmount = dAmount;
            _interactingObject = interactingObject;
            _startingLocation = village;
        }

        private void CollectInfo()
        {
            _totalInfo = new Dictionary<Supplies, float>();
            foreach (var item in System.Enum.GetValues(typeof(Supplies)).Cast<Supplies>())
                _totalInfo.Add(item, 0);
            foreach (IMaterial troop in Troops)
            {
                _totalInfo[Supplies.Population] += troop.Population;
                _totalInfo[Supplies.Food] += troop.Food;
                _totalInfo[Supplies.Water] += troop.Water;
                _totalInfo[Supplies.Stone] += troop.Stone;
                _totalInfo[Supplies.Wood] += troop.Wood;
            }
        }

        public Dictionary<Supplies, float> GetInfo()
        {
            CollectInfo();
            return _totalInfo;
        }

        public void ConsumeFood(bool isDay)
        {
            foreach (IMaterial troop in Troops)
            {
                if (isDay)
                {
                    troop.Food = troop.Food - 2f > 0 ? troop.Food - 2f : 0;
                    troop.SurvivalChance = troop.Food == 0 ? troop.SurvivalChance - 0.1f : troop.SurvivalChance - 0.01f;
                }
                else
                {
                    troop.Food = troop.Food - 1f > 0 ? troop.Food - 1f : 0;
                    troop.SurvivalChance = troop.Food == 0 ? troop.SurvivalChance - 0.2f : troop.SurvivalChance - 0.02f;
                }
            }
        }

        public void DrinkWater()
        {
            foreach (IMaterial troop in Troops)
            {
                troop.Water -= 2.5f;
            }
        }

        public void DecayFood()
        {
            foreach (IMaterial troop in Troops)
            {
                troop.Food *= troop.FoodDecay;
            }
        }

        public void CheckSurvivors()
        {
            for (int i = Troops.Count - 1; i > 0; i--)
            {
                var deathChance = Random.Range(0f, 1f);
                if (deathChance > Troops[i].SurvivalChance) Troops.RemoveAt(i);
                Debug.Log(Troops.Count);
            }
        } 

        private void Deposit()
        {
            CollectInfo();
            var temp = new Dictionary<Supplies, float>()
            {
                {Supplies.Heat, Mathf.Min(_totalInfo[Supplies.Heat], _depositAmount[Supplies.Heat])},
                {Supplies.Water, Mathf.Min(_totalInfo[Supplies.Water], _depositAmount[Supplies.Water])},
                {Supplies.Wood, Mathf.Min(_totalInfo[Supplies.Wood], _depositAmount[Supplies.Wood])},
                {Supplies.Stone, Mathf.Min(_totalInfo[Supplies.Stone], _depositAmount[Supplies.Stone])},
                {Supplies.Food, Mathf.Min(_totalInfo[Supplies.Food], _depositAmount[Supplies.Food])}
            };
            _depositAmount = temp;
            foreach (var troop in Troops)
            {
                troop.Stone -= _depositAmount[Supplies.Stone];
                troop.Wood -= _depositAmount[Supplies.Wood];
                troop.Water -= _depositAmount[Supplies.Water];
                troop.Food -= _depositAmount[Supplies.Food];
            }
        }

        private void Withdraw()
        {
            foreach (var troop in Troops)
            {
                troop.Stone += _widthrawAmount[Supplies.Stone] / Troops.Count;
                troop.Wood += _widthrawAmount[Supplies.Wood] / Troops.Count;
                troop.Water += _widthrawAmount[Supplies.Water] / Troops.Count;
                troop.Food += _widthrawAmount[Supplies.Food] / Troops.Count;
            }
        }

        public bool Move()
        {
            DistanceFromStart += _reachedDestination ? -10 : 10;
            ElapsedTime += 10;
            if (ElapsedTime == TravelTime)
            {
                _reachedDestination = true;
                Deposit();
                _interactingObject.Deposit(_depositAmount);
                _widthrawAmount = _interactingObject.Withdraw(_widthrawAmount);
                Withdraw();
            }
            if (DistanceFromStart == 0 && _reachedDestination)
            {
                CollectInfo();
                _startingLocation.Deposit(_totalInfo);
                return true;
            }
            return false;
        }
    }
}