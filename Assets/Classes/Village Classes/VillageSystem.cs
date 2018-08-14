using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Village
{
    public class VillageSystem : ITime
    {
        public List<VillageComponent> Villages { get; private set; }
        private Dictionary<Supplies, float> _resources;

        public VillageSystem()
        {
            Villages = new List<VillageComponent>();
            _resources = new Dictionary<Supplies, float>();
            foreach (var item in System.Enum.GetValues(typeof(Supplies)).Cast<Supplies>())
            {
                _resources.Add(item, 0);
            }
        }

        public void FirstVillagePos(Vector3Int pos)
        {
            CreateFirstVillage(pos);
        }

        private void CreateFirstVillage(Vector3Int pos)
        {
            var village = new VillageComponent(pos, 5, new VillageEntity(25, 25, 25, 25, "Base"));

            AddVillage(village);
        }

        public void AddVillage(VillageComponent village)
        {
            Villages.Add(village);
        }

        public Dictionary<Supplies, float> GetAllVillageInfo()
        {
            foreach (var item in System.Enum.GetValues(typeof(Supplies)).Cast<Supplies>())
                _resources[item] = 0;

            foreach(var village in Villages)
            {
                var temp = village.GetInfo();
                foreach (Supplies item in System.Enum.GetValues(typeof(Supplies)).Cast<Supplies>())
                    _resources[item] += temp[item];
            }

            return _resources;
        }

        public void ReduceMaterial(KeyValuePair<Supplies, float> material)
        {
            var amount = material.Value / Villages.Count;
            foreach (var village in Villages)
            {
                village.ReduceMaterial(new KeyValuePair<Supplies, float>(material.Key, amount));
            }
        }

        public VillageComponent GetVillage(Vector3Int pos)
        {
            var temp = Villages.FirstOrDefault(x => x.Position == pos);
            if (temp == null)
                return null;
            else
                return temp;
        }

        public void PerTick()
        {
            foreach (VillageComponent village in Villages)
                village.BurnWood();
        }

        public void PerDay()
        {
            foreach (VillageComponent village in Villages)
                village.Consume();
        }

        public void PerNight() { }

        public void PerCycle()
        {
            foreach (VillageComponent village in Villages)
            {
                village.FoodDecay();
                village.MorePeople();
            }
        }
    }
}