using System.Collections.Generic;

namespace Collector
{
    public class CollectorEntity
    {

        public Queue<KeyValuePair<Supplies, float>> Produce { get; set; }
        public Queue<KeyValuePair<Supplies, float>> Consume { get; set; }

        public CollectorEntity()
        {
            Produce = new Queue<KeyValuePair<Supplies, float>>();
            Consume = new Queue<KeyValuePair<Supplies, float>>();
        }
    }
}
