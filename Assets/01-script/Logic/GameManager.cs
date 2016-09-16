using UnityEngine;
using System;

public class GameManager : MonoSingleton<GameManager>
{
    public StatManager statManager;
    public CashCardManager cashCardManager;

    public string CreateBoard(string board, string cardRemaining)
    {
        CreateManager();

        statManager.CreateBoard(board);
        statManager.CestLaQueLaPoutineSePasse();


        cashCardManager.PopulateCashCards(cardRemaining);
        statManager.FindEquityCashCard(cashCardManager.GetCashCards());

        CashCard highestCard = cashCardManager.HighestCashCard();
        Case highestCase = statManager.rankCounts.HighestCase(statManager.initialCamels);

        string result = cashCardManager.CashCardsInfo();

        result += "\n";

        result += highestCase.Info();

        return result;
    }

    public string GetTokensOnCase(int caseNb)
    {
        return statManager.GetTokensOnCase(caseNb);
    }


    //Info Util
    public bool IsCamel(char token)
    {
        if (token != 'M' && token != 'P')
            return true;

        return false;
    }

    private void CreateManager()
    {
        if (statManager != null)
            Destroy(statManager);

        if (statManager != null)
            Destroy(statManager);

        statManager = new GameObject("StatManager").AddComponent<StatManager>();
        cashCardManager = new GameObject("CashCardManager").AddComponent<CashCardManager>();
    }

    
}
