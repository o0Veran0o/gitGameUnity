using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public static class ItemDatabase
{
    private static Dictionary<string, GameObject> itemPrefabDictionary = new Dictionary<string, GameObject>();

    /// <summary>
    /// Initialize the database with a list of item prefabs.
    /// </summary>
    /// <param name="itemPrefabs">List of item prefabs to add to the database.</param>
    public static void Initialize(List<GameObject> itemPrefabs)
    {
        itemPrefabDictionary.Clear();

        foreach (var prefab in itemPrefabs)
        {

            var item = prefab.GetComponent<Item>();

            if (item != null && !itemPrefabDictionary.ContainsKey(item.GetItemName()))
            {
                itemPrefabDictionary[item.GetItemName()] = prefab;
                Debug.Log($"Item '{item.GetItemName()}' added to the database.");
            }
            else if (item == null)
            {
                Debug.LogWarning("Invalid item prefab without Item component.");
            }
            else
            {
                Debug.LogWarning($"Duplicate item name found: {item.GetItemName()}");
            }
        }
    }
    /// <summary>
    /// Get an item instance by its name.
    /// </summary>
    /// <param name="itemName">Name of the item to retrieve.</param>
    /// <returns>A new instance of the item if found, otherwise null.</returns>
    public static Item GetItemByName(string itemName)
    {
        if (itemPrefabDictionary.TryGetValue(itemName, out GameObject prefab))
        {
            GameObject instance = Object.Instantiate(prefab);
            return instance.GetComponent<Item>();
        }

        Debug.LogWarning($"Item '{itemName}' not found in the database.");
        return null;
    }

    /// <summary>
    /// Check if an item exists in the database.
    /// </summary>
    /// <param name="itemName">Name of the item to check.</param>
    /// <returns>True if the item exists, false otherwise.</returns>
    public static bool HasItem(string itemName)
    {
        return itemPrefabDictionary.ContainsKey(itemName);
    }

    /// <summary>
    /// List all items in the database (for debugging).
    /// </summary>
    public static void ListAllItems()
    {
        foreach (var itemName in itemPrefabDictionary.Keys)
        {
            Debug.Log($"Item in database: {itemName}");
        }
    }
}
