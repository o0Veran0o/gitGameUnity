using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Item
{
    public GameObject m_BulletPrefab;
    public int m_BulletsPerShoot = 5;
    public int m_ShootSpread = 10;
    public int m_BulletDefaultSpeed = 10;
    // TODO ammo_cnt is not added after picking up ammo.
    public int m_AmmoCount = 0;
    int m_MaxAmmo = 2;
    public GameObject fireplace ;

    void Awake()
    {
        m_ItemName = "Shotgun";
        m_MaxAmmo = 2;
        m_MaxStack = 1;
        m_IsConsumable = false;
    }

    public override void UseItem(PlayerStats player)
    {
        Debug.Log($"USE SHOTGUN CALL. Remaining Ammo: {m_AmmoCount}");

        if (m_AmmoCount > 0)
        {
            for (int i = 0; i < m_BulletsPerShoot; i++)
            {
                float spreadAngle = Random.Range(-m_ShootSpread, m_ShootSpread);
                Quaternion bulletRotation = Quaternion.Euler(0, 0, spreadAngle);
                GameObject bullet = Instantiate(m_BulletPrefab, fireplace.transform.position, fireplace.transform.rotation * bulletRotation);

                Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();

        
                if (bulletRb != null)
                {
                    bulletRb.velocity = bullet.transform.up * m_BulletDefaultSpeed;
                }
            }

            m_AmmoCount--;
            Debug.Log($"Shotgun fired. Remaining Ammo: {m_AmmoCount}");
        }
        else
        {
            Debug.Log("No ammo left.");
        }
    }

    public override bool AddAmmo()
    {
        if ( m_AmmoCount < m_MaxAmmo ) {
            m_AmmoCount ++;

            return true;
        }

        return false;
    }

    public override int GetAmmoCount()
    {
        return m_AmmoCount;
    }

    public void ActivateForUse(Transform parent)
    {
        gameObject.SetActive(true);
        transform.SetParent(parent);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }



}
