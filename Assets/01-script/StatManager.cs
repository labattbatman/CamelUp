using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

public class StatManager : MonoBehaviour
{

    public static bool isBugged = false;
		
    public AllCamels initialCamels = new AllCamels();
    public AllCamels gameCamels = new AllCamels();
	public AllRankCount rankCounts = new AllRankCount();

    public Dictionary<string, Dictionary<string, string>> logRank = new Dictionary<string, Dictionary<string, string>>();


    // Use this for initialization
    void Start ()
    {
        ReadInfoFile();

        gameCamels = new AllCamels (initialCamels);
        gameCamels.ShortInfoCamel();
        if (isBugged)
            gameCamels.InfoCamel("GameCamel First");

        List<AllCamels> allPermutations = gameCamels.AllUnrollCamelsPermutation();
        foreach (var permu in allPermutations)
        {
            if(isBugged)
                permu.InfoCamel("Permuration de camels",permu.OrderedCamelsForDice);

			rankCounts = MoveWithAllDicesCombo (permu, rankCounts);
		}
       
		rankCounts.InfoRankCount ();

        UnityEngine.Debug.Log(LogRankToInfo(initialCamels.InfoCamel(), DicesCombinationsPossible(initialCamels.GetUnrollCamelsCount())));
    }

    //Get Initial information
    void ReadInfoFile()
    {
        string pathCSV = Directory.GetCurrentDirectory() + "/Camel-Up.txt";

        var reader = new StreamReader(File.OpenRead(pathCSV));

        int pos = 1;

        while (!reader.EndOfStream)
        {
            string line = reader.ReadLine();

            if (!string.IsNullOrEmpty(line))
            {
                Camel lastCamel = null;
                for (int i = line.Length -1; i >= 0; i--)
                {
                    if (line[i] == '+' || line[i] == '-')
                    {
                        CreateTrap(line[i], pos);
                    }
                    else
                    {
                        lastCamel = CreateNewCamelinInitialCamels(line[i], pos, lastCamel);
                    }           
                }             
            }
            pos++;
        }

        reader.Close();
    }

    Camel CreateNewCamelinInitialCamels(char name, int pos, Camel lastCamelCreated)
    {
        Camel newCamel = initialCamels.GetCamel(name);
        newCamel.pos = pos;

        newCamel.isDiceRoll = char.IsLower(name);

        if(lastCamelCreated != null)
        {
            if(lastCamelCreated.pos == newCamel.pos)
            {
                newCamel.camelsOnTop = lastCamelCreated;
            }
        }

        return newCamel;
    }

    Trap CreateTrap(char kind, int pos)
    {
        Trap newTrap = new Trap(kind, pos);
        initialCamels.AddTrap(newTrap);
        return newTrap;
    }

	AllRankCount MoveWithAllDicesCombo(AllCamels allCamels, AllRankCount ranks)
    {
		List<List<int>> dices = DicesCombinationsPossible(allCamels.GetUnrollCamelsCount());
        Dictionary<string, string> dicesAndRank = new Dictionary<string, string>();

        foreach (List<int> d in dices)
        { 
			AllCamels tempCamels = new AllCamels(allCamels);
			tempCamels.MoveCamels(d);
			ranks.UpdateRankCount (tempCamels.SortCamelInOrderPos());

            if(dicesAndRank.ContainsKey(DicesToString(d)))
                UnityEngine.Debug.Log("dfdsa");

            dicesAndRank.Add(DicesToString(d), tempCamels.ShortInfoCamel());
        }

        logRank.Add(allCamels.OrderForDiceInfoCamel(), dicesAndRank);

        return ranks;
    }

	List<List<int>> DicesCombinationsPossible(int input)
	{
		List<List<int>> dice = new List<List<int>>();
		int numberOfDice = input;
		const int diceFace = 3;
		int indexNumber = (int)Math.Pow(diceFace, numberOfDice);
		int range = (int)Math.Pow(diceFace, numberOfDice) / diceFace;

		for (int i = 0; i < (int)Math.Pow (diceFace, numberOfDice); i++)
			dice.Add (new List<int> ());

		int diceNumber = 1;
		int counter = 0;

		for (int i = 1; i <= indexNumber; i++)
		{
			if (range != 0)
			{
				dice[i - 1].Add(diceNumber);
				counter++;
				if (counter == range)
				{
					counter = 0;
					diceNumber++;
				}
				if (i == indexNumber)
				{
					range /= diceFace;
					i = 0;
				}
				if (diceNumber == diceFace + 1)
				{
					diceNumber = 1;
				}
			}
		}
		return dice;
	}

    // rankLog
    string DicesToString(List<int> dices)
    {
        string result = string.Empty;

        foreach (var dice in dices)
        {
            result += dice + "-";
        }

        return result.Remove(result.Length - 1,1);
    }

    string LogRankToInfo(string initialInfo)
    {
        UnityEngine.Debug.LogError("Pas sur que ca marche!!!");
        string result = initialInfo + "\n";
        Dictionary<string, string> resultByDice = new Dictionary<string, string>();
        string title = ",";

        foreach (KeyValuePair<string, Dictionary<string, string>> entry in logRank)
        {
            title += entry.Key + ",";

            foreach (KeyValuePair<string, string> entryDice in entry.Value)
            {
                if (!resultByDice.ContainsKey(entryDice.Key))
                {
                    resultByDice.Add(entryDice.Key, entryDice.Key);
                }
                else
                {
                    resultByDice[entryDice.Key] += "," + entryDice.Value;
                }
            }
        }

        result += title + "\n";

        foreach (KeyValuePair<string, string> entry in resultByDice)
        {
            result += entry.Value + "\n";
        }
       
        return result;
    }

    string LogRankToInfo(string initialInfo, List<List<int>> dices)
    {
        string result = initialInfo + "\n";
        Dictionary<string, string> resultByDice = new Dictionary<string, string>();
        string title = ",";

        List<string> dicesInString = new List<string>();

        foreach(var d in dices)
        {
            dicesInString.Add(DicesToString(d));
        }

        foreach (KeyValuePair<string, Dictionary<string, string>> entry in logRank)
        {
            title += entry.Key + ",";
        }

        result += title + "\n";

        foreach(var diceCombi in dicesInString)
        {
            string dicesResult = diceCombi + ",";

            foreach (KeyValuePair<string, Dictionary<string, string>> entry in logRank)
            {
                dicesResult += entry.Value[diceCombi] + ",";
            }

            result += dicesResult + "\n";
        }

        return result;
    }

}

