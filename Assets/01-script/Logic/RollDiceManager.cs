using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class RollDiceManager : MonoSingleton<DecisionManager>
{   
    public void GetRollDiceEquity(string board, string remainingCard)
    {       
        AllCamels originAllCamels = new AllCamels(board);

        List<Camel> unRollCamels = originAllCamels.GetUnrollCamels().ToList();

        UnityEngine.Debug.LogWarning(board);
        foreach (var unRollCamel in unRollCamels)
        {
            AllCamels tempCamels = new AllCamels(board);
            tempCamels.MoveCamel(unRollCamel.name, 1);

            //todo avec tempCamels sortir les stats
            //accumuler les stats
            //comparer les stats avec les stats d'origin(ils sont ailloeur, ne pas les refaire)
            // sortir un ev magique prenant en compte GameRules.ROLL_DICE_REWARD
        }
    }
	
}
