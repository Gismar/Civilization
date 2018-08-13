using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Collector
{
    public class CollectorSystem : ITime
    {
        private List<CollectorComponent> _collectors;
        private Dictionary<Supplies, float> _resources;

        public CollectorSystem()
        {
            _collectors = new List<CollectorComponent>();
            _resources = new Dictionary<Supplies, float>();
            foreach (var item in System.Enum.GetValues(typeof(Supplies)).Cast<Supplies>())
            {
                _resources.Add(item, 0);
            }
        }

        public void AddCollector(CollectorComponent collector)
        {
            _collectors.Add(collector);
        }

        public CollectorComponent GetCollector (Vector3Int pos)
        {
            var temp = _collectors.FirstOrDefault(x => x.Position == pos);
            return temp;
        }

        public Dictionary<Supplies, float> GetAllCollectorInfo()
        {
            foreach(Supplies item in _resources.Keys)
            {
                _resources[item] = 0;
            }

            foreach (var collector in _collectors)
            {
                var produce = collector.GetProduce();
                var consume = collector.GetConsume();

                _resources[produce.Key] += produce.Value;
                _resources[consume.Key] += consume.Value;
            }
            return _resources;
        }

        public bool FillCollector(Vector3Int pos, int amount)
        {
            var temp = GetCollector(pos);
            if (temp == null)
                return false;
            else
            {
                temp.Refill(amount);
                return true;
            }
        }

        public void PerTick()
        {
            Debug.Log(_collectors.Count);
            foreach (CollectorComponent collector in _collectors)
                collector.Produce();
        }
        
        public void PerDay() { }

        public void PerNight() { }

        public void PerCycle() { }
    }
}
