using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour
{
    [System.Serializable]
    public struct WaveInfo
    {
        public PlayerNum playerNum;
        [Range(0, 1)] public float width;//X振幅
        [Range(0, 1)] public float hight;//Y振幅
        [Range(0, 10)] public float AVEx;//角速度
        [Range(0, 10)] public float AVEy;//角速度
        [Range(0, 1)] public float daviation;
    }
    public WaveInfo info = default;
    private Vector3 m_angle = Vector3.zero;
    WaveInfo m_info = default;

    void Awake()
    {
        m_info = info;
    }
    void FixedUpdate()
    {
        Vector3 m_temp = Vector3.zero;
        m_temp.x = m_info.width * m_info.AVEx * Mathf.Sin(m_angle.x += m_info.AVEx * Time.fixedDeltaTime) * Time.fixedDeltaTime;
        m_temp.y = m_info.hight * m_info.AVEy * Mathf.Cos(m_angle.y += m_info.AVEy * Time.fixedDeltaTime) * Time.fixedDeltaTime;

        if (m_info.playerNum == PlayerNum.Player2)
        {
            m_temp *= -1;
        }

        transform.localPosition += m_temp;
        if (m_angle.x > 2 * Mathf.PI) m_angle.x -= 2 * Mathf.PI;
        if (m_angle.y > 2 * Mathf.PI) m_angle.y -= 2 * Mathf.PI;
    }
}
