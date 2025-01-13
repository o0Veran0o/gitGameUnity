using UnityEngine;

public class MedKit : Item
{
    float m_HealthRegen;

    private void Awake()
    {
        m_ItemName = "Medkit";
        m_HealthRegen = 25f;
        m_MaxStack = 1;
        m_IsConsumable = true;
    }

    // Define what happens when this specific item is used
    public override void UseItem ( PlayerStats player )
    {
        player.Heal ( m_HealthRegen );
        Debug.Log($"{m_ItemName} used. Health restored by {m_HealthRegen}");
    }
}
