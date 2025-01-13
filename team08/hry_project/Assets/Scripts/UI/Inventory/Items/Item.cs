using Unity.VisualScripting;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public Sprite m_Sprite;


    protected string m_ItemName;
    protected int m_MaxStack;
    protected bool m_IsConsumable;

    // Abstract method that must be implemented by each specific item type
    public abstract void UseItem ( PlayerStats player );

    // Method that can be optionally overridden by specific items when picked up
    public virtual void OnPickup()
    {
        Debug.Log($"{m_ItemName} has been picked up!");
    }

    public bool EqualTo ( Item item )
    {
        if ( m_ItemName == item.GetItemName() ) {
            return true;
        }

        return false;
    }

    public string GetItemName()
    {
        return m_ItemName;
    }

    public int GetItemStack()
    {
        return m_MaxStack;
    }

    public bool IsConsumable()
    {
        return m_IsConsumable;
    }

    public virtual bool AddAmmo()
    {
        return false;
    }

    public virtual int GetAmmoCount()
    {
        return -1;    
    }

    public object Clone()
    {
        return this.MemberwiseClone();    
    }
}
