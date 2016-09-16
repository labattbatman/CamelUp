using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Okay : MonoBehaviour
{
    /*
    12345:12 345,54321,23 4 1 5,2  41 53
    12 345:12345,54321,23 4 1 5,2  41 53
    23 4 1 5:12 345,54321,12345,2  41 53
    2  41 53:12 345,54321,23 4 1 5,12345
    */

    private Dictionary<Pattern, List<Pattern>> AllMyPatterns;

    class Pattern
    {
        public string name;

        public Camel camel1;
        public Camel camel2;
        public Camel camel3;
        public Camel camel4;
        public Camel camel5;

        public List<Camel> camels; 

        public Pattern(string myName)
        {
            name = myName;

            camel1 = new Camel();
            camel2 = new Camel();
            camel3 = new Camel();
            camel4 = new Camel();
            camel5 = new Camel();

            camel1.name = "1";
            camel2.name = "2";
            camel3.name = "3";
            camel4.name = "4";
            camel5.name = "5";

            camels = new List<Camel>();

            camels.Add(camel1);
            camels.Add(camel2);
            camels.Add(camel3);
            camels.Add(camel4);
            camels.Add(camel5);
        }

        public Camel GetCamel(string name)
        {
            for (int i = 0; i < camels.Count; i++)
            {
                if (camels[i].name == name)
                {
                    return camels[i];
                }
            }

            UnityEngine.Debug.LogError(string.Format("Didnt find {0} camel in {1} pattern", name, this.name));

            return null;
        }

        public void InfoPattern()
        {
            string info = string.Empty;

            for (int i = 0; i < camels.Count; i++)
            {
                info += string.Format("Name: {0} Pos: {1} CamelOnTop: {2}", camels[i].name, camels[i].pos, camels[i].camelOnTop != null ? camels[i].camelOnTop.name : "No");
                info += "\n";
            }

            UnityEngine.Debug.Log(info);
        }

        public List<Camel> Sorted()
        {
            //TODO
            return new List<Camel>();
        }
    }

    class Camel
    {
        public string name;
        public int pos;
        public Camel camelOnTop;
    }

	// Use this for initialization
	void Start ()
	{
        Pattern test = StringToPattern("51 2 4    3 ");
        ReadPatternFile("12345:12 345, 54321, 23 4 1 5,2  41 53");
        test.InfoPattern();
    }

    Pattern StringToPattern(string paternString)
    {
        if (!paternString.Contains("1") || !paternString.Contains("2") || !paternString.Contains("3") ||
            !paternString.Contains("4") || !paternString.Contains("5"))
        {
            UnityEngine.Debug.LogError("Missing Camel in StringToPatern()");
            return null;
        }

        Pattern result = new Pattern("Test");

        int currentPos = 1;

        for (int i = 0; i < paternString.Length; i++)
        {
            if (paternString[i] == ' ')
            {
                currentPos++;
            }
            else
            {
                result.GetCamel(paternString[i].ToString()).pos = currentPos;

                if (i + 1 < paternString.Length && paternString[i +1] != ' ')
                {
                    result.GetCamel(paternString[i].ToString()).camelOnTop = result.GetCamel(paternString[i + 1].ToString());
                }
            }
        }

        return result;
    }

    string PatternToString(Pattern myPattern)
    {
        List<Camel> sortedPattern = myPattern.Sorted();
        string result = string.Empty;
        int currentPos = 1;

        for (int i = sortedPattern.Count -1; i < 0; i--)
        {
            while (sortedPattern[i].pos != currentPos)
            {
                if (currentPos > sortedPattern[i].pos)
                {
                    UnityEngine.Debug.LogError("Something bad happen here");
                    return result;
                }

                result += " ";
                currentPos++;
            }

            result += sortedPattern[i].name;
        }

        return result;
    }

    void ReadPatternFile(string files)
    {
        AllMyPatterns = new Dictionary<Pattern, List<Pattern>>();

        //Todo pour chaque line

        string[] splitLine = files.Split(':');

        string[] patternResult = splitLine[1].Split(',');
        List<Pattern> patternList = new List<Pattern>();

        for (int i = 0; i < patternResult.Length; i++)
        {
            patternList.Add(StringToPattern(patternResult[i]));
        }

        AllMyPatterns.Add(StringToPattern(splitLine[0]), patternList);
    }

    void WritePatternFile()
    {
        //Todo TESTER
        string result = string.Empty;

        foreach (KeyValuePair<Pattern, List<Pattern>> pattern in AllMyPatterns)
        {
            result += pattern.Key + ":";
            foreach (var p in pattern.Value)
            {
                result += PatternToString(p) + ",";           
            }

            result += "\n";
        }

        UnityEngine.Debug.Log(result);
    }

}
