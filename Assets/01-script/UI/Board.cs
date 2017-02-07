using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Board : MonoBehaviour
{
    [SerializeField]
    private CaseBoard caseBoardPrefab;

    [SerializeField]
    private List<GameObject> camelsToken;

    [SerializeField]
    private Vector3 camelInitPos;

    [SerializeField]
    private Transform[] casesTransform;

    [SerializeField]
    private List<Toggle> camelToggles;

    [SerializeField]
    private AutoMoveUI autoMoveUI;

    private List<CaseBoard> cases = new List<CaseBoard>();

    [SerializeField]
    private Text boardText;

    private const int NUMBER_OF_CASES = 16;

    private string startingBoard = ";;ywo;B;G";
	private string emptyBoard = ";;;;;;";

    public void Start()
    {
        Setup();
        UpdateBoard(startingBoard);
    }  

    public void Setup()
    {
        camelInitPos = GetCamelToken('W').transform.position;
        CreateCases();
        autoMoveUI.Setup();
        autoMoveUI.onButtonClick += OnAutoMoveUIClick;
    }

    public void ResetBoard()
	{
		UpdateBoard(emptyBoard);
	}

    public void RemoveTrap()
    {
        Debug.Log(GetBoard());
        string board = GetBoard().Replace('+', '\0');
        board = board.Replace('-', '\0');
        Debug.Log(board);
        UpdateBoard(board);
    }

    public GameObject GetCamelToken(char aCamelName)
    {
        int camel = -1;
        switch (aCamelName)
        {
            case 'W': camel = 0; break;
            case 'w': camel = 0; break;
            case 'Y': camel = 1; break;
            case 'y': camel = 1; break;
            case 'O': camel = 2; break;
            case 'o': camel = 2; break;
            case 'B': camel = 3; break;
            case 'b': camel = 3; break;
            case 'G': camel = 4; break;
            case 'g': camel = 4; break;
        }

        return camelsToken[camel];
    }

    public void RepoolCamel(char aCamelName)
    {
        GameObject camel = GetCamelToken(aCamelName);
        camel.transform.position = camelInitPos;
        camel.transform.SetParent(transform);      
    }

    public void RemoveCamelFromAnotherCase(char aCamelName)
    {
        if (!GameManager.Instance.IsCamel(aCamelName))
            return;

        foreach (var caseboard in cases)
        {
            caseboard.RemoveToken(aCamelName);            
        }
    }

    private void UpdateCase(int caseNb, string tokens)
    {
        foreach(CaseBoard caseboad in cases)
        {          
            if(caseboad.CaseNb == caseNb)
            {
                caseboad.UpdateCase(tokens);
            }
        }
    }

    private void UpdateBoard(string board)
    {
        string[] cases = board.Split(';');

        for(int i = 0; i < cases.Length; i++)
        {
            UpdateCase(i, cases[i]);
        }

        for (int i = 0; i < board.Length; i++)
        {
            if(board[i] != ';')
                SetToggle(board[i]);
        }
    }

    private void CreateCases()
    {
        for(int i = 1; i <= NUMBER_OF_CASES; i++)
        {
            AddCase(i <= NUMBER_OF_CASES / 2, i);
        }
    }

    private void AddCase(bool isFirstRow, int nb)
    {
        CaseBoard newCase = GameObject.Instantiate(caseBoardPrefab).GetComponent<CaseBoard>();
        newCase.Setup(nb,ChangeContainerOrder);
        cases.Add(newCase);

        newCase.gameObject.transform.SetParent(isFirstRow ? casesTransform[0] : casesTransform[1]);
        newCase.transform.localScale = Vector3.one;
    }

    private void RemoveCase()
    {
        cases.RemoveAt(0);
        Destroy(transform.GetChild(0).gameObject);
    }

    public void CloseAllOptions(CaseBoard cb)
    {
        foreach(var caseboard in cases)
        {
            if (caseboard != cb)
            {
                caseboard.HideDropdownOption();
            }
        }

        ShowAllContainer();
    }

    public string GetBoard()
    {
        string result = ";";

        foreach(CaseBoard caseboard in cases)
        {
            result += FormatTokenOnCase(caseboard.TokensOnCase) + ";";
        }

        return result;
    }

    private bool IsCamelRolled(char camel)
    {
        switch(camel)
		{
        case 'G': return camelToggles[0].isOn;
		case 'g': return camelToggles[0].isOn;
		case 'B': return camelToggles[1].isOn;
		case 'b': return camelToggles[1].isOn;	
	    case 'Y': return camelToggles[2].isOn;
		case 'y': return camelToggles[2].isOn;
		case 'W': return camelToggles[3].isOn;
		case 'w': return camelToggles[3].isOn;
		case 'O': return camelToggles[4].isOn;
		case 'o': return camelToggles[4].isOn;
		
		default: Debug.LogError("Oh oh spaguatiii-oo"); return false;
		}
    }

    private void SetToggle(char camel)
    {
        switch (camel)
        {
            case 'G': camelToggles[0].isOn = false; break;
            case 'g': camelToggles[0].isOn = true; break;
            case 'B': camelToggles[1].isOn = false; break;
            case 'b': camelToggles[1].isOn = true; break;
            case 'Y': camelToggles[2].isOn = false; break;
            case 'y': camelToggles[2].isOn = true; break;
            case 'W': camelToggles[3].isOn = false; break;
            case 'w': camelToggles[3].isOn = true; break;
            case 'O': camelToggles[4].isOn = false; break;
            case 'o': camelToggles[4].isOn = true; break;

            default: Debug.LogError("Oh oh spaguatiii-oo"); break;
        }
    }

    private string FormatTokenOnCase(string tokens)
    {
        if (string.IsNullOrEmpty(tokens))
            return tokens;

        if (!GameManager.Instance.IsCamel(tokens[0]))
            return tokens;
        
        
        string result = string.Empty;

        foreach(char token in tokens)
        {
            result += IsCamelRolled(token) ? token.ToString().ToLower() : token.ToString().ToUpper();
        }

        return result;
    }

    private void ChangeContainerOrder(CaseBoard caseboard)
    {
        if(caseboard.IsContainerShown && caseboard.CaseNb < NUMBER_OF_CASES / 2)
        {
            casesTransform[1].gameObject.SetActive(false);
        }
        else
        {
            ShowAllContainer();
        }
    }

    private void ShowAllContainer()
    {
        foreach (var container in casesTransform)
            container.gameObject.SetActive(true);
    }

    private void OnAutoMoveUIClick(string camel, int diceNumber)
    {
        AllCamels currentCamels = new AllCamels(GetBoard());
        currentCamels.MoveCamel(camel, diceNumber, true);
        UpdateBoard(currentCamels.GetBoard());
    }

    private void UpdateBoardText(string board)
    {
        boardText.text = board;
    }

    private void Update()
    {
        UpdateBoardText(GetBoard());
    }
}
