using UnityEngine;

public class SanityPills : Item
{
    float m_SanityRegen;


    private void Awake()
    {
        m_ItemName = "SanityPills";
        m_SanityRegen = 25f;
        m_MaxStack = 1;
        m_IsConsumable = true;
    }

    // Define what happens when this specific item is used
    public override void UseItem(PlayerStats player)
    {
        player.HealSanity(m_SanityRegen);
        Debug.Log($"{m_ItemName} used. Sanity restored by {m_SanityRegen}");
    }
}
