using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public Image m_HealthBar;

    void Start()
    {
    }

    void Update()
    {
        float health = gameObject.GetComponent<PlayerStats>().GetHealth();

        m_HealthBar.fillAmount = health / 100.0f;
    }
}
