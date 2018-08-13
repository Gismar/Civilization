using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Village
{
    public class VillageEntity : IMaterial
    {
        public int Population { get; set; }
        public float Food { get; set; }
        public float Water { get; set; }
        public float Stone { get; set; }
        public float Wood { get; set; }
        public float FoodDecay { get; set; }
        public float SurvivalChance { get; set; }
        public string VillageName { get; set; }
        public float Heat { get; set; }

        public VillageEntity(float food, float water, float stone, float wood, string villageName)
        {
            Food = food;
            Water = water;
            Stone = stone;
            Wood = wood;
            VillageName = villageName;
            Heat = 22f;
            FoodDecay = 0.99f;
        }
    }
}