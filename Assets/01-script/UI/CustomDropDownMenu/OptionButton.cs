using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public enum OptionButtonType
{
    Yellow = 'Y',
    Green = 'G',
    White = 'W',
    Blue = 'B',
    Orange = 'O',
    Plus = 'P',
    Minus = 'M'
}

[RequireComponent(typeof(Button))]
public class OptionButton : MonoBehaviour
{
    private Text text;

    public OptionButtonType optionType;

    public Action<OptionButton> OnClick;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnButtonClick);
    }

    public void Setup(OptionButtonType optionButtonType)
    {
        this.optionType = optionButtonType;
        text = transform.GetChild(0).GetComponent<Text>();
        SetText();
    }

    public void SetText(int pos = 0)
    {
        string middleFix = "- ";
        text.text = string.Format("  {0} {1}", pos != -1 ? (pos +1) + middleFix : "X " + middleFix, optionType);
    }

    public void OnButtonClick()
    {
        OnClick(this);
    }
}
