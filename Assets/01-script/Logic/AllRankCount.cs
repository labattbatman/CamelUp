using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

public enum RankPosition
{
	First,
	Second,
	Third,
	Fourth,
	Fifth,
}


public class RankCount
{
	public string name;

	private int[] timeHappen;

	public int GetPositionNumber(RankPosition pos)
	{
		return timeHappen[(int)pos];
	}

	public int GetCourseNumber
	{
		get
		{
			int result = 0;
			foreach(int time in timeHappen)
			{
				result += time;
			}
			return result;
		}    
	}   

	public float GetPercent(RankPosition pos)
	{
		return (float)timeHappen[(int)pos] / (float)GetCourseNumber;
	}
	public void SetPosition(RankPosition pos)
	{
		timeHappen[(int)pos]++;
	}

	public RankCount(string name)
	{
		this.name =  name;
		timeHappen = new int[AllRankCount.GetRankCount()];

		for (int i = 0; i < timeHappen.Length; i++)
			timeHappen [i] = 0;
	}

    public RankCount(RankCount myRankCount)
    {
        this.name = myRankCount.name;
        timeHappen = new int[AllRankCount.GetRankCount()];

        for (int i = 0; i < timeHappen.Length; i++)
        {
            timeHappen[i] = myRankCount.GetPositionNumber((RankPosition)i);
        }
    }

	public void ConsumeRankCount(RankCount consumedRankCount)
	{
		for(int i = 0; i< AllRankCount.GetRankCount(); i++)
		{
			timeHappen[i] += consumedRankCount.timeHappen[i];
		}
	}
}

public struct Case
{
    public int pos;
    public int nbVisite;
    public float equity;

    public string Info()
    {
        return string.Format("Pos: {0}: Visite: {1}, Equity: {2}", pos, nbVisite, equity);
    }
}

public class AllRankCount 
{
	private List<RankCount> rankCounts;

	private RankCount yellow;
	private RankCount blue;
	private RankCount orange;
	private RankCount green;
	private RankCount white;

    private Dictionary<int, int> casesVisited = new Dictionary<int, int>();

    public static int GetRankCount()
	{
		return Enum.GetNames(typeof(RankPosition)).Length;
	}

	public AllRankCount()
	{
		yellow = new RankCount ("Yellow");
		blue = new RankCount ("Blue");
		orange = new RankCount ("Orange");
		green = new RankCount ("Green");
		white = new RankCount ("White");

		rankCounts = new List<RankCount> ();
		rankCounts.Add (yellow);
		rankCounts.Add (blue);
		rankCounts.Add (orange);
		rankCounts.Add (green);
		rankCounts.Add (white);
	}

	public void UpdateRankCount(List<Camel> camels)
	{
		for(int i = 0; i < camels.Count; i++)
		{
			GetRankCount (camels [i].name).SetPosition ((RankPosition)i);
        }

		//InfoRankCount ();
	}

	public void InfoRankCount(string text = "")
	{
		string infoRank = text + "\n";
        List<RankCount> sortedRankCount = SortRankCount();

        for (int i = 0; i < sortedRankCount.Count; i++)
		{
			int totalRank = sortedRankCount[i].GetCourseNumber;
			infoRank += sortedRankCount[i].name + ": \t";
		    int paddingLeft = Mathf.Min(totalRank.ToString().Length, 3);

            for (int j = 0; j < GetRankCount(); j++)
			{
                float percent = (float) sortedRankCount[i].GetPositionNumber((RankPosition)j) / (float)totalRank;
                infoRank += ((RankPosition)j).ToString() + " " + sortedRankCount[i].GetPositionNumber((RankPosition)j).ToString().PadLeft(paddingLeft, '0') + "/" + totalRank + " " + percent.ToString("0.00") + "  \t";
			}

            infoRank += "\n";
        }

		Debug.Log (infoRank);
	}

	public RankCount GetRankCount(string name)
	{
		for(int i = 0; i < rankCounts.Count; i++)
		{
			if (rankCounts [i].name == name)
				return rankCounts [i];
		}

		Debug.LogError (string.Format ("Je n'ai pas trouvé le rankCount {0}", name));

		return null;
	}

    private List<RankCount> SortRankCount()
    {
        List<RankCount> newList = new List<RankCount>();
        List<RankCount> remainingRankCount = new List<RankCount>(rankCounts);

        for(int i = 0; i < rankCounts.Count; i++)
        {
            RankCount higherRankCount = new RankCount("temp RankCount");

            for(int j = 0; j < remainingRankCount.Count; j++)
            {
                if(remainingRankCount[j].GetPositionNumber(RankPosition.First) > higherRankCount.GetPositionNumber(RankPosition.First))
                {
                    higherRankCount = new RankCount(remainingRankCount[j]);
                }
                else if (remainingRankCount[j].GetPositionNumber(RankPosition.First) == higherRankCount.GetPositionNumber(RankPosition.First) &&
                        remainingRankCount[j].GetPositionNumber(RankPosition.Second) > higherRankCount.GetPositionNumber(RankPosition.Second))
                {
                    higherRankCount = new RankCount(remainingRankCount[j]);
                }
                else if (remainingRankCount[j].GetPositionNumber(RankPosition.First) == higherRankCount.GetPositionNumber(RankPosition.First) &&
                        remainingRankCount[j].GetPositionNumber(RankPosition.Second) == higherRankCount.GetPositionNumber(RankPosition.Second) &&
                        remainingRankCount[j].GetPositionNumber(RankPosition.Third) > higherRankCount.GetPositionNumber(RankPosition.Third))
                {
                    higherRankCount = new RankCount(remainingRankCount[j]);
                }
                else if (remainingRankCount[j].GetPositionNumber(RankPosition.First) == higherRankCount.GetPositionNumber(RankPosition.First) &&
                        remainingRankCount[j].GetPositionNumber(RankPosition.Second) == higherRankCount.GetPositionNumber(RankPosition.Second) &&
                        remainingRankCount[j].GetPositionNumber(RankPosition.Third) == higherRankCount.GetPositionNumber(RankPosition.Third) &&
                        remainingRankCount[j].GetPositionNumber(RankPosition.Fourth) > higherRankCount.GetPositionNumber(RankPosition.Fourth))
                {
                    higherRankCount = new RankCount(remainingRankCount[j]);
                }
                else if (remainingRankCount[j].GetPositionNumber(RankPosition.First) == higherRankCount.GetPositionNumber(RankPosition.First) &&
                        remainingRankCount[j].GetPositionNumber(RankPosition.Second) == higherRankCount.GetPositionNumber(RankPosition.Second) &&
                        remainingRankCount[j].GetPositionNumber(RankPosition.Third) == higherRankCount.GetPositionNumber(RankPosition.Third) &&
                        remainingRankCount[j].GetPositionNumber(RankPosition.Fourth) == higherRankCount.GetPositionNumber(RankPosition.Fourth) &&
                        remainingRankCount[j].GetPositionNumber(RankPosition.Fifth) > higherRankCount.GetPositionNumber(RankPosition.Fifth))
                {
                    higherRankCount = new RankCount(remainingRankCount[j]);
                }                   
            }
            for (int k = 0; k < remainingRankCount.Count; k++)
            {
                if (remainingRankCount[k].name == higherRankCount.name)
                    remainingRankCount.Remove(remainingRankCount[k]);
            }

            newList.Add(higherRankCount);
        }

        return newList;
    }

    public void UpdateCasesVisited(int newPos)
    {
        if (casesVisited.ContainsKey(newPos))
        {
            casesVisited[newPos]++;
        }
        else
        {
            casesVisited.Add(newPos, 1);
        }
    }

    public Case HighestCase(AllCamels camels)
    {
        Case result = new Case();
        int totalVisite = 0;


        foreach (var caseVisited in casesVisited)
        {
            totalVisite += caseVisited.Value;

            if (camels.CanPutTrap(caseVisited.Key) && caseVisited.Value >= result.nbVisite)
            {
                result.pos = caseVisited.Key;
                result.nbVisite = caseVisited.Value;
            }
        }

        result.equity = (float)result.nbVisite / (totalVisite / camels.GetUnrollCamelsCount());

        return result;
    }

}
