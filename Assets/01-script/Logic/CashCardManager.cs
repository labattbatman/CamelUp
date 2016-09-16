using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


public struct CashCard
{
    public string camel;
    public int winForFirst;
    public int winForSecond;
    public int lostForOthers;
    public float equity { get; set; }


    public CashCard(string camelName, int winForFirst, float equity = -10)
    {
        this.camel = camelName;
        this.winForFirst = winForFirst;
        this.winForSecond = 1;
        this.lostForOthers = -1;
        this.equity = equity;
    }

    public CashCard(char camelName, int winForFirst, float equity = -10)
    {
        string name = string.Empty;

        switch (camelName)
        {
            case 'W': name = "White"; break;
            case 'w': name = "White"; break;
            case 'Y': name = "Yellow"; break;
            case 'y': name = "Yellow"; break;
            case 'O': name = "Orange"; break;
            case 'o': name = "Orange"; break;
            case 'B': name = "Blue"; break;
            case 'b': name = "Blue"; break;
            case 'G': name = "Green"; break;
            case 'g': name = "Green"; break;

        }
        this.camel = name;

        this.winForFirst = winForFirst;
        this.winForSecond = 1;
        this.lostForOthers = -1;
        this.equity = equity;
    }

    public string Info()
    {
        return camel + " " + winForFirst + " " + equity;
    }
}

public class CashCardManager : MonoBehaviour
{
    private List<CashCard> cashCards = new List<CashCard>();

    public void PopulateCashCards(string cards)
    {
        cashCards = new List<CashCard>();
        string[] cardsInfo = cards.Split(';');

        foreach(var cardInfo in cardsInfo)
        {
            if(!String.IsNullOrEmpty(cardInfo))
                cashCards.Add(new CashCard(cardInfo[0], Convert.ToInt32(new string(cardInfo[1], 1))));
        }
    }

    public List<CashCard> GetCashCards()
    {
        

        return cashCards;
    }

    public List<CashCard> SortCashCard()
    {
        return cashCards.OrderByDescending(o => o.equity).ToList();
    }

    public string CashCardsInfo()
    {
        string result = string.Empty;

        foreach(var card in SortCashCard())
        {
            result += card.Info() + '\n';
        }

        return result;
    }

    public CashCard HighestCashCard()
    {
        CashCard result = new CashCard("TempCamel", -10);

        foreach (var card in cashCards)
        {
            if (card.equity > result.equity)
            {
                result = card;
            }
        }

        return result;
    }

}
