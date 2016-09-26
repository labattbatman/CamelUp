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

    private int winForFirst = GameRules.WIN_FOR_FIRST[0];

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
        for (int i = 0; i < GameRules.WIN_FOR_FIRST.Length; i++)
        {
            if (winForFirst == GameRules.WIN_FOR_FIRST[i])
            {
                if (i < GameRules.WIN_FOR_FIRST.Length - 1)
                {
                    winForFirst = GameRules.WIN_FOR_FIRST[i + 1];
                }
                else
                {
                    winForFirst = GameRules.WIN_FOR_FIRST[0];
                }

                break;
            }
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
