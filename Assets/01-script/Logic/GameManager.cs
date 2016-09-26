using UnityEngine;
using System;

public class GameManager : MonoSingleton<GameManager>
{
    public CashCardManager cashCardManager;
    public RollDiceManager rollDiceManager;

    private void Start()
    {
        CreateManager();
    }


    public string CreateBoard(string board, string cardRemaining)
    {     
        StatFactory statFactory = new GameObject("StatFactory").AddComponent<StatFactory>(); ;
        statFactory.CreateBoard(board);
        statFactory.CestLaQueLaPoutineSePasse();


        cashCardManager.PopulateCashCards(cardRemaining);
        statFactory.FindEquityCashCard(cashCardManager.GetCashCards());

        CashCard highestCard = cashCardManager.HighestCashCard();
        Case highestCase = statFactory.rankCounts.HighestCase(statFactory.initialCamels);

        string result = cashCardManager.CashCardsInfo();

        result += "\n";

        result += highestCase.Info();

        if (statFactory != null)
            Destroy(statFactory);

        return result;
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
        if (cashCardManager != null)
            Destroy(cashCardManager);

        if (rollDiceManager != null)
            Destroy(rollDiceManager);

        cashCardManager = new GameObject("CashCardManager").AddComponent<CashCardManager>();
        rollDiceManager = new GameObject("RollDiceManager").AddComponent<RollDiceManager>();
    }

    
}
