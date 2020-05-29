using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForegroundScript : MonoBehaviour
{
    public static ForegroundScript instance
    {
        get
        {
            if (m_instance != null)
                return m_instance;
            m_instance = FindObjectOfType<ForegroundScript>();
            if (m_instance != null)
                return m_instance;
            return null;
        }
    }
    protected static ForegroundScript m_instance;

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
