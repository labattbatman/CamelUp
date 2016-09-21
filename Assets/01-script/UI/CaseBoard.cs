using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;

enum OptionButtonType
{
    Yellow = 0,
    Green,
    White,
    Blue,
    Orange,
    Plus,
    Minus
}

public class CaseBoard : MonoBehaviour
{
    [SerializeField]
    private CustomDropDownMenu dropdown;

    [SerializeField]
    private List<GameObject> traps = new List<GameObject>();

    [SerializeField]
    private List<GameObject> camelPos = new List<GameObject>();

    private List<char> tokensOnCase = new List<char>();

    public Action<char> OnCamelResquested;

    private float posX = 0;
    private float dist = 50;
    private float posY = 0;

    private int caseNb;
    public int CaseNb
    {
        get { return caseNb; }
    }

    public bool IsContainerShown
    {
        get { return dropdown.Container.active; }
    }

    public string TokensOnCase
    {
        get
        {
            string result = string.Empty;
            foreach (var token in tokensOnCase)
                result += token;

            return result;
        }
    }

    public void Setup(int rank, Action<CaseBoard> onMainClick, string tokens = "")
    {
        dropdown.OnDropDownClick += OnDropdownChange;
        dropdown.OnMainButtonClick += onMainClick;
        dropdown.Setup(typeof(OptionButtonType));

        caseNb = rank;
        SetTokenOnCase(tokens);
        UpdateCaseBoardVisual();
    }  

    public void UpdateCase(string tokens)
    {
        SetTokenOnCase(tokens);
        UpdateCaseBoardVisual();
    }

    private void SetTokenOnCase(string tokens)
    {
        tokensOnCase = new List<char>();

        foreach (var token in tokens)
        {
            tokensOnCase.Add(token);
        }
    }   

    #region Drowdown
    private void OnDropdownChange(OptionButton ob)
    {
        char token = ob.optionType[0];
        bool isTokenOnCase = IsTokenOnCase(token);

        if (isTokenOnCase)
        {
            RemoveToken(token);
        }
        else
        {
            AddToken(token);
        }

        UpdateCaseBoardVisual();       
    }

    public void HideDropdownOption()
    {
        dropdown.ShowContainer(false);
    }

    #endregion //Drowdown

    #region Tokens

    private bool IsTokenOnCase(char token)
    {
        return tokensOnCase.Contains(token);
    }

    private string TokensToString()
    {
        string result = string.Empty;

        foreach (var token in tokensOnCase)
            result += token;

        return result;
    }

    private void AddToken(char newToken)
    {
        if (!GameManager.Instance.IsCamel(newToken))
        {
            foreach (var token in tokensOnCase)
            {
                if(GameManager.Instance.IsCamel(token))
                    UIManager.Instance.Board.RepoolCamel(token);
            }
                
            tokensOnCase = new List<char>();
        }

        if(tokensOnCase.Count > 0 && !GameManager.Instance.IsCamel(tokensOnCase[0]))
        {
            tokensOnCase = new List<char>();
        }

        UIManager.Instance.Board.RemoveCamelFromAnotherCase(newToken);
        tokensOnCase.Insert(0,newToken);
        UpdateCaseBoardVisual();
    }

    public void RemoveToken(char oldToken)
    {
        if (tokensOnCase.Contains(oldToken))
        {
            tokensOnCase.Remove(oldToken);

            if(GameManager.Instance.IsCamel(oldToken))
                UIManager.Instance.Board.RepoolCamel(oldToken);

            UpdateCaseBoardVisual();
        }
    }

    #endregion //Drowdown

    #region VisualTokens

    private void HideTrap()
    {
        traps[0].SetActive(false);
        traps[1].SetActive(false);
    }

    private void ShowTrap(bool isPlusTrap)
    {
        if (isPlusTrap)
        {
            traps[0].SetActive(true);
            traps[1].SetActive(false);
        }
        else
        {
            traps[0].SetActive(false);
            traps[1].SetActive(true);
        }
    }

    private void SetTokenGameObject(char token, int pos = 0)
    {
        if (GameManager.Instance.IsCamel(token))
        {
            GameObject go = UIManager.Instance.Board.GetCamel(token);
            go.transform.position = camelPos[pos].transform.position;
            go.transform.SetParent(transform);
        }
        else
        {
            ShowTrap(token == 'P');
        }
    }

    private void UpdateCaseBoardVisual()
    {
        string text = caseNb + "." + Environment.NewLine + TokensToString();
        dropdown.UpdateDropDown(text);
        HideTrap();

        for (int i = tokensOnCase.Count; i > 0; i--)
        {
            SetTokenGameObject(tokensOnCase[i - 1], i - 1);
        }
    }

    #endregion //Drowdown

    #region Drowdgfdgfdsgown

    #endregion //Drowdown

}
