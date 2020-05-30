using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgrounsScript : MonoBehaviour
{
    public static BackgrounsScript instance
    {
        get
        {
            if (m_instance != null)
                return m_instance;
            m_instance = FindObjectOfType<BackgrounsScript>();
            if (m_instance != null)
                return m_instance;
            return null;
        }
    }
    protected static BackgrounsScript m_instance;

    void Awake()
    {
        if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }
}
