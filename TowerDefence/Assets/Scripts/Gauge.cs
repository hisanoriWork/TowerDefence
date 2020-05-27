using UnityEngine;
using UnityEngine.UI;
public class Gauge : MonoBehaviour
{
    [SerializeField]private Image fillImage = default;
    [SerializeField]private Image backImage = default;
    [SerializeField]private Image gaugeImage = default;
    [SerializeField]private bool fillMethodIsHorizontal = true;
    [SerializeField] private Image.OriginHorizontal origin = default;
    [SerializeField] private Vector2 GaugeSize = default;
    private Vector2 m_pos = Vector2.zero;
    private int m_gaugeMaxValue = 1;
    private int m_maxValue = 1;
    private int m_value = 1;

    void Awake()
    {
        if(fillMethodIsHorizontal)
            fillImage.fillOrigin = (int)origin;
        fillImage.rectTransform.sizeDelta = GaugeSize;
        backImage.rectTransform.sizeDelta = GaugeSize;
        m_pos = gaugeImage.rectTransform.anchoredPosition;
    }
    public int gaugeMaxValue
    {
        get { return m_gaugeMaxValue; }
        set
        {
            m_gaugeMaxValue = value;
            m_maxValue = value;
            if (fillMethodIsHorizontal)
            {
                fillImage.rectTransform.sizeDelta = GaugeSize;
                backImage.rectTransform.sizeDelta = GaugeSize;
            }
            fillImage.fillAmount = (float)m_value / (float)m_maxValue;
        }
    }
    public int maxValue
    {
        get { return m_maxValue; }
        set
        {
            m_maxValue = value;
            if (fillMethodIsHorizontal)
            {
                fillImage.rectTransform.sizeDelta = new Vector2(GaugeSize.x * m_maxValue / m_gaugeMaxValue, GaugeSize.y);
                backImage.rectTransform.sizeDelta = new Vector2(GaugeSize.x * m_maxValue / m_gaugeMaxValue, GaugeSize.y);
                gaugeImage.rectTransform.sizeDelta = new Vector2(GaugeSize.x * m_maxValue / m_gaugeMaxValue, GaugeSize.y) +new Vector2(15,10);
                if (origin == Image.OriginHorizontal.Left)
                {
                    gaugeImage.rectTransform.anchoredPosition = m_pos + Vector2.left * GaugeSize.x * (m_gaugeMaxValue - m_maxValue) / (2 * m_gaugeMaxValue);
                }
                else
                {
                    gaugeImage.rectTransform.anchoredPosition = m_pos + Vector2.right * GaugeSize.x * (m_gaugeMaxValue - m_maxValue) / (2 * m_gaugeMaxValue);
                }
            }
            fillImage.fillAmount = (float)m_value / (float)m_maxValue;
        }
    }
    public int value
    {
        get { return m_value; }
        set
        {
            m_value = value;
            fillImage.fillAmount = (float)m_value / (float)m_maxValue;
        }
    }
}
