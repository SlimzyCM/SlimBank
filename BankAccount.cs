using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SlimBank
{   
    public enum AccountType
    {
        Savings,
        Current
    }
    public class BankAccount
    {
        public Customer AccountOwner { get; }

        public AccountType TypeOfAcc { get; }

        public string AccountNumber { get; }
        private static int accountNumberSeed = 1234567890;

        public DateTime DateCreated { get; }

        public decimal Balance 
        {
            get
            {
                return allTransactions.Sum(x => x.Amount);
            }
        }

        public List<Transaction> allTransactions = new List<Transaction>();

        public BankAccount(Customer accountOwner, AccountType type, decimal initialBalance)
        {
            if (type == AccountType.Savings)
            {
                if (initialBalance >= 100) type = AccountType.Savings;
                else throw new InvalidOperationException("Initial deposit for a Savings account must be 100 and above.");
            }
            else if (type == AccountType.Current)
            {
                if (initialBalance >= 1000) type = AccountType.Current;
                else throw new InvalidOperationException("Initial deposit for a Current account must be 1000 and above.");
            }
            else throw new InvalidDataException("You can only create a Current or Savings accoount!!!");


            AccountOwner = accountOwner;
            TypeOfAcc = type;
            DateCreated = DateTime.Now;
            MakeDeposit(initialBalance, DateTime.Now, "Initial Deposit Amount");

            this.AccountNumber = accountNumberSeed.ToString();
            accountNumberSeed++;

            AccountOwner.AddAcount(this);

        }


        public void MakeDeposit(decimal amount, DateTime date, string note)
        {
            if (!AccountOwner.LoggedIn) throw new InvalidOperationException("Please Log In to Continue");

            if (amount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount), "Amount of deposit must be positive");
            }
            var deposit = new Transaction(AccountOwner, AccountNumber, TypeOfAcc, Balance, Balance + amount, amount, date, note);
            allTransactions.Add(deposit);
        }

        public void MakeWithdrawal(decimal amount, DateTime date, string note)
        {

            if (!AccountOwner.LoggedIn) throw new InvalidOperationException("Please Log In to Continue");

            if (amount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount), "Amount of withdrawal must be positive");
            }

            if (TypeOfAcc == AccountType.Savings && Balance - amount < 100)
            {
                throw new InvalidOperationException("Insufficient funds for this Transaction");
            }

            if (Balance - amount < 0)
            {
                throw new InvalidOperationException("Insufficient funds for this Transaction");
            }
            var withdrawal = new Transaction(AccountOwner, AccountNumber, TypeOfAcc, Balance, Balance - amount, -amount, date, note);
            allTransactions.Add(withdrawal);
        }


        public void MakeTransfer(BankAccount reciever, decimal amount, DateTime date, string note)
        {

            if (!AccountOwner.LoggedIn) throw new InvalidOperationException("Please Log In to Continue");

            if (AccountNumber == reciever.AccountNumber)
            {
                throw new InvalidOperationException("You can not transfer to the same account");
            }

            reciever.MakeDeposit(amount, date, note);
            this.MakeWithdrawal(amount, date, note);
           
        }



        public string GetAccountHistory()
        {
            var report = new System.Text.StringBuilder();

            decimal balance = 0;

            // Header for the transaction history
            report.AppendLine("Date\t\tAmount\tBalance\tNote");
            foreach (var item in allTransactions)
            {
                balance += item.Amount;

                //Each rows of transaction in the transction list
                report.AppendLine($"{item.Date.ToShortDateString()}\t{item.Amount}\t{balance}\t{item.Notes}");
            }

            return report.ToString();
        }

    }
}
