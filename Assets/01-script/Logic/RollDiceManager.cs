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
    private float equity;

    public float Equity
    {
        get { return equity; }
    }

    public void GetRollDiceEquity(string board, string remainingCard)
    {       
        //il va avoir un bug quand il reste un des à jouer car il faut remettre tous les camels à isrolldice = false;
        AllCamels originAllCamels = new AllCamels(board);
        List<AllCamels> test = originAllCamels.GetAllPossibleCamelsNextDice();

        for (int i = 0; i < test.Count; i++)
        {            
            return;

            float t = Time.realtimeSinceStartup;
            Result result = new Result();

            //todo mettre cash card = 0
            //todo tres tres lent à calculer essayer un forloop
            //juste faire ca quand lev des autres est moins de 1
            result.CreateResult(test[i].GetBoard(), remainingCard, false);

            Debug.Log(i + " :" + (Time.realtimeSinceStartup - t));
        }
    }
	
}
