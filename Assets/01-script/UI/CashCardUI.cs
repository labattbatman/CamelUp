using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CashCardUI : MonoBehaviour 
{
    [SerializeField]
    private Text textButton;

    [SerializeField]
    private char color;

    public string Color
    {
        get { return color.ToString(); }
    }

    private int winForFirst = 5;

    public string WinForFirst
    {
        get { return winForFirst.ToString(); }
    }

    private void Start()
    {
        UpdateText();
    }

    public void OnClick()
    {
        switch(winForFirst)
        {
            case 5:
                winForFirst = 3;
                break;
            case 3:
                winForFirst = 2;
                break;
            case 2:
                winForFirst = 5;
                break;
        }

        UpdateText();
    }
	
    private void UpdateText()
    {
        textButton.text = winForFirst.ToString();
    }

    public void ResetCashCard()
    {
        winForFirst = 5;
        UpdateText();
    }
}
