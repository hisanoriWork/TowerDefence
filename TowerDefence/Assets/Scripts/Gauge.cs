using UnityEngine;
using UnityEngine.UI;
public class Gauge : MonoBehaviour
{
    [SerializeField]private Image image = default;
    private int m_maxValue = 1;
    private int m_value = 1;
    public int maxValue
    {
        get { return m_maxValue; }
        set
        {
            m_maxValue = value;
            image.fillAmount = (float)m_value / (float)m_maxValue;
        }
    }
    public int value
    {
        get { return m_value; }
        set
        {
            m_value = value;
            image.fillAmount = (float)m_value / (float)m_maxValue;
        }
    }
}
