using System;
using System.Collections.Generic;

namespace classes
{
  class Program
  {
    static void Main(string[] args)
    {
      var account = new BankAccount("Test Account", 50);
      Console.WriteLine($"Account {account.Number} was created for {account.Owner} with {account.Balance} initial balance.");
      Console.WriteLine(account.GetAccountHistory());
      
      // account.MakeWithdrawal(500, DateTime.Now, "Rent payment");
      // Console.WriteLine(account.Balance);
      // account.MakeDeposit(100, DateTime.Now, "Friend paid me back");
      // Console.WriteLine(account.Balance);

      // // Test that the initial balances must be positive.
      // try
      // {
      //   var invalidAccount = new BankAccount("invalid", -55);
      // }
      // catch (ArgumentOutOfRangeException e)
      // {
      //   Console.WriteLine("Exception caught creating account with negative balance");
      //   Console.WriteLine(e.ToString());
      // }

      // // Test for a negative balance.
      // try
      // {
      //   account.MakeWithdrawal(750, DateTime.Now, "Attempt to overdraw");
      // }
      // catch (InvalidOperationException e)
      // {
      //   Console.WriteLine("Exception caught trying to overdraw");
      //   Console.WriteLine(e.ToString());
      // }
    }
  }

  public class BankAccount
  {
    private static int accountNumberSeed = 1234567890;
    public string Number { get; }
    public string Owner { get; set; }
    public decimal Balance {
      get
      {
        decimal balance = 0;
        foreach (var transaction in allTransactions)
        {
          balance  += transaction.Amount;
        }

        return balance;
      }
    }

    public string GetAccountHistory()
    {
      var report = new System.Text.StringBuilder();

      decimal balance = 0;
      report.AppendLine("Date\t\tAmount\tBalance\tNote");
      foreach (var item in allTransactions)
      {
        balance += item.Amount;
        report.AppendLine($"{item.Date.ToShortDateString()}\t{item.Amount}\t{balance}\t{item.Notes}");
      }

      return report.ToString();
    }

    private List<Transaction> allTransactions = new List<Transaction>();

    public BankAccount(string name, decimal initialBalance)
    {
      this.Owner = name;
      this.Number = accountNumberSeed.ToString();
      accountNumberSeed++;

      // set initialBalance using 'real' transaction rather than directly setting its value
      MakeDeposit(initialBalance, DateTime.Now, "Initial balance");

    }

    public void MakeDeposit(decimal amount, DateTime date, string note)
    {
      if (amount <= 0)
      {
        throw new ArgumentOutOfRangeException(nameof(amount), "Amount of deposit must be positive");
      }
      var deposit = new Transaction(amount, date, note);
      allTransactions.Add(deposit);
    }

    public void MakeWithdrawal(decimal amount, DateTime date, string note)
    {
      if (amount <= 0)
      {
        throw new ArgumentOutOfRangeException(nameof(amount), "Amount of withdrawal must be positive");
      }
      if (Balance - amount < 0)
      {
        throw new InvalidOperationException("Not sufficient funds for this withdrawal");
      }
      var withdrawal = new Transaction(-amount, date, note);
      allTransactions.Add(withdrawal);
    }
  }
}
