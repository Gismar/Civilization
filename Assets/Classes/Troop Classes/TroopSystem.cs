using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Troop
{
    public class TroopSystem : ITime
    {
        public List<TroopComponent> TroopGroup { get; private set; }
        private int count = 0;

        public TroopSystem()
        {
            TroopGroup = new List<TroopComponent>();
            Test();
        }

        private void Test()
        {
            var village = new Village.VillageComponent(Vector3Int.up, 2, new Village.VillageEntity(100, 100, 1001, 100, "YEET"));
            for (int i = 0; i < 20; i++)
            {
                var troopAmount = Random.Range(0, 10f);
                var troop = new List<TroopEntity>();
                for (int j = 0; j < troopAmount; j++)
                    troop.Add(new TroopEntity(Random.Range(0, 100), Random.Range(0, 100), Random.Range(0, 100), Random.Range(0, 100)));
                AddGroup(10, troop, new Dictionary<Supplies, float>()
                {
                    {Supplies.Wood, 10f },
                    {Supplies.Water, 10f }
                }, new Dictionary<Supplies, float>()
                {
                    {Supplies.Stone, 10f },
                    {Supplies.Food, 10f }
                }, village);
            }
        }

        public void AddGroup(TroopComponent group)
        {
            TroopGroup.Add(group);
            count++;
        }

        public void AddGroup(int travelTime, List<TroopEntity> troops, Dictionary<Supplies, float> wAmount, Dictionary<Supplies, float> dAmount, ICollect interactingObject)
        {
            var temp = new TroopComponent(travelTime, troops, count + TroopGroup.Count, wAmount, dAmount, interactingObject);
            TroopGroup.Add(temp);
            count++;
        }

        public void PerTick()
        {
            var temp = new List<TroopComponent>();
            foreach (TroopComponent group in TroopGroup)
            {
                group.CheckSurvivors();
                var isDone = group.Move();
                if (isDone) temp.Add(group);
            }
            TroopGroup = TroopGroup.Except(temp).ToList();
        }

        public void PerDay()
        {
            foreach (TroopComponent group in TroopGroup)
                group.ConsumeFood(true);
        }

        public void PerNight()
        {
            foreach (TroopComponent group in TroopGroup)
                group.ConsumeFood(false);
        }

        public void PerCycle()
        {
            foreach (TroopComponent group in TroopGroup)
            {
                group.DrinkWater();
                group.DecayFood();
            }
        }
    }
}
