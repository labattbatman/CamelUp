using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

[RequireComponent(typeof(Button))]
public class OptionButton : MonoBehaviour
{
    private Text text;

    public string optionType;

    public Action<OptionButton> OnClick;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnButtonClick);
    }

    public void Setup(string optionButtonType)
    {
        this.optionType = optionButtonType;
        text = transform.GetChild(0).GetComponent<Text>();
        SetText();
    }

    public void SetText()
    {
        text.text = string.Format("  {0}", optionType);
    }

    public void OnButtonClick()
    {
        OnClick(this);
    }
}
