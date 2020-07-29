using System;
using System.Collections.Generic;
using System.Text;

namespace SlimBank
{
    public class Transaction
    {

        public Customer AccountOwner { get; }
        public string AccountNumber { get; }
        public AccountType TypeOfAcc { get; }
        public decimal BalanceBefore { get; }
        public decimal BalanceAfter { get; }
        public decimal Amount { get; }
        public DateTime Date { get; }
        public string Notes { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="owner"> the customers </param>
        /// <param name="accountNumber"> the account number performing transction</param>
        /// <param name="type"> The account type</param>
        /// <param name="balance"> return the account balance</param>
        /// <param name="amount"> the amount of the transaction</param>
        /// <param name="date"> time of transaction</param>
        /// <param name="note"> transaction note</param>

        public Transaction( Customer owner, string accountNumber, AccountType type, decimal balance, decimal balanceAfter, decimal amount, DateTime date, string note)
        {
            AccountOwner = owner;
            AccountNumber = accountNumber;
            TypeOfAcc = type;
            BalanceBefore = balance;
            BalanceAfter = balanceAfter;
            Amount = amount;
            Date = date;
            Notes = note;
        }
    }
}
