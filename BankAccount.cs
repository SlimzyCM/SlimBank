using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace SlimBank
{   
    public enum AccountType
    {
        Savings,
        Current
    }
    public class BankAccount
    {
        /// <summary>
        /// class containing the account details
        /// 
        /// </summary>
        public Customer AccountOwner { get; }
        //property that indicates the customer that owns the account

        public AccountType TypeOfAcc { get; }
        //customer account type
        
        //the account number
        public string AccountNumber { get; }

        // combine numbers to create an account format and then auto increment
        private static int accountNumberSeed = 1234567890;

        // account creation time
        public DateTime DateCreated { get; }

        //balance
        public decimal Balance 
        {
            get
            {
                return allTransactions.Sum(x => x.Amount);
            }
        }

        //list of type transaction to store all transactions
        public List<Transaction> allTransactions = new List<Transaction>();

        // the construtor to create the account
        public BankAccount(Customer accountOwner, AccountType type, decimal initialBalance)
        {

            AccountOwner = accountOwner;
            TypeOfAcc = type;
            DateCreated = DateTime.Now;
            MakeDeposit(initialBalance, DateTime.Now, "Initial Deposit Amount");

            //Convert to string
            this.AccountNumber = accountNumberSeed.ToString();
            accountNumberSeed++;

            // add the created account by calling the customer owned account constructor
            AccountOwner.AddAcount(this);

            // add all account to all customer account list
            Bank.allCustomersBankAccount.Add(this);


        }

        // the method for making deposit
        public void MakeDeposit(decimal amount, DateTime date, string note)
        {

            if (amount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount), "Amount of deposit must be positive");
            }
            var deposit = new Transaction(AccountOwner, AccountNumber, TypeOfAcc, Balance, Balance + amount, amount, date, note);
            allTransactions.Add(deposit);
        }

        // the withdrawal method

        public void MakeWithdrawal(decimal amount, DateTime date, string note)
        {


            if (amount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount), "Amount of withdrawal must be positive");
            }

            var withdrawal = new Transaction(AccountOwner, AccountNumber, TypeOfAcc, Balance, Balance - amount, -amount, date, note);
            allTransactions.Add(withdrawal);
        }

        // transfer method
        public void MakeTransfer(BankAccount reciever, decimal amount, DateTime date, string note)
        {

            // call the deposit method for the reciever
            reciever.MakeDeposit(amount, date, note);

            // same call the withdrawal method for the sender
            this.MakeWithdrawal(amount, date, note);
           
        }


        // used to get the statement of account
        public string GetAccountHistory()
        {
            var report = new System.Text.StringBuilder();

            decimal balance = 0;



            report.AppendLine("--------------------------------------------------------");
            
            // Header for the transaction history
             report.AppendLine("Date\t\tAmount\tBalance\t\tNote");
            report.AppendLine("--------------------------------------------------------");

            foreach (var item in allTransactions)
            {
                balance += item.Amount;

                //Each rows of transaction in the transction list
                report.AppendLine($"\n{item.Date.ToShortDateString()}\t{item.Amount}\t{balance}\t{item.Notes}");
            }
            report.AppendLine("--------------------------------------------------------");

            return report.ToString();
        }

    }
}
