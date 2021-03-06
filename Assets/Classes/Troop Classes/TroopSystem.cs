﻿using System.Collections;
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
        }

        public void AddGroup(TroopComponent group)
        {
            TroopGroup.Add(group);
            count = count == int.MaxValue ? 0 : count + 1;
        }

        public void AddGroup(int travelTime, List<TroopEntity> troops, Dictionary<Supplies, float> wAmount, Dictionary<Supplies, float> dAmount, ICollect interactingObject, ICollect village)
        {
            var temp = new TroopComponent(travelTime, troops, count, wAmount, dAmount, interactingObject, village);
            TroopGroup.Add(temp);
            count = count == int.MaxValue ? 0 : count + 1;
        }

        public void PerTick()
        {
            var temp = new List<TroopComponent>();
            foreach (TroopComponent group in TroopGroup)
            {
                group.CheckSurvivors();
                if (group == null) { temp.Add(group); return; }
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
