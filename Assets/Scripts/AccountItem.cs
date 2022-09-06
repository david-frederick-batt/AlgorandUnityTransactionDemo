using Algorand.Algod.Model;
using UnityEngine;
using TMPro;
using Algorand.Algod;
using System.Threading.Tasks;

public class AccountItem : MonoBehaviour
{
    [SerializeField] TMP_Text nameField;
    [SerializeField] TMP_Text addressField;
    [SerializeField] TMP_Text balanceField;

    [HideInInspector] public string AccountName;
    public Account account;

    static int counter = 0;

    DefaultApi algodApiInstance;

    public void Initialize(DefaultApi algodApiInstance)
    {
        this.algodApiInstance = algodApiInstance;
        counter++;
        
        AccountName = "Account " + counter.ToString();
        nameField.text = AccountName;
        addressField.text = "Address: " + account.Address.ToString();

        UpdateBalance();
    }

    public async void UpdateBalance()
    {
        var accountInfo = await algodApiInstance.AccountInformationAsync(account.Address.ToString(), null, null);
        balanceField.text = string.Format("Balance: {0} microalgos", accountInfo.Amount);
    }

    public async Task<ulong> GetBalance()
    {
        var accountInfo = await algodApiInstance.AccountInformationAsync(account.Address.ToString(), null, null);
        return accountInfo.Amount;
    }
}
