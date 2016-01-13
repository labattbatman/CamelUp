using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

public class StatManager : MonoBehaviour {

    class Camel
    {
        public string name;
        public int pos;
        public int newPos;
        public bool isDiceRoll;
        public Camel camelOnTopAtStart;
        public Camel newCamelsOnTop;
    }

    class Result
    {
        public string name;
        public int[] pos;
    }

    List<Camel> allCamels;
    Camel lastCamelCreated;
    List<Result> allResult;

    // Use this for initialization
    void Start ()
    {
        /*Camel testa = new Camel(); testa.newPos = 1;
        Camel testb = new Camel(); testb.newPos = 2;
        allCamels = new List<Camel>();
        allCamels.Add(testa);
        allCamels.Add(testb);
        SortCamelInOrderPos();
        return;*/

        InitiateResult();

        ReadInfoFile();
        
        InfoCamel("Start: ");

        List<Camel> camelsToRoll = new List<Camel>();
        for(int i = 0; i < allCamels.Count; i++)
            if (!allCamels[i].isDiceRoll)
                camelsToRoll.Add(allCamels[i]);

        List<List<Camel>> allSequence = FindAllPermutation(camelsToRoll);

        for (int i = 0; i < allSequence.Count; i++)
            MoveWithAllDicesCombo(allSequence[i]);

        ShowResult();
    }

    void ShowResultTxt(List<int> d)
    {
        string result = "RESULT: ";
        foreach (Camel newCamel in allCamels)
        {
            string camelInfo = newCamel.name + newCamel.newPos + " ";
            if (newCamel.camelOnTopAtStart != null)
            {
                camelInfo += "OnTop: ";
                camelInfo += newCamel.camelOnTopAtStart.name;
            }

            result += " - " + camelInfo;
        }

        Debug.Log(result);
    }

    //Get Initial information
    void ReadInfoFile()
    {
        string pathCSV = Directory.GetCurrentDirectory() + "/Camel-Up.txt";

        var reader = new StreamReader(File.OpenRead(pathCSV));

        int pos = 0;
        allCamels = new List<Camel>();

        while (!reader.EndOfStream)
        {
            string line = reader.ReadLine();

            if (!string.IsNullOrEmpty(line))
            {
                for (int i = line.Length; i > 0; i--)
                {
                    CreateNewCamel(line[i - 1], pos, false);
                    
                }             
            }
            pos++;
        }
    }

    void CreateNewCamel(char name, int pos, bool d)
    {
        Camel newCamel = new Camel();

        newCamel.name = FindCamelName(name);
        newCamel.pos = pos;
        newCamel.newPos = pos;

        newCamel.isDiceRoll = char.IsLower(name);

        if(lastCamelCreated != null)
        {
            if(lastCamelCreated.pos == newCamel.pos)
            {
                newCamel.camelOnTopAtStart = lastCamelCreated;
            }
        }

        lastCamelCreated = newCamel;
        allCamels.Add(newCamel);
    }

    string FindCamelName(char color)
    {
        switch(color)
        {
            case 'B': return "Blue";
            case 'b': return "Blue";
            case 'G': return "Green";
            case 'g': return "Green";
            case 'W': return "White";
            case 'w': return "White";
            case 'O': return "Orange";
            case 'o': return "Orange";
            case 'Y': return "Yellow";
            case 'y': return "Yellow";
            default: return "Unknow Camel";
        }
    }

    Camel GetCamel(string name)
    {
        for (int i = 0; i < allCamels.Count; i++)
            if (allCamels[i].name == name)
                return allCamels[i];

        Debug.LogWarning("Pas trouver le camel");

        return null;
    }

    //MoveCamel
    void MoveCamel(Camel camel, int dice, bool isFirstCamel = true)
    {
        Debug.Log("Dice: " + camel.name + " avance de " + dice);
        camel.newPos = camel.newPos + dice;
        
        if (camel.newCamelsOnTop != null)
        {
            //Debug.Log("2E CAMEL");
            MoveCamel(camel.newCamelsOnTop, dice, false);
        }

        if (isFirstCamel)
            RemoveCamelOnTop(camel);
        
        IsCamelLandOnAnotherCamel(camel);        
    }

    void MoveCamels(List<Camel> camels, List<int> dices)
    {
        for (int i = 0; i < camels.Count; i++)
        {
            MoveCamel(camels[i], dices[i]);
        }
    }

    void MoveWithAllDicesCombo(List<Camel> camels)
    {
        List<List<int>> dices = DicesCombinationsPossible(camels.Count);

        foreach(List<int> d in dices)
        {
            string order = "Order: ";
            foreach (Camel c in camels)
                order += c.name + ",";

            foreach (int a in d)
                order += a.ToString();

            Debug.Log(order);
            ResetCamels();
            
            MoveCamels(camels, d);
            SortCamelInOrderPos();
            EnterResult();
            //ShowResultTxt(d);
        }
    }

    void ResetCamels()
    {
        //Debug.Log("Reset");
        for(int i = 0; i < allCamels.Count; i++)
        {
            allCamels[i].newCamelsOnTop = allCamels[i].camelOnTopAtStart;
            allCamels[i].newPos = allCamels[i].pos;
        }
    }

    void IsCamelLandOnAnotherCamel(Camel movingCamel)
    {
        //Debug.Log(movingCamel.name + movingCamel.newPos);
        for (int i = 0; i < allCamels.Count; i++)
        {
            if(allCamels[i] != movingCamel && allCamels[i].newPos == movingCamel.newPos && allCamels[i].newCamelsOnTop == null && movingCamel.newCamelsOnTop != allCamels[i])
            {
                //Debug.Log("OnTop: " + movingCamel.name + " over " + allCamels[i].name);
                allCamels[i].newCamelsOnTop = movingCamel;
            }
        }
    }

    void RemoveCamelOnTop(Camel camel)
    {
        InfoCamel("preREMOVE");
        for (int i = 0; i < allCamels.Count; i++)
        {
            if (allCamels[i].newCamelsOnTop == camel)
            {
                //LE PROBLEME EST ICI
                allCamels[i].newCamelsOnTop = null;
                Debug.Log(camel.name + " est pu sur " + allCamels[i].name);
                
            }
        }
        InfoCamel("REMOVE");
    }


    //Permutation & Combination
    List<List<Camel>> FindAllPermutation(List<Camel> camels)
    {
        List<List<Camel>> result = new List<List<Camel>>();
        foreach (var permu in Permutate(camels, camels.Count))
        {
            List<Camel> sequence = new List<Camel>();
            foreach (var i in permu)
                sequence.Add(i as Camel);

            result.Add(sequence);
        }

        foreach(List<Camel> c in result)
        {
            string seq = string.Empty;
            foreach (Camel camel in c)
                seq += camel.name + " ";
        }

        return result;
    }

    void RotateRight(IList sequence, int count)
    {
        object tmp = sequence[count - 1];
        sequence.RemoveAt(count - 1);
        sequence.Insert(0, tmp);
    }

    IEnumerable<IList> Permutate(IList sequence, int count)
    {
        if (count == 1) yield return sequence;
        else
        {
            for (int i = 0; i < count; i++)
            {
                foreach (var perm in Permutate(sequence, count - 1))
                    yield return perm;
                RotateRight(sequence, count);
            }
        }
    }

    List<List<int>> DicesCombinationsPossible(int numberOfDice)
    {
        List<List<int>> result = new List<List<int>>();

        result.Add(new List<int> { 1, 1 });
        result.Add(new List<int> { 1, 2 });
        result.Add(new List<int> { 1, 3 });

        result.Add(new List<int> { 2, 1 });
        result.Add(new List<int> { 2, 2 });
        result.Add(new List<int> { 2, 3 });

        result.Add(new List<int> { 3, 1 });
        result.Add(new List<int> { 3, 2 });
        result.Add(new List<int> { 3, 3 });

        return result;
    }

    //Log Camel Info
    void InfoCamel(string text)
    {
        string camelInfo = text;
        for(int i = 0; i < allCamels.Count; i++)
        {       
            camelInfo += " - " + allCamels[i].name + " " + allCamels[i].newPos + " ";
            if (allCamels[i].camelOnTopAtStart != null)
                camelInfo += "sous " + allCamels[i].camelOnTopAtStart.name;         
        }

        Debug.Log(camelInfo);
    }

    void SortCamelInOrderPos()
    {
        List<Camel> newList = new List<Camel>();
        InfoCamel("PreSort: ");
        for (int j = 0; j < 2; j++)
        {
            Camel higherCamel = new Camel();
            higherCamel.newPos = 0;
            for (int i = 0; i < allCamels.Count; i++)
            {
                if(newList.Count == 0)
                {
                    if(allCamels[i].newCamelsOnTop == null && allCamels[i].newPos > higherCamel.newPos)
                    {
                        //Debug.Log("1");
                        higherCamel = allCamels[i];
                    }
                }
                else if(allCamels[i].newCamelsOnTop == newList[newList.Count - 1])
                {
                    //Debug.Log("2");
                    higherCamel = allCamels[i];
                    break;
                }
                else
                {
                    if (allCamels[i].newCamelsOnTop == null && allCamels[i].newPos > higherCamel.newPos)
                    {
                        //Debug.Log("3");
                        higherCamel = allCamels[i];
                    }
                }
            }
            for(int k = 0;  k < allCamels.Count; k++)
            {
                //Debug.Log(allCamels[k].name + higherCamel.name);
                if (allCamels[k].name == higherCamel.name)
                    allCamels.Remove(allCamels[k]);
            }
            //allCamels.Remove(higherCamel);           
            newList.Add(higherCamel);
        }

        allCamels = newList;
        InfoCamel("Sort: ");
    }

    void SwitchCamelInAllCamel(int posA, int posB)
    {
        Debug.Log(allCamels[posA].name + " " + allCamels[posB].name);
        Camel temp = allCamels[posA];
        allCamels[posA] = allCamels[posB];
        allCamels[posB] = temp;
    }

    //Result
    void InitiateResult()
    {
        Result white = new Result();
        white.name = "White";
        white.pos = new int[5];

        Result blue = new Result();
        blue.name = "Blue";
        blue.pos = new int[5];

        Result red = new Result();
        red.name = "Red";
        red.pos = new int[5];

        Result orange = new Result();
        orange.name = "Orange";
        orange.pos = new int[5];

        Result yellow = new Result();
        yellow.name = "Yellow";
        yellow.pos = new int[5];

        allResult = new List<Result>();
        allResult.Add(white);
        allResult.Add(blue);
        allResult.Add(red);
        allResult.Add(orange);
        allResult.Add(yellow);
    }

    void EnterResult()
    {        
        for (int i=0; i < allCamels.Count; i++)
        {
            for(int j = 0; j < allResult.Count; j++)
            {
                //Debug.Log(allResult[j].name + " " + result[i].name);
                if (allResult[j].name == allCamels[i].name)
                {
                    allResult[j].pos[i]++;                 
                }
            }
        }
    }

    void ShowResult()
    {
        foreach (Result camel in allResult)
        {
            int total = 0;
            for (int i = 0; i < camel.pos.Length; i++)
            {
                total += camel.pos[i];
            }

            string result = camel.name + " ";
            for (int i = 0; i < camel.pos.Length; i++)
            {
                result += i+1 + ": " + camel.pos[i] + "/" + total + " ";
                //result += camel.pos[i] / total + "%";
                result += "\n";
            }

            Debug.Log(result);
        }
    }
}
