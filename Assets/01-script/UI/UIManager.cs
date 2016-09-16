using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIManager : MonoSingleton<UIManager>
{
    [SerializeField]
    private Board board;

    [SerializeField]
    private List<CashCardUI> cashCards;

    [SerializeField]
    private Text resultText;


    public Board Board
    {
        get { return board; }
    }

    public void TakeDecision()
    {
        string boardInfo = board.GetBoard();
        string remainingCards = GetRemainingCashCards();
        string result = GameManager.Instance.CreateBoard(boardInfo, remainingCards);

        resultText.text = result;
    }

    public void ResetCashCards()
    {
        foreach(CashCardUI cc in cashCards)
        {
            cc.ResetCashCard();
        }
    }

    public string GetRemainingCashCards()
    {
        string result = string.Empty;

        foreach(CashCardUI cc in cashCards)
        {
            result += string.Format("{0}{1};", cc.Color, cc.WinForFirst);
        }

        return result;
    }
 
}
