using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : Item
{
    void Awake()
    {
        m_ItemName = "Ammo";
        m_MaxStack = 2;
        m_IsConsumable = false;
    }

    public override void UseItem ( PlayerStats player )
    {
    }
}
