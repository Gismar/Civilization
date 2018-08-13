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
        public FinishedTraveling FinishedTravelingDeletage;
        private bool _reachedDestination;
        private Dictionary<Supplies, float> _totalInfo;
        private Dictionary<Supplies, float> _widthrawAmount;
        private Dictionary<Supplies, float> _depositAmount;
        private ICollect _interactingObject;

        public TroopComponent(int travelTime, List<TroopEntity> troops, int id, Dictionary<Supplies, float> wAmount, Dictionary<Supplies,float> dAmount, ICollect interactingObject)
        {
            Troops = troops;
            TravelTime = travelTime;
            RaidID = id;
            _widthrawAmount = wAmount;
            _depositAmount = dAmount;
            _interactingObject = interactingObject;
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
                troop.SurvivalChance *= troop.Heat / 20f;
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
            var deathChance = Random.Range(0f, 1f);
            for (int i = Troops.Count - 1; i > 0; i--)
            {
                if (deathChance > Troops[i].SurvivalChance) Troops.RemoveAt(i);
            }
        }

        public bool Move()
        {
            DistanceFromStart += _reachedDestination ? -10 : 10;
            ElapsedTime += 10;
            if (ElapsedTime == TravelTime)
            {
                _reachedDestination = true;
                _interactingObject.Deposit(_depositAmount);
                _interactingObject.Withdraw(_widthrawAmount);
                _depositAmount = _widthrawAmount;
                _widthrawAmount = new Dictionary<Supplies, float>();
            }
            if (DistanceFromStart == 0 && _reachedDestination)
            {
                return true;
            }
            return false;
        }
    }
}