using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMaterial  {
    int Population { get; set; }
    float Food { get; set; }
    float Water { get; set; }
    float Stone { get; set; }
    float Wood { get; set; }
    float FoodDecay { get; set; }
    float SurvivalChance { get; set; }
    float Heat { get; set; }
}
