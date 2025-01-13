using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public float health;
    public float sanity;
   // public float torchFuel;

    public Vector3 position;

    public List<string> inventoryItems; // Store item names in the inventory
}
