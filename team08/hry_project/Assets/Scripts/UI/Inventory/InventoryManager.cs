using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;  // Import TextMeshPro namespace

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private GameObject m_InventoryUI;
    private List<GameObject> m_InventorySlots;
    private PlayerStats m_Player;
    private int m_Current; // currently highlighted hotbar slot
    [SerializeField] private Transform weaponHoldPoint; // Transform for the player's weapon holding position

    private void Start()
    {
        m_Player = gameObject.GetComponent<PlayerStats>();
        m_Current = 0;

        m_InventorySlots = new List<GameObject>();

        Transform hotbar = transform.Find ( "Hotbar" );
        
        foreach ( Transform child in hotbar ) {
            m_InventorySlots.Add( child.gameObject );
            child.GetComponent<InventorySlot>().Init();
        }

        m_InventorySlots[m_Current].GetComponent<InventorySlot>().Select(weaponHoldPoint);
    }


    public void LoadInventoryItems(List<string> items)
    {
        ClearInventory();

        foreach (string itemEntry in items)
        {
            // Split item name and quantity
            string[] itemParts = itemEntry.Split(' ');
            if (itemParts.Length < 2)
            {
                Debug.LogWarning("Invalid item entry: " + itemEntry);
                continue;
            }
            string itemName;
            if (itemParts.Length<=2) 
                itemName = string.Join(" ", itemParts, 0, itemParts.Length - 1); // Join parts for multi-word item names
            else
                 itemName = string.Join(" ", itemParts, 0, itemParts.Length - 2); // Join parts for multi-word item names
            if (!int.TryParse(itemParts[^1], out int quantity))
            {
                Debug.LogWarning("Invalid quantity for item: " + itemEntry);
                continue;
            }

            Item newItem = ItemDatabase.GetItemByName(itemName); // Retrieve the item from the database
            if (newItem == null)
            {
                Debug.LogWarning("Item not found in database: " + itemName);
                continue;
            }

            if (itemName == "Shotgun" && itemParts.Length == 3)
            {
                if (int.TryParse(itemParts[^1], out int ammoCount))
                {
                    Shotgun shotgun = newItem as Shotgun;
                    if (shotgun != null)
                    {
                        shotgun.m_AmmoCount = ammoCount;
                    }
                }
                AddItemToInventory(newItem);
            }
            else
            {
                for (int i = 0; i < quantity; i++)
                {
                    AddItemToInventory(newItem); // Add the item `quantity` times
                }
            }
        }
    }

    public List<string> GetInventoryItems()
    {
        List<string> inventoryData = new List<string>();

        for (int i = 0; i < m_InventorySlots.Count; i++)
        {

            InventorySlot inventorySlot = m_InventorySlots[i].GetComponent<InventorySlot>();
            string itemName = inventorySlot.GetItemName();
            int quantity = inventorySlot.Get_m_Count();

            if (!string.IsNullOrEmpty(itemName))
            {
                if (itemName == "Shotgun") // Special case for the Shotgun
                {
                    
                       inventoryData.Add($"{itemName} {quantity} {inventorySlot.GetAmmo()}");
                }
                else
                {
                    inventoryData.Add($"{itemName} {quantity}");
                }
            }
        }

        return inventoryData;
    }



    void Update()
    {
        // Number key scrolling (1-5 for 5 slots)
        for (int i = 1; i <= m_InventorySlots.Count; i++)
        {
            if (Input.GetKeyDown(i.ToString()))
            {
                SwitchToSlot(i - 1); // Adjust index to be 0-based
                break; // Exit loop after a key is pressed
            }
        }

        // ... (rest of the Update method, if any)
    }

    private void SwitchToSlot(int newSlotIndex)
    {
        if (newSlotIndex >= 0 && newSlotIndex < m_InventorySlots.Count)
        {
            m_InventorySlots[m_Current].GetComponent<InventorySlot>().Unselect();
            m_Current = newSlotIndex;
            m_InventorySlots[m_Current].GetComponent<InventorySlot>().Select(weaponHoldPoint);
        }
    }

    public void ReloadShotgun()
    {
        InventorySlot CurrentSlot = m_InventorySlots[m_Current].GetComponent<InventorySlot>();

        if ( CurrentSlot.GetItemName() == "Shotgun" ) { // check if player is holding a shotgun
            for ( int i = 0; i < m_InventorySlots.Count; i ++ ) { // look for ammo in the inventory
                InventorySlot other = m_InventorySlots[i].GetComponent<InventorySlot>();

                if ( other.GetItemName() == "Ammo" ) { // found ammo
                    if ( CurrentSlot.AddAmmo () ) { // ammo was added to the shotgun
                        other.DropItem(); // remove 1 Ammo from the inventory

                        return;
                    }
                }    
            }    
        }  
    }

    public bool AddItemToInventory(Item newItem)
    {
        // Try to stack the item in an existing slot
        for (int i = 0; i < m_InventorySlots.Count; i++)
        {
            var inventorySlot = m_InventorySlots[i].GetComponent<InventorySlot>();
            if (inventorySlot.TryMatchItem(newItem))
            {
                Debug.Log($"Item '{newItem.GetItemName()}' stacked in slot {i}.");
                return true;
            }
            else
            {
                Debug.Log($"Failed to stack item '{newItem.GetItemName()}' in slot {i}. Reason: '{inventorySlot.Empty()}' 'Slot is empty.Item does not match.");
            }
        }

        // Try to place the item in an empty slot
        for (int i = 0; i < m_InventorySlots.Count; i++)
        {
         
       
            if (m_InventorySlots[i].GetComponent<InventorySlot>().TryPutItem(newItem, weaponHoldPoint))
            {
                Debug.Log($"Item '{newItem.GetItemName()}' added to slot {i}.");
                return true;
            }
            else
            {
                Debug.Log($"Failed to add item '{newItem.GetItemName()}' to slot {i}. Reason: Slot is occupied or invalid.");
            }
        }

        Debug.LogWarning($"Inventory full! Could not add item '{newItem.GetItemName()}'.");
        return false;
    }

    public Item DropItem()
    {
        return m_InventorySlots[m_Current].GetComponent<InventorySlot>().DropItem();
    }
    public void ClearInventory()
    {
        foreach (var slot in m_InventorySlots)
        {
            InventorySlot inventorySlot = slot.GetComponent<InventorySlot>();
            inventorySlot.ClearSlot(); // Fully reset the slot
        }

        Debug.Log("Inventory cleared.");
    }

    public void UseItem()
    {
        m_InventorySlots[m_Current].GetComponent<InventorySlot>().UseItem ( m_Player );
    }
}
