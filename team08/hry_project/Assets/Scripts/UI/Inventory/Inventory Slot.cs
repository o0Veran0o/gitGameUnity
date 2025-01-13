using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class InventorySlot : MonoBehaviour
{
    private Item m_Item;
    private int m_Count;

    private Image m_Border;
    private Image m_Fill;
    private Image m_Icon;
    private Image m_Number;
    public AudioSource audioSource; // Add an AudioSource component
    static List<Sprite> m_NumberSprites;
    public AudioClip shotgun;
    public AudioClip medic;
    public AudioClip axe;
    private GameObject m_IconObject;
    private GameObject m_NumberObject;
    private GameObject instantiatedObject; // Holds the instantiated in-scene when selected in inventory.

    void Start()
    {
    }
    public void ClearSlot()
    {
        m_Item = null;
        m_Count = 0;
        m_Icon.sprite = null;
        m_IconObject.SetActive(false);
        m_NumberObject.SetActive(false);

        if (instantiatedObject != null)
        {
            Destroy(instantiatedObject);
            instantiatedObject = null;
        }

        Debug.Log("Slot cleared.");
    }


    void Update()
    {
        if ( m_IconObject.activeSelf ) {
            if ( m_Item.GetItemStack() != 1 ) {
                m_NumberObject.SetActive ( true );

                m_Number.sprite = m_NumberSprites[m_Count];
            }
            else if ( m_Item.GetItemName() == "Shotgun" ) {
                m_NumberObject.SetActive ( true );
                
                m_Number.sprite = m_NumberSprites[m_Item.GetAmmoCount()];
            }
        }
    }

    public void Init()
    {
        m_Item = null;
        m_Count = 0;

        m_Border = transform.Find ( "Border" ).gameObject.transform.GetComponent<Image>();
        m_Fill = transform.Find ( "Fill" ).gameObject.transform.GetComponent<Image>();

        m_NumberObject = transform.Find ( "Number" ).gameObject;
        m_Number = m_NumberObject.transform.GetComponent<Image>();

        m_IconObject = transform.Find ( "Icon" ).gameObject;
        m_Icon = m_IconObject.transform.GetComponent<Image>();

        m_NumberSprites = new List<Sprite>();

        for ( int i = 0; i <= 5; i ++ ) {
            string path = "Models/UI/Numbers/";
            path = path + i.ToString();

            m_NumberSprites.Add ( Resources.Load<Sprite> ( path ) );
        }
    }

    public bool Empty()
    {
        if ( m_Item == null ) {
            return true;    
        }     

        return false;
    }

    public bool TryMatchItem ( Item item )
    {
        if ( m_IconObject.activeSelf && m_Item.EqualTo ( item ) ) {
            if ( m_Count < m_Item.GetItemStack() ) {
                m_Count ++;

                return true;
            }
        }

        return false;
    }

    public bool TryPutItem(Item item, Transform firepoint)
    {
        Debug.Log($"Attempting to add item: {item.GetItemName()} to slot. IconObject active: {m_IconObject.activeSelf}");

        if (!m_IconObject.activeSelf)
        {
            m_Item = item;
            m_Count++;

            m_Icon.sprite = item.m_Sprite;
            m_IconObject.SetActive(true);

            Debug.Log($"Item {m_Item.GetItemName()} added to inventory slot.");

            // Instantiate item prefab if selected
            if (m_Border.color == new Color(154.0f / 256.0f, 154.0f / 256.0f, 154.0f / 256.0f))
            {
                Debug.Log($"Equipping item {m_Item?.GetItemName() ?? "null"} to player's hand.");
                if (m_Item?.GetItemName() != "")
                {
                    GameObject prefab = Resources.Load<GameObject>($"Prefabs/Items/{m_Item.GetItemName()}");
                    if (prefab != null)
                    {
                        if (instantiatedObject != null)
                        {
                            Destroy(instantiatedObject);
                            instantiatedObject = null;
                        }
                        instantiatedObject = Instantiate(prefab, firepoint.position, firepoint.rotation);
                        instantiatedObject.transform.SetParent(firepoint);
                        instantiatedObject.transform.localPosition = Vector3.zero;
                        instantiatedObject.transform.localRotation = Quaternion.identity;
                    }
                    else
                    {
                        Debug.LogWarning($"Prefab for item {m_Item.GetItemName()} not found.");
                    }
                }
            }

            return true;
        }

        Debug.Log("Slot is not empty, cannot add item.");
        return false;
    }


    public Item DropItem()
    {
        Item result = null;

        if ( m_IconObject.activeSelf ) { 
            result = ( Item ) m_Item.Clone();
            m_Count --;

            if ( m_Count == 0 ) { 
                m_Icon.sprite = null;
                m_IconObject.SetActive ( false );
                m_NumberObject.SetActive ( false );
                m_Item = null;
                Destroy(instantiatedObject);
                instantiatedObject = null;
            }
        } 

        return result;
    }

    public void UseItem ( PlayerStats player )
    {
        if ( m_IconObject.activeSelf ) {
         
            m_Item.UseItem ( player );
            PlayItemUseSound(m_Item.GetItemName());

            if ( m_Item.IsConsumable() ) {
                DropItem();    
            }
        }
    }

    private void PlayItemUseSound(string itemName)
    {
        AudioClip clipToPlay = null;

        switch (itemName)  // Use a switch for efficient name checking
        {
            case "Shotgun":
                clipToPlay = shotgun;
                break;
            case "Medkit":
                clipToPlay = medic;
                break;
            case "Axe":
                clipToPlay = axe;
                break;
            // Add more cases for other items
            default:
                Debug.LogWarning($"No sound defined for item: {itemName}");
                break;
        }

        if (clipToPlay != null)
        {
            audioSource.PlayOneShot(clipToPlay);
        }
    }

    public void Select(Transform weaponHoldPoint)
    {
        m_Border.color = new Color ( 154.0f/256.0f, 154.0f/256.0f, 154.0f/256.0f );
        m_Fill.color   = new Color ( 256.0f/256.0f, 256.0f/256.0f, 256.0f/256.0f );
        m_Icon.color   = new Color ( 256.0f/256.0f, 256.0f/256.0f, 256.0f/256.0f );
        
        // Weird, but checking m_Item != null instead does not work. This approach works fine.
        if (m_Item?.GetItemName() != "" ) {
            GameObject prefab = Resources.Load<GameObject>($"Prefabs/Items/{m_Item.GetItemName()}");
            if (prefab != null) {
                // Destroy previously instantiated object
                if (instantiatedObject != null) {
                    Destroy(instantiatedObject);
                    instantiatedObject = null;
                }

                // Instantiate the item prefab
                instantiatedObject = Instantiate(prefab, weaponHoldPoint.position, weaponHoldPoint.rotation);
                instantiatedObject.transform.SetParent(weaponHoldPoint); // Parent to holding point
                instantiatedObject.transform.localPosition = Vector3.zero; // Reset position relative to parent
                instantiatedObject.transform.localRotation = Quaternion.identity; // Reset rotation
            } else {
                Debug.LogWarning($"Prefab for item {m_Item.GetItemName()} not found.");
            }
        }
    }

    public void Unselect()
    {
        m_Border.color = new Color ( 94.0f/256.0f,  94.0f/256.0f,  94.0f/256.0f  );
        m_Fill.color   = new Color ( 154.0f/256.0f, 154.0f/256.0f, 154.0f/256.0f );
        m_Icon.color   = new Color ( 154.0f/256.0f, 154.0f/256.0f, 154.0f/256.0f );

                // Destroy the instantiated in-scene object
        if (instantiatedObject != null)
        {
            Destroy(instantiatedObject);
            instantiatedObject = null;
        }
    }

    public string GetItemName()
    {
        if ( m_IconObject.activeSelf ) {
            return m_Item.GetItemName();
        }
        
        return "";
    }
    public T GetItemInstance<T>() where T : Item
    {
        if (m_Item is T itemInstance)
        {
            if(itemInstance!=null)
                return itemInstance;
        }
        Debug.LogWarning($"Failed to cast item '{m_Item?.GetItemName() ?? "None"}' to type {typeof(T).Name}");
        return null;
    }

    public Item GetItem()
    {
        if (m_Item == null)
        {
            Debug.LogWarning("GetItem called, but m_Item is null!");
        }
        else
        {
            Debug.Log($"GetItem called. Found item: {m_Item.GetItemName()}");
        }
        return m_Item;
    }



    public int Get_m_Count() 
    { 
    return m_Count;
    }

    public bool AddAmmo()
    {
       return m_Item.AddAmmo();
    }
    public int GetAmmo()
    {
        return m_Item.GetAmmoCount();
    }
}