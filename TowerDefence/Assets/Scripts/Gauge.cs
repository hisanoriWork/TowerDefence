using UnityEngine;
using UnityEngine.UI;
public class Gauge : MonoBehaviour
{
    [SerializeField] private Image fillImage = default;
    private int m_maxValue = 1;
    private int m_value = 1;
    public int maxValue
    {
        get { return m_maxValue; }
        set
        {
            m_maxValue = value;
            fillImage.fillAmount = rate;
        }
    }
    public int value
    {
        get { return m_value; }
        set
        {
            m_value = value;
            fillImage.fillAmount = rate;
        }
    }
    public float rate
    {
        get { return (float)m_value / (float)m_maxValue; }
    }
}
