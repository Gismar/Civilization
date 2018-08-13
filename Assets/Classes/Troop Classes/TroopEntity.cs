using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Troop
{
    public class TroopEntity : IMaterial
    {
        public int Population { get; set; }
        public float Food { get; set; }
        public float Water { get; set; }
        public float Stone { get; set; }
        public float Wood { get; set; }
        public float FoodDecay { get; set; }
        public float SurvivalChance { get; set; }
        public float Heat { get; set; }

        public TroopEntity(float food, float water, float stone, float wood)
        {
            Population = 1;
            Food = food;
            Water = water;
            Stone = stone;
            Wood = Wood;
            Heat = 22f;
            FoodDecay = 0.95f;
        }
    }
}
