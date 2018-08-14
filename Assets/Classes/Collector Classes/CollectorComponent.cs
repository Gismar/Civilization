using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Collector
{
    public class CollectorComponent : ICollect
    {
        private CollectorEntity _collector;
        public Vector3Int Position { get; set; }
        private Supplies _produceType;
        private Supplies _consumeType;

        public CollectorComponent(Supplies produceType, Supplies consumeType, Vector3Int pos, CollectorEntity collect = default(CollectorEntity))
        {
            Position = pos;
            _produceType = produceType;
            _consumeType = consumeType;
            _collector = collect ?? NewCollector();
        }

        private CollectorEntity NewCollector()
        {
            _collector = new CollectorEntity();
            Refill(30);
            return _collector;
        }

        public void Produce()
        {
            if (_collector.Consume.Count == 0) return;
            var r = UnityEngine.Random.Range(5, 15);
            for (int i = 0; i < r; i++){
                _collector.Produce.Enqueue(new KeyValuePair<Supplies, float>(_produceType, 1));
            }
            _collector.Consume.Dequeue();
        }

        public void Refill(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                _collector.Consume.Enqueue(new KeyValuePair<Supplies, float>(_consumeType, 1));
            }
        }

        #region GetStuff
        public KeyValuePair<Supplies, float> GetProduce()
        {
            float temp = 0f;
            foreach (var data in _collector.Produce.ToList())
            {
                Debug.Log(data.Value);
                temp += data.Value;
            }
            return new KeyValuePair<Supplies, float>(_produceType, temp);
        }

        public KeyValuePair<Supplies, float> GetConsume()
        {
            float temp = 0f;
            foreach (var data in _collector.Consume.ToList())
            {
                temp += data.Value;
            }
            return new KeyValuePair<Supplies, float>(_consumeType, temp);
        }
        #endregion

        #region ICollect Functions
        public Dictionary<Supplies, float> Withdraw(Dictionary<Supplies, float> material)
        {
            var temp = new Dictionary<Supplies, float>(){
                {Supplies.Population, 0},
                {Supplies.Heat,0},
                {Supplies.Water, 0},
                {Supplies.Wood, 0},
                {Supplies.Stone, 0},
                {Supplies.Food, 0}
            };
            if (!material.ContainsKey(_produceType)) return temp;

            var count = _collector.Produce.Count < material[_produceType] ? _collector.Produce.Count : Mathf.FloorToInt(material[_produceType]);
            Debug.Log($"Amount of times withdrawn: {count}");
            for (int i = 0; i < count; i++)
            {
                var data = _collector.Produce.Dequeue();
                temp[data.Key] += data.Value;
            }

            return temp;
        }

        public void Deposit(Dictionary<Supplies, float> material)
        {
            if (!material.ContainsKey(_consumeType)) return;

            var amount = Mathf.FloorToInt(material[_consumeType]);
            Refill(amount);
        }
        #endregion
    }
}