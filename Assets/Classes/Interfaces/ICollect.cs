using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICollect  {
    Vector3Int Position { get; set; }
    Dictionary<Supplies, float> Withdraw(Dictionary<Supplies, float> material);
    void Deposit(Dictionary<Supplies, float> material);
}
