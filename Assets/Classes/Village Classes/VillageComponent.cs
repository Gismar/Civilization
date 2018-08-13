using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


namespace Village
{
    public class VillageComponent : ICollect
    {
        private VillageEntity _village;
        private AnimationCurve _populationCurve;
        private Dictionary<Supplies, float> _supplies;
        public Vector3Int Position { get; set; }

        public VillageComponent(Vector3Int pos, int pop, VillageEntity village)
        {
            _village = village;
            Position = pos;
            _village.Population = pop;
        }

        private void CollectInfo()
        {
            _supplies = new Dictionary<Supplies, float>
            {
                {Supplies.Population, _village.Population },
                {Supplies.Food, _village.Food },
                {Supplies.Water, _village.Water },
                {Supplies.Stone, _village.Stone },
                {Supplies.Wood, _village.Wood },
                {Supplies.Heat, _village.Heat },
            };
        
        }

        public void ReduceMaterial(KeyValuePair<Supplies, float> material)
        {
            switch (material.Key)
            {
                case Supplies.Food: _village.Food -= material.Value; break;
                case Supplies.Population: _village.Population -= Mathf.FloorToInt(material.Value); break;
                case Supplies.Heat: _village.Heat -= material.Value; break;
                case Supplies.Water: _village.Water -= material.Value; break;
                case Supplies.Stone: _village.Stone -= material.Value; break;
                case Supplies.Wood: _village.Wood -= material.Value; break;
            }
        }

        public Dictionary<Supplies, float> GetInfo()
        {
            CollectInfo();
            return _supplies;
        }

        public string GetName()
        {
            return _village.VillageName;
        }

        public void BurnWood()
        {
            _village.Wood -= Mathf.CeilToInt(_village.Population / 10f);
            if(_village.Wood <= 0)
            {
                _village.Wood = 0;
                _village.Heat -= 0.5f;
            }
            else
            {
                _village.Heat += 1f;
            }
            _village.Heat = Mathf.Clamp(_village.Heat, 0, 22);
        }

        public void Consume()
        {
            _village.Food -= 2 * _village.Population;
            _village.Water -= 1.5f * _village.Population;
        }

        public void FoodDecay()
        {
            _village.Food *= _village.FoodDecay;
        }

        public Dictionary<Supplies, float> Withdraw(Dictionary<Supplies, float> material)
        {
            CollectInfo();
            var temp = new Dictionary<Supplies, float>();
            foreach (var item in _supplies)
            {
                temp.Add(item.Key, 0);
            }

            foreach (var data in _supplies)
            {
                if (!material.ContainsKey(data.Key)) continue;
                var amount = material[data.Key] > data.Value ? data.Value : material[data.Key];
                temp[data.Key] = amount;
                ModifyVillage(new KeyValuePair<Supplies, float>(data.Key, amount));
            }

            return temp;
        }

        private void ModifyVillage(KeyValuePair<Supplies, float> material)
        {
            switch (material.Key)
            {
                case Supplies.Wood: _village.Wood -= Mathf.FloorToInt(material.Value); break;
                case Supplies.Water: _village.Water -= Mathf.FloorToInt(material.Value); break;
                case Supplies.Food: _village.Food -= Mathf.FloorToInt(material.Value); break;
                case Supplies.Stone: _village.Stone -= Mathf.FloorToInt(material.Value); break;
                case Supplies.Population: _village.Population -= Mathf.FloorToInt(material.Value); break;
                case Supplies.Heat: _village.Heat -= Mathf.FloorToInt(material.Value); break;
            }
        }

        public void Deposit(Dictionary<Supplies, float> material)
        {
            foreach (var data in material)
            {
                ModifyVillage(new KeyValuePair<Supplies, float>(data.Key, -data.Value));
            }
        }
    }
}
