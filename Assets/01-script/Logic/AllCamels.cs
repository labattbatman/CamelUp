using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


public class Camel
{
	public string name;
	public int pos;
	public bool isDiceRoll = true;
	public Camel camelsOnTop;

	public Camel(string camelName)
	{
		name = camelName;
	}

	public Camel(Camel oldCamel)
	{
		name = oldCamel.name;
		pos = oldCamel.pos;
		isDiceRoll = oldCamel.isDiceRoll;
	}
}

public class Trap
{
    public bool isPlusTrap;
    public int pos;

    public Trap(char kind, int pos)
    {
        isPlusTrap = kind == '+';
        this.pos = pos;
    }
}

public class AllCamels
{
	public Camel blue;
	public Camel white;
	public Camel orange;
	public Camel yellow;
	public Camel green;
   
	private List<Camel> camels;

    private List<Camel> orderedCamelsForDice;

    public List<Camel> OrderedCamelsForDice
    {
        get { return orderedCamelsForDice; }
    }

    private List<Trap> allTraps;
    public List<Trap> AllTraps
    {
        get { return allTraps; }
    }

    public AllCamels()
	{
		blue = new Camel("Blue");
		white = new Camel("White");
		orange = new Camel("Orange");
		yellow = new Camel("Yellow");
		green = new Camel("Green");

		camels = new List<Camel>();

		camels.Add(blue);
		camels.Add(white);
		camels.Add(orange);
		camels.Add(yellow);
		camels.Add(green);

        allTraps = new List<Trap>();
    }

	public AllCamels(AllCamels myCamels)
	{
		blue = new Camel(myCamels.blue);
		white = new Camel(myCamels.white);
		orange = new Camel(myCamels.orange);
		yellow = new Camel(myCamels.yellow);
		green = new Camel(myCamels.green);

		camels = new List<Camel> ();

		camels.Add(blue);
		camels.Add(white);
		camels.Add(orange);
		camels.Add(yellow);
		camels.Add(green);

        allTraps = new List<Trap>(myCamels.allTraps);

        orderedCamelsForDice = new List<Camel>();

        if(myCamels.orderedCamelsForDice != null)
        {
            foreach (var myCamel in myCamels.orderedCamelsForDice)
	        {
                orderedCamelsForDice.Add(GetCamel(myCamel.name));
            }
        }

		for(int i = 0; i < myCamels.GetCamels().Count; i++)
		{
			Camel camel = myCamels.GetCamel (i);

			if(camel.camelsOnTop != null)
			{
				GetCamel (camel.name).camelsOnTop = GetCamel (camel.camelsOnTop.name);
			}				
		}
	}

    public AllCamels(List<Camel> myCamels, List<Trap> myTraps)
    {
        blue = new Camel("Blue");
        white = new Camel("White");
        orange = new Camel("Orange");
        yellow = new Camel("Yellow");
        green = new Camel("Green");

        camels = new List<Camel>();

        camels.Add(blue);
        camels.Add(white);
        camels.Add(orange);
        camels.Add(yellow);
        camels.Add(green);

        allTraps = new List<Trap>(myTraps);

        foreach (Camel myCamel in myCamels)
        {
            Camel camel = GetCamel(myCamel.name);
            camel.pos = myCamel.pos;
            camel.isDiceRoll = myCamel.isDiceRoll;

            if (myCamel.camelsOnTop != null)
            {
                GetCamel(camel.name).camelsOnTop = GetCamel(myCamel.camelsOnTop.name);
            }
        }
    }

	#region Private Function
	private List<Camel> GetCamels()
	{
		List<Camel> result = new List<Camel>();

		result.Add(blue);
		result.Add(white);
		result.Add(orange);
		result.Add(yellow);
		result.Add(green);

		return result;
	}

	private string FindCamelName(char color)
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

	#endregion //PrivateFunction

	#region Public Function

    public void AddTrap(Trap trap)
    {
        if (allTraps == null)
            allTraps = new List<Trap>();

        allTraps.Add(trap);
    }

    public List<Camel> ToList()
    {
        return camels;
    } 

	public Camel GetCamel(int i)
	{
		return GetCamels()[i];
	}

	public Camel GetCamel(string name)
	{
		for(int i = 0; i < camels.Count; i ++)
		{
			if (camels[i].name == name)
				return camels[i];
		}

		UnityEngine.Debug.LogError(string.Format("J'ai pas trouvé {0}.", name));
		return null;
	}

	public Camel GetCamel(char name)
	{
		return GetCamel(FindCamelName (name));
	}

	public int GetUnrollCamelsCount()
	{
		return GetUnrollCamels().Count();
	}

    public IEnumerable<Camel> GetRollCamels()
    {
        return GetCamels().Where(x => x.isDiceRoll).Distinct();
    }

    public IEnumerable<Camel> GetUnrollCamels()
	{
		return GetCamels().Where(x => !x.isDiceRoll).Distinct();
	}

	public void IsCamelLandOnAnotherCamel(string name)
	{
		Camel movingCamel = GetCamel (name);

		for (int i = 0; i < camels.Count; i++)
		{
			if (camels[i] != movingCamel && 
				camels[i].pos == movingCamel.pos && 
				camels[i].camelsOnTop == null)
			{
				camels[i].camelsOnTop = movingCamel;
			}
		}
	}

	public void RemoveCamelOnTop(string name)
	{
		for (int i = 0; i < camels.Count; i++)
		{
			if (camels[i].camelsOnTop != null && camels[i].camelsOnTop.name == name)
			{
				camels[i].camelsOnTop = null;
			}
		}
	}

	//MoveCamel
    private void MoveCamelUnder(string name)
    {
        Camel camel = GetCamel(name);
        Camel camelPos = null;
        List<Camel> sortedCamel = SortCamelInOrderPos();


        for(int i = sortedCamel.Count -1; i >= 0; i--)
        {
            if(sortedCamel[i].pos == camel.pos -1)
            {
                camelPos = sortedCamel[i];
                break;
            }
        }

        camel.pos--;
        camel.camelsOnTop = camelPos;
    }

	private void MoveCamel(string name, int dice, bool isFirstCamel = true)
	{
		//Debug.Log(string.Format("Je move {0} de {1} et 1erCamel {2}",name,dice,isFirstCamel));
		Camel camel = GetCamel(name);

		camel.pos += dice;

        if (isFirstCamel)
        {
            RemoveCamelOnTop(camel.name);
        }
		
		IsCamelLandOnAnotherCamel(camel.name);

        if (camel.camelsOnTop != null)
		{
			MoveCamel(camel.camelsOnTop.name, dice, false);
		}

        IsLandingOnTrap(camel);

        //InfoCamel ("Post MoveCamel");
    }

	public void MoveCamels(List<int> dices, AllRankCount ranks)
	{
        string info = string.Empty;

		if(dices.Count != GetUnrollCamelsCount())
		{
			Debug.LogError (string.Format ("Il y a {0} camels Unroll et {1} dés", GetUnrollCamelsCount (), dices.Count));
			return;
		}

	    int diceIndex = 0;

        for (int i = 0; i < orderedCamelsForDice.Count; i++)
		{
		    if (!orderedCamelsForDice[i].isDiceRoll)
		    {
                              
                MoveCamel(orderedCamelsForDice[i].name, dices[diceIndex]);
		        diceIndex++;

                ranks.UpdateCasesVisited(orderedCamelsForDice[i].pos, orderedCamelsForDice[i]);
		    }
		}
	}

    private bool IsLandingOnTrap(Camel camel)
    {
        foreach(var trap in allTraps)
        {
            if(trap.pos == camel.pos)
            {
                DoTrap(trap, camel);
                return true;
            }
        }

        return false;
    }

    private void DoTrap(Trap trap, Camel camel)
    {
        //Debug.Log(string.Format("Camel {0} land on trap {1}", camel.name, trap.isPlusTrap ? "+" : "-"));

        if(trap.isPlusTrap)
        {
            MoveCamel(camel.name, 1);
        }
        else
        {
            MoveCamelUnder(camel.name);
        }
    }

	public List<Camel> SortCamelInOrderPos()
	{  
		List<Camel> newList = new List<Camel>();
		List<Camel> remainingCamels = new List<Camel>(GetCamels());       

        for (int j = 0; j < GetCamels().Count; j++)
        {
            string tempCamelName = "TempCamel";

            Camel higherCamel = new Camel(tempCamelName);
			higherCamel.pos = 0;

			for (int i = 0; i < remainingCamels.Count; i++)
			{
				Camel currentCamel = remainingCamels[i];

				if(newList.Count > 0 && currentCamel.camelsOnTop == newList[newList.Count - 1])
				{
					//prend le camelOnTop du dernier camel entrer
					higherCamel = currentCamel;
					break;
				}
				else
				{
					//Prend le plus grosse pos + sans camel on top
					if (currentCamel.camelsOnTop == null && currentCamel.pos > higherCamel.pos)
					{
						higherCamel = currentCamel;
					}
                }
			}

			for(int k = 0;  k < remainingCamels.Count; k++)
			{
				if (remainingCamels[k].name == higherCamel.name)
					remainingCamels.Remove(remainingCamels[k]);
			}

            if (higherCamel.name == tempCamelName)
            {
                UnityEngine.Debug.LogError("Didnt find higherCamel");
            }

			newList.Add(higherCamel);
		}

        if(remainingCamels.Count != 0)
        {
            UnityEngine.Debug.LogError("We miss a Camel");
        }

        return newList;
	}

	public List<AllCamels> AllUnrollCamelsPermutation()
	{
        List<AllCamels> result = new List<AllCamels>();
        
        foreach (var unrollCamels in FindAllPermutation(GetUnrollCamels().ToList()))
        {
            List<Camel> allCamelsForDice = GetRollCamels().ToList();

            foreach (var unrollCamel in unrollCamels)
                allCamelsForDice.Add(unrollCamel);

            if(allCamelsForDice.Count != 5 )
            {
                Debug.LogError(string.Format("Pas le bon nombre de camel: {0}", allCamelsForDice.Count));
            }

            AllCamels combo = new AllCamels(allCamelsForDice, allTraps);
            combo.orderedCamelsForDice = allCamelsForDice;

            result.Add(combo);
        }

        return result;
	}
		
	public string InfoCamel(string text = "", List<Camel> infoCamels = null, bool showOnTop = true)
	{
		string camelInfo = text + "\n";

		List<Camel> sortCamels = infoCamels != null? infoCamels : SortCamelInOrderPos (); 

		for(int i = 0; i < sortCamels.Count; i++)
		{       
			Camel camel = sortCamels[i];
			camelInfo += "- " + camel.name + " " + camel.pos + " ";

			if (camel.isDiceRoll)
				camelInfo += "DiceIsRoll ";
			
			if (showOnTop && camel.camelsOnTop != null)
				camelInfo += "sous ->" + camel.camelsOnTop.name + " " ;

			camelInfo += "\n";
		}

        foreach (var trap in allTraps)
        {
            camelInfo += string.Format("- Trap: {0} {1} ", (trap.isPlusTrap ? "+" : "-"), trap.pos);
            camelInfo += "\n";
        }

		Debug.Log(camelInfo);

	    return camelInfo;
	}

    public string ShortInfoCamel()
    {
        string camelInfo = string.Empty;

        List<Camel> sortCamels = SortCamelInOrderPos();

        for (int i = sortCamels.Count - 1; i >= 0; i--)
        {
            Camel camel = sortCamels[i];

            camelInfo += camel.name + camel.pos;

            if (camel.camelsOnTop != null)
                camelInfo += "->" + camel.camelsOnTop.name[0] + " ";

            camelInfo += " ";
        }

        return camelInfo;
    }

    public string OrderForDiceInfoCamel()
    {
        string result = string.Empty;

        for (int i = 0; i < orderedCamelsForDice.Count; i ++)
        {
            if (!orderedCamelsForDice[i].isDiceRoll)
            {
                result += orderedCamelsForDice[i].name + "-";
            }
        }

        return result.Remove(result.Length -1,1);
    }

	#endregion //Public Function

	#region Permutation & Combination
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

        //Debug.Log(result.Count + "!!!!!!!!!!!!!!!!!!!");
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
	#endregion //Permutation & Combination

    //GetInfo
    public string GetTokensOnCase(int caseNb)
    {
        string result = string.Empty;

        foreach(var trap in allTraps )
        {
            if(trap.pos == caseNb)
            {
                result += trap.isPlusTrap ? "+" : "-";
            }
        }

        foreach(var camel in SortCamelInOrderPos())
        {
            result += camel.name[0];
        }

        return result;
    }


    public bool CanPutTrap(int pos)
    {
        foreach(var camel in camels)
        {
            if (camel.pos == pos)
            {
                //Debug.Log("Refuse by Camel; " + pos);
                return false;
            }
        }

        foreach(var trap in allTraps)
        {
            if (Math.Abs(trap.pos - pos) == 1)
            {
                //Debug.Log("Refuse by Trap; " + pos);
                return false;
            }
        }

        return true;
    }

}
