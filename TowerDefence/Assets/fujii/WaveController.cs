using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveController : MonoBehaviour
{
    [System.Serializable]
    public struct WaveInfo
    {
        [Range(0, 1)] public float width;//X振幅
        [Range(0, 1)] public float hight;//Y振幅
        [Range(0, 100)] public float swingWidth;//振れ幅
        [Range(0, 10)] public float AVE;//角速度
        [Range(0, 1)] public float deviation;
    }
    [SerializeField] private WaveInfo[] m_info = default;
    private float[] m_angleX;
    private float[] m_angleY;
    private float[] m_AVE;
    private Vector3 m_temp = Vector3.zero;
    private Vector3 m_origin;
    private float[] m_angleZ;
    private float[] m_swingAVE;
    private Vector3 m_tempAngle = Vector3.zero;
    private Vector3 m_originAngle;
    void Awake()
    {
        m_angleX = new float[m_info.Length];
        m_angleY = new float[m_info.Length];
        m_angleZ = new float[m_info.Length];
        m_AVE = new float[m_info.Length];
        m_origin = transform.position;
        m_originAngle = transform.localEulerAngles;
        
    }
    void FixedUpdate()
    {
        for (int i= 0;i< m_info.Length; i++)
        {
            m_AVE[i] = UnityEngine.Random.Range(m_info[i].AVE * (1-m_info[i].deviation), m_info[i].AVE * (1 + m_info[i].deviation));
            m_temp = m_origin;
            m_tempAngle = m_originAngle;

            m_temp.x += m_info[i].width * Mathf.Sin(m_angleX[i] += m_info[i].AVE * Time.fixedDeltaTime);
            m_temp.y += m_info[i].hight * Mathf.Cos(m_angleY[i] += m_info[i].AVE * Time.fixedDeltaTime);
            m_tempAngle.z += m_info[i].swingWidth * Mathf.Sin(m_angleZ[i] += m_info[i].AVE * Time.fixedDeltaTime);

            transform.position = m_temp;
            transform.localEulerAngles = m_tempAngle;
            if (m_angleX[i] > 2 * Mathf.PI) m_angleX[i] -= 2 * Mathf.PI;
            if (m_angleY[i] > 2 * Mathf.PI) m_angleY[i] -= 2 * Mathf.PI;
            if (m_angleZ[i] > 2 * Mathf.PI) m_angleZ[i] -= 2 * Mathf.PI;
        }
    }
}
