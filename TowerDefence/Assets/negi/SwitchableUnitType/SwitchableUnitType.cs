using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SwitchableUnitType : MonoBehaviour
{
    [SerializeField] private List<Button> m_buttons = default;
    private ColorBlock m_defaultColors;

    private void Start()
    {
        m_defaultColors = m_buttons[0].colors;
        ChangeButtonColor(m_buttons[0]);
    }



    public void ChangeButtonColor(Button selectedButton)
    {
        var color = m_defaultColors;
        color.normalColor = m_defaultColors.selectedColor;
        color.highlightedColor = m_defaultColors.selectedColor;
        color.selectedColor = m_defaultColors.selectedColor;
        ResetButtonColor();
        selectedButton.colors = color;

        return;
    }

    private void ResetButtonColor()
    {
        foreach(Button button in m_buttons)
        {
            button.colors = m_defaultColors;
        }
    }

}