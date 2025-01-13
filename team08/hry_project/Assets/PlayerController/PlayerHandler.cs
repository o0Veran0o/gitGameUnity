using System;
using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    private Item nearbyItem;
    public InventoryManager inventoryManager;

    private void Start()
    {
        inventoryManager = GetComponent<InventoryManager>();
    }

    private void Update()
    {
        // Check if the player presses the "F" key and an item is nearby
        if ( Input.GetKeyDown ( KeyCode.E ) && nearbyItem != null) {
            PickUpItem();
        }
        if ( Input.GetKeyDown ( KeyCode.G ) ) {
            DropItem();
        }
        if ( Input.GetKeyDown ( KeyCode.R ) ) {
            ReloadShotgun();    
        }
        if ( Input.GetKeyDown ( KeyCode.F ) ) {
            ToggleFlashlight();    
        }
        if ( Input.GetMouseButtonDown ( 0 ) ) {
            UseItem();
        }
    }

    private void PickUpItem()
    {
        if ( inventoryManager.AddItemToInventory(nearbyItem) ) {
            Destroy(nearbyItem.gameObject); // Remove the item from the scene after pickup
            nearbyItem = null; // Reset nearby item
        }
    }

    private void DropItem()
    {
        Item droppedItem = inventoryManager.DropItem();
       
        // I am truly sorry for this, however "== null" doesn't seem to work at all :)
        try {
            droppedItem.GetItemName();
        }
        catch ( NullReferenceException ex ) {
            return;
        }

        GameObject prefab = Resources.Load($"Prefabs/Items/{droppedItem.GetItemName()}") as GameObject;
        GameObject newObject = Instantiate ( prefab, transform.position, Quaternion.identity );

        for ( int i = 0; i < droppedItem.GetAmmoCount(); i ++ ) {
            newObject.transform.GetComponent<Item>().AddAmmo();
        }
    }

    private void ReloadShotgun()
    {
        inventoryManager.ReloadShotgun();    
    }

    private void UseItem()
    {
        inventoryManager.UseItem();    
    }

    private void ToggleFlashlight()
    {
        transform.Find ( "Light 2D" ).gameObject.SetActive ( transform.GetComponent<PlayerStats>().ToggleFlashlight() );    
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the player is near a pickable item
        Item item = collision.GetComponent<Item>();
        if (item != null)
        {
            nearbyItem = item;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Clear the item reference when leaving the item's area
        Item item = collision.GetComponent<Item>();
        if (item != null && item == nearbyItem)
        {
            nearbyItem = null;
        }
    }

    // Example method to simulate healing (called when a healing item is used)
    public void Heal(float amount)
    {
        Debug.Log($"Player healed by {amount} points");
        // Add your health update logic here
    }

    private void OnGUI()
    {
        if (nearbyItem != null)
        {
            // Create a new GUIStyle for the label
            GUIStyle guiStyle = new GUIStyle();
            guiStyle.fontSize = 24; // Set the font size (adjust the size as needed)
            guiStyle.normal.textColor = Color.white; // Set the text color

            // Calculate the screen position of the nearby item
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(nearbyItem.transform.position);

            // Display the label with the new style
            GUI.Label(new Rect(screenPosition.x - 50, Screen.height - screenPosition.y - 20, 300, 50),
                      $"To pick up {nearbyItem.GetItemName()}, press E", guiStyle);
        }
    }
}
