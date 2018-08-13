using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITime {
    void PerTick();
    void PerDay();
    void PerNight();
    void PerCycle();
}
