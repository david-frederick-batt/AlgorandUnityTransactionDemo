using UnityEngine;
using System.Collections.Generic;
using TMPro;
using Algorand;
using Algorand.Algod;
using Algorand.Algod.Model;
using Algorand.Algod.Model.Transactions;
using Algorand.Utils;
using System;
using UnityEngine.Events;

public class Manager : MonoBehaviour
{
    [SerializeField] GameObject accountItemPrefab;
    [SerializeField] Transform accountItemParent;
    [SerializeField] TransactionWindow transactionWindow;

    [SerializeField] string ALGOD_API_ADDR = "http://localhost:4001/";
    [SerializeField] string ALGOD_API_TOKEN = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";

    DefaultApi algodApiInstance;

    public static List<AccountItem> accountItems = new List<AccountItem>();

    public UnityEvent OnAccountGenerated = new UnityEvent();

    private async void Start()
    {   
        var httpClient = HttpClientConfigurator.ConfigureHttpClient(ALGOD_API_ADDR, ALGOD_API_TOKEN);
        algodApiInstance = new DefaultApi(httpClient);

        try
        {
            var supply = await algodApiInstance.GetSupplyAsync();
            Debug.Log("Total Algorand Supply: " + supply.TotalMoney);
            Debug.Log("Online Algorand Supply: " + supply.OnlineMoney);
        }
        catch (ApiException<ErrorResponse> e)
        {
            Debug.Log("Exception when calling algod#getSupply: " + e.Result.Message);
        }

        transactionWindow.SetButtonStatusToLoading(false);
    }

    public async void ProcessTransaction()
    {
        transactionWindow.SetButtonStatusToLoading(true);
        transactionWindow.DisplayError(string.Empty);

        AccountItem sourceItem = null;
        AccountItem destItem = null;
        
        try
        {
            sourceItem = accountItems[transactionWindow.from.value];
            destItem = accountItems[transactionWindow.to.value];
        }
        catch (Exception e)
        {
            transactionWindow.SetButtonStatusToLoading(false);
            transactionWindow.DisplayError("Please create an account before attempting to make a transaction");
            return;
        }

        ulong amount = 0;
        try
        {
            amount = Convert.ToUInt64(transactionWindow.amount.text);
        }
        catch(FormatException e)
        {
            transactionWindow.DisplayError("Amount is not a integer");
            transactionWindow.SetButtonStatusToLoading(false);
            return;
        }
        catch (OverflowException e)
        {
            transactionWindow.DisplayError("Amount is not valid (too large or negative)");
            transactionWindow.SetButtonStatusToLoading(false);
            return;
        }

        if (amount == 0)
        {
            transactionWindow.DisplayError("Amount is not a valid");
            transactionWindow.SetButtonStatusToLoading(false);
            return;
        }

        var sourceBalance = await sourceItem.GetBalance();
        if (sourceBalance < amount)
        {
            transactionWindow.DisplayError("Insufficient funds for transaction");
            transactionWindow.SetButtonStatusToLoading(false);
            return;
        }

        MakeTransaction(sourceItem.account, destItem.account, amount);
    }

    public async void MakeTransaction(Account src, Account dest, ulong amount)
    {
        TransactionParametersResponse transParams = null;
        try
        {
            transParams = await algodApiInstance.TransactionParamsAsync();
        }
        catch (ApiException<ErrorResponse> e)
        {
            Debug.Log("Exception when calling algod#getSupply: " + e.Result.Message);
            transactionWindow.DisplayError(e.Result.Message);
            transactionWindow.SetButtonStatusToLoading(false);
        }

        var tx = PaymentTransaction.GetPaymentTransactionFromNetworkTransactionParameters(src.Address, dest.Address, amount, "pay message", transParams);
        var signedTx = tx.Sign(src);

        Debug.Log("Signed transaction with txid: " + signedTx.Tx.TxID());

        try
        {
            var id = await Utils.SubmitTransaction(algodApiInstance, signedTx);
            Debug.Log("Successfully sent tx with id: " + id.Txid);

            var resp = await Utils.WaitTransactionToComplete(algodApiInstance, id.Txid);

            Debug.Log("Confirmed Round is: " + resp.ConfirmedRound);
        }
        catch (ApiException<ErrorResponse> e)
        {
            Debug.Log("Exception when calling algod#rawTransaction: " + e.Result.Message);
            transactionWindow.DisplayError(e.Result.Message);
            transactionWindow.SetButtonStatusToLoading(false);
        }

        transactionWindow.SetButtonStatusToLoading(false);
        RefreshAccountItems();
    }

    public void GenerateAccount()
    {   
        var accountItem = Instantiate(accountItemPrefab, accountItemParent).GetComponent<AccountItem>();
        accountItem.account = new Account();
        accountItem.Initialize(algodApiInstance);

        accountItems.Add(accountItem);
        OnAccountGenerated?.Invoke();
    }

    public void GenerateAccount(string mnemonic)
    {
        var accountItem = Instantiate(accountItemPrefab, accountItemParent).GetComponent<AccountItem>();
        try
        {
            accountItem.account = new Account(mnemonic);
        } 
        catch (Exception e)
        {
            Debug.Log(e.Message);
            Destroy(accountItem.gameObject);
            return;
        }

        accountItem.Initialize(algodApiInstance);
        accountItems.Add(accountItem);
        OnAccountGenerated.Invoke();
    }

    public void GenerateAccount(TMP_InputField inputField)
    {
        GenerateAccount(inputField.text);
    }

    public void RefreshAccountItems()
    {
        foreach (var account in accountItems)
        {
            account.UpdateBalance();
        }
    }
}
