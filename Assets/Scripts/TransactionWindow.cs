using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;

public class TransactionWindow : MonoBehaviour
{
    public TMP_Dropdown from;
    public TMP_Dropdown to;
    public TMP_InputField amount;
    [SerializeField] TMP_Text _errorText;
    public Button submitButton;
    public void DisplayError(string errorMessage)
    {
        _errorText.text = errorMessage;
    }

    private void RefreshAccountList()
    {
        from.ClearOptions();
        to.ClearOptions();
        var accountNames = new List<string>();

        foreach (var acc in Manager.accountItems)
        {
            accountNames.Add(acc.AccountName);
        }

        from.AddOptions(accountNames);
        to.AddOptions(accountNames);
    }

    public void AddAccountToList()
    {
        var option = new List<string>();
        option.Add(Manager.accountItems[Manager.accountItems.Count - 1].AccountName);
        from.AddOptions(option);
        to.AddOptions(option);

        // ListAccountsExludingSelected();
    }

    public void SetButtonStatusToLoading(bool value)
    {
        if (value == true)
        {
            submitButton.interactable = false;
            submitButton.GetComponentInChildren<TMP_Text>().text = "Submitting...";
        }

        if (value == false)
        {
            submitButton.interactable = true;
            submitButton.GetComponentInChildren<TMP_Text>().text = "Submit Transaction";
        }
    }

    /*
    public void ListAccountsExludingSelected()
    {
        to.ClearOptions();
        var options = new List<TMP_Dropdown.OptionData>(from.options);
        options.RemoveAt(from.value);
        
        to.AddOptions(options);
    }
    */

    private void OnEnable()
    {
        SetButtonStatusToLoading(false);
        GetComponent<RectTransform>().anchoredPosition= Vector2.zero;
        DisplayError(string.Empty);
        RefreshAccountList();
    }

    private void OnDisable()
    {
        from.ClearOptions();
        to.ClearOptions();
    }
}
