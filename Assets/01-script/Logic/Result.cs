using UnityEngine;


public enum EPlayOption
{
    RollDice = 0,
    TakeCashCard,
    TakeLongTermCashCard,
    PlaceTrap,
}

public class Result
{
    private StatFactory statFactory = new StatFactory();
    private CashCardManager cashCardManager = new CashCardManager();
    private RollDiceManager rollDiceManager = new RollDiceManager();

    public void CreateResult(string board, string cardRemaining, bool getRollDiceEquity = true)
    {
        statFactory.CreateBoard(board);
        statFactory.CestLaQueLaPoutineSePasse();

        cashCardManager.PopulateCashCards(cardRemaining);
        statFactory.FindEquityCashCard(cashCardManager.GetCashCards());

        if (getRollDiceEquity)
        {
            rollDiceManager.GetRollDiceEquity(board, cardRemaining);
        }
    }

    public string GetResultInDetail()
    {
        string result = cashCardManager.CashCardsInfo();
        Case highestCase = statFactory.rankCounts.HighestCase(statFactory.initialCamels);

        result += "\n";

        result += highestCase.Info();

        return result;
    }

    public float BiggestEv(bool withRollDice)
    {
        //todo add longterm cash card
        float result = cashCardManager.HighestCashCard().equity;

        if (statFactory.rankCounts.HighestCase(statFactory.initialCamels).equity > result)
        {
            result = statFactory.rankCounts.HighestCase(statFactory.initialCamels).equity;
        }

        if(withRollDice && rollDiceManager.Equity > result)
        {
            result = rollDiceManager.Equity;
        }

        return result;
    }
}
