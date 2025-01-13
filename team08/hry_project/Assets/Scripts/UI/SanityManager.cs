using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SanityManager : MonoBehaviour
{
    public Image m_SanityBar;

    void Start()
    {
    }

    void Update()
    {
        float sanity = gameObject.GetComponent<PlayerStats>().GetSanity();

        m_SanityBar.fillAmount = sanity / 100.0f;
    }
}

