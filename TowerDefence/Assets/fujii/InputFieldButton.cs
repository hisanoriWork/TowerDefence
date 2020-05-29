using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InputFieldButton : Button
{
    /*****private field*****/
    private InputField inputField
    {
        get
        {
            if (inputField_ == null)
            {
                inputField_ = GetComponentInParent<InputField>();
            }
            return inputField_;
        }
    }
    private InputField inputField_;
    private TouchScreenKeyboard keyboard_ = null;
    private string originalText_ = "";
    private bool wasCanceled_ = false;

    /*****Monobehaviour method*****/
    void LateUpdate()
    {
        if (keyboard_ == null) return;

        inputField.text = keyboard_.text;

        if (keyboard_.wasCanceled)
        {
            wasCanceled_ = true;
            DestroyKeyboard();
        }

        if (keyboard_.done)
        {
            DestroyKeyboard();
        }
    }
    /*****Button method*****/
    public override void OnPointerDown(UnityEngine.EventSystems.PointerEventData eventData)
    {
        CreateKeyboard();
        base.OnPointerDown(eventData);
    }

    public override void OnPointerClick(UnityEngine.EventSystems.PointerEventData eventData)
    {
        CreateKeyboard();
        base.OnPointerClick(eventData);
    }

    /*****private method*****/
    void CreateKeyboard()
    {
        if (keyboard_ != null) return;
        if (!IsInteractable()) return;

        if (!inputField.textComponent || inputField.textComponent.font == null)
            return;

        originalText_ = inputField.text;
        keyboard_ = TouchScreenKeyboard.Open(inputField.text, TouchScreenKeyboardType.Default, true);
    }

    void DestroyKeyboard()
    {
        if (!inputField.textComponent || !inputField.IsInteractable())
            return;

        if (wasCanceled_)
        {
            inputField.text = originalText_;
            wasCanceled_ = false;
        }

        if (keyboard_ != null)
        {
            keyboard_.active = false;
            keyboard_ = null;
            originalText_ = "";
        }
    }
}