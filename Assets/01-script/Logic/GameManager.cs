using UnityEngine;
using System;

public class GameManager : MonoSingleton<GameManager>
{
    public string CreateBoard(string board, string cardRemaining)
    {
        StatFactory statFactory = new StatFactory();
        statFactory.CreateBoard(board);
        statFactory.CestLaQueLaPoutineSePasse();

        CashCardManager cashCardManager = new CashCardManager();
        RollDiceManager rollDiceManager = new RollDiceManager();

        cashCardManager.PopulateCashCards(cardRemaining);
        statFactory.FindEquityCashCard(cashCardManager.GetCashCards());

        rollDiceManager.GetRollDiceEquity(board, cardRemaining);

        CashCard highestCard = cashCardManager.HighestCashCard();
        Case highestCase = statFactory.rankCounts.HighestCase(statFactory.initialCamels);

        string result = cashCardManager.CashCardsInfo();

        result += "\n";

        result += highestCase.Info();

        return result;
    }

    //Info Util
    public bool IsCamel(char token)
    {
        if (token != 'M' && token != 'P')
            return true;

        return false;
    }   
}
