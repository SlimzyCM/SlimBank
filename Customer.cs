using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;
using System.Transactions;

namespace SlimBank
{
    public class Customer
    {
        
        public int CustomerId { get; private set; }

        private string FirstName { get; set; }

        private string LastName { get; set; }

        public string FullName
        {
            get
            {
                return LastName + " " + FirstName;
            }

        }
        public string Email { get; }
        public string Username { get; }
        public string Password { get; }

        public bool LoggedIn { get; private set; }

        private static int seedId = 1;

        private readonly List<BankAccount> customerAccount = new List<BankAccount>();



        public Customer(string firstName, string lastName, string email, string username, string password)
        {
            CustomerId = seedId++;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Username = username;
            Password = password;

            Bank.allCustomers.Add(this);
        }

        public void AddAcount(BankAccount account)
        {
            customerAccount.Add(account);
        }

        public void LoggedInCheck (string username, string password)
        {
            if (username == Username && password == Password) LoggedIn = true;
            else Console.WriteLine("Enter the correct username or password");
        }

        public void LoggedOut()
        {
            LoggedIn = false;
        }

    }
}
