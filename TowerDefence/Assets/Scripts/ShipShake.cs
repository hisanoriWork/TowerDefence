using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipShake : MonoBehaviour
{
    [System.Serializable]
    public struct WaveInfo
    {
        public PlayerNum playerNum;
        [Range(0, 10)] public float AVE;//角速度
        [Range(0, 180)] public float swingWidth;
    }
    [SerializeField] private WaveInfo m_info = default;
    private float m_angle = 0f;
    void FixedUpdate()
    {
        float m_temp;
        m_temp = m_info.swingWidth * m_info.AVE * Mathf.Cos(m_angle += m_info.AVE * Time.fixedDeltaTime) * Time.fixedDeltaTime;

        if (m_info.playerNum == PlayerNum.Player2)
        {
            m_temp *= -1;
        }
        transform.localEulerAngles += Vector3.forward * m_temp;
        if (m_angle > 2 * Mathf.PI) m_angle -= 2 * Mathf.PI;
    }
}
