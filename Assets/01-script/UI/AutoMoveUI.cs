using System;
using UnityEngine;
using System.Collections;

enum AutoMoveCamel
{
    Yellow = 0,
    Green,
    White,
    Blue,
    Orange,
}

enum AutoMoveDices
{
    One,
    Two,
    Three,
}

public class AutoMoveUI : MonoBehaviour
{
    [SerializeField]
    private CustomDropDownMenu dicesNumber;

    [SerializeField]
    private CustomDropDownMenu camels;

    public Action<string,int> onButtonClick;

    private int currentDice;
    private string currentCamel = String.Empty;

    public void Setup()
    {
        dicesNumber.OnDropDownClick += OnDropdownChangeDices;
        dicesNumber.Setup(typeof(AutoMoveDices));

        camels.OnDropDownClick += OnDropdownChangeCamles;
        camels.Setup(typeof(AutoMoveCamel));
    }

    private void OnDropdownChangeDices(OptionButton ob)
    {
        switch (ob.optionType)
        {
            case "One":
                currentDice = 1;
                break;
            case "Two":
                currentDice = 2;
                break;
            case "Three":
                currentDice = 3;
                break;
            default:
                UnityEngine.Debug.LogError("Something wrong with AutoMoveUI dice dropdown");
                break;
        }
        dicesNumber.UpdateDropDown(ob.optionType);
        dicesNumber.ShowContainer(false);
    }

    private void OnDropdownChangeCamles(OptionButton ob)
    {
        currentCamel = ob.optionType;

        camels.UpdateDropDown(ob.optionType);
        camels.ShowContainer(false);
    }

    public void OnButtonClick()
    {
        if (currentDice == 0 || String.IsNullOrEmpty(currentCamel))
        {
            UnityEngine.Debug.LogError("No no no, choose things");
            return;
        }

        if (onButtonClick != null)
        {
            onButtonClick.Invoke(currentCamel,currentDice);
        }
    }
}
