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
        StatFactory statFactory = CreateStatFactory();
        statFactory.CreateBoard(board);
        statFactory.CestLaQueLaPoutineSePasse();


        cashCardManager.PopulateCashCards(cardRemaining);
        statFactory.FindEquityCashCard(cashCardManager.GetCashCards());

        rollDiceManager.GetRollDiceEquity(board, cardRemaining);

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

    public StatFactory CreateStatFactory()
    {
        return new GameObject("StatFactory").AddComponent<StatFactory>();
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
