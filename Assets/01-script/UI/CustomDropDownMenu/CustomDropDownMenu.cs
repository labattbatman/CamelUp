using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class CustomDropDownMenu : MonoBehaviour
{
    [SerializeField]
    private Button mainButton; 

    [SerializeField]
    private GameObject container;

    [SerializeField]
    private GameObject optionButtonPrefab;

    public Action<OptionButton> OnDropDownClick;
    public Action<CaseBoard> OnMainButtonClick;

    private List<OptionButton> optionButtons = new List<OptionButton>();   

    public GameObject Container
    {
        get { return container; }
    }

    public void Setup()
    {
        ShowContainer(false);
        CreateOptionButtons();
    }
    
    public void OnMainButtonClicked()
    {
        CaseBoard cb = transform.parent.GetComponent<CaseBoard>();
        UIManager.Instance.Board.CloseAllOptions(cb);
        ShowContainer(!container.active);

        OnMainButtonClick.Invoke(cb);
    }

    public void ChangeMainButtontext(string newText)
    {
        mainButton.transform.GetChild(0).gameObject.GetComponent<Text>().text = newText;
    }

    private void CreateOptionButtons()
    {
        foreach (OptionButtonType option in Enum.GetValues(typeof(OptionButtonType)))
        {
            GameObject button = Instantiate(optionButtonPrefab);
            button.transform.SetParent(container.transform);
            OptionButton obt = button.AddComponent<OptionButton>();
            optionButtons.Add(obt);
            obt.Setup(option);
            obt.OnClick += OnOptionClick;
        }

        Destroy(optionButtonPrefab);
    }

    public void UpdateDropDown(string newText)
    {
        foreach (var ob in optionButtons)
        {
            var test = newText.ToUpper().IndexOf(ob.optionType.ToString()[0]);
            
            ob.SetText(test);
        }

        ChangeMainButtontext(newText);
    }

    public void ShowContainer(bool show)
    {
        container.SetActive(show);
    }

    private void OnOptionClick(OptionButton ob)
    {
        OnDropDownClick(ob);
    }
}
