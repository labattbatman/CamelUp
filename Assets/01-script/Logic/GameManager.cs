using UnityEngine;
using System;

public class GameManager : MonoSingleton<GameManager>
{
    public string CreateBoard(string board, string cardRemaining)
    {
        Result result = new Result();

        result.CreateResult(board, cardRemaining);

        return result.GetResultInDetail();      
    }

    public void CreateAllTheStat()
    {

    }

    //Info Util
    public bool IsCamel(char token)
    {
        if (token != 'M' && token != 'P')
            return true;

        return false;
    }   
}
