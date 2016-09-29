using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


//todo avec tempCamels sortir les stats
//accumuler les stats
//comparer les stats avec les stats d'origin(ils sont ailloeur, ne pas les refaire)
// sortir un ev magique prenant en compte GameRules.ROLL_DICE_REWARD


public class RollDiceManager : MonoBehaviour
{   
    public void GetRollDiceEquity(string board, string remainingCard)
    {       
        AllCamels originAllCamels = new AllCamels(board);

        List<Camel> unRollCamels = originAllCamels.GetUnrollCamels().ToList();
        /*
        UnityEngine.Debug.LogWarning(board);
        foreach (var unRollCamel in unRollCamels)
        {
            for (int dice = 0; dice < GameRules.DICES_FACES; dice++)
            {
                AllCamels tempCamels = new AllCamels(board);
                tempCamels.MoveCamel(unRollCamel.name, dice);

                StatFactory statFactory =new StatFactory();
                statFactory.CreateBoard(board);
                statFactory.CestLaQueLaPoutineSePasse();


                //cashCardManager.PopulateCashCards(cardRemaining);
                //statFactory.FindEquityCashCard(cashCardManager.GetCashCards());
            }                    
        }*/
    }
	
}
