using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Transactions;

namespace SlimBank
{
    class Program
    {
        static void Main(string[] args)
        {
            // start the program by call the hompage method
            HomePage();
        }

        /// <summary>
        /// HopePage is the Default method
        /// </summary>
        public static void HomePage()
        {
            Console.Clear(); // clear the Console
            Console.WriteLine("=====================================");

            Console.ForegroundColor = ConsoleColor.Yellow; // set the text color to yellow
            Console.WriteLine("\n\tWelcome to SlimBank\n  ");
            Console.ResetColor(); // Reset the console text color to default

            Console.WriteLine("=====================================");
            

            //homepage menu 
            Console.ForegroundColor = ConsoleColor.White; // set the text color to white
            Console.WriteLine("\n1: New Customer Sign Up\n");
            Console.WriteLine("2: Existing Customers Log In\n");
            Console.WriteLine("-------------------------------------");
            Console.ResetColor();
           
            Console.WriteLine("Note: Use left Numbering for Navigation..\n\n"); //instruction for the program use

            Console.ForegroundColor = ConsoleColor.White; // set the text color to white
            Console.Write("Enter your option:");
            Console.ResetColor();
               
            // Read User input
            string command = Console.ReadLine();

            //check the input and perform based on it
            if (command == "1")
            {
                CustomerSignUp(); 
            }
            else if (command == "2")
            {
                CustomerLogIn();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red; // set the text color to red
                Console.WriteLine($"\n\n\tInvalid option.. \n");
                Console.ResetColor(); // Reset the console text color to default

                Console.ReadLine();

                HomePage();

            }

        }

        /// <summary>
        /// Customer
        /// </summary>
        public static void CustomerSignUp()
        {
            Console.Clear();
            Console.WriteLine("----------------------------------");

            Console.ForegroundColor = ConsoleColor.Cyan; // set the text color to yellow
            Console.WriteLine("   --- Create new Customer ---  ");
            Console.ResetColor(); // Reset the console text color to default

            Console.WriteLine("----------------------------------");

            Console.ForegroundColor = ConsoleColor.White; // set the text color to white

            Console.Write("\nEnter Username: ");// sign up entry details
            string userName = Console.ReadLine();

            Console.Write("\nEnter first name: ");
            string firstName = Console.ReadLine();

            Console.Write("\nEnter last name: ");
            string lastName = Console.ReadLine();

            Console.Write("\nEnter email Address: ");
            string eMail = Console.ReadLine();

            Console.Write("\nEnter password: ");
            string password = Console.ReadLine();
            Console.ResetColor(); // Reset the console text color to default


            //Create a Customer object
            Customer customer = new Customer(firstName, lastName, eMail, userName, password);

            Console.Clear();

            Console.ForegroundColor = ConsoleColor.Yellow; // set the text color to yellow
            Console.WriteLine($"\n\n{customer.FullName}, You have successfully registered... \n");
            Console.ResetColor(); // Reset the console text color to default


            Console.Write("\nPress Any key to LogIn");
            Console.ReadLine();

            //call the customerLogin method
            CustomerLogIn();

        }



        public static void CustomerLogIn()
        {
            Console.Clear();
            Console.WriteLine("------------------------------------------------------");

            Console.ForegroundColor = ConsoleColor.Yellow; // set the text color to yellow
            Console.WriteLine("  \t\t --- Log In  ---  ");
            Console.ResetColor(); // Reset the console text color to default

            Console.WriteLine("------------------------------------------------------");

            //collect username
            Console.Write("\nEnter username: ");
            string userName = Console.ReadLine();
            //collect password
            Console.Write("\nEnter password: ");
            string password = Console.ReadLine();

            //create customer variable assigned to null
            Customer myCustomer = null;
            foreach (Customer item in Bank.allCustomers)// loop through all the customer list and find a match
            {
                if (item.Username == userName && item.Password == password)
                {
                    // assign match to the customer variable
                    myCustomer = item;
                    break;
                }
            }
            //check if found
            if (myCustomer != null)
            {
                myCustomer.LoggedInCheck(userName, password);
                Profile(myCustomer.CustomerId, myCustomer.FullName);

            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red; // set the text color to yellow
                Console.WriteLine($"\n\nIncorrect Username or Password.. \n");
                Console.ResetColor(); // Reset the console text color to default

                Console.WriteLine("\nPress * to Sign Up  || Press Any other key to try again"); //navigate back using *
                string command = Console.ReadLine();

                if (command == "*") CustomerSignUp();
                else  CustomerLogIn();
            } 
        }



        public static void Profile(int customerId, string name)
        {
            profileLabel: Console.Clear(); // clear the Console
            Console.WriteLine("---------------------------------------");

            Console.ForegroundColor = ConsoleColor.Cyan; // set the text color to cyan
            Console.WriteLine($"\t      Welcome {name}  ");
            Console.ResetColor(); // Reset the console text color to default

            Console.WriteLine("---------------------------------------");

            Console.ForegroundColor = ConsoleColor.White; // set the text color to white
            Console.WriteLine("\n1: Create New Account\n"); // menu for selection
            Console.WriteLine("2: Perform Transaction on Existing Account\n");
            Console.WriteLine("3: Log Out \n");

            Console.WriteLine("---------------------------------------");
            Console.ResetColor();

            Console.WriteLine("Note: Use left Numbering for Navigation..\n\n"); //instruction for the program use


            string command = Console.ReadLine();

            // 1 => create account functionality
            if (command == "1") selectAccountOperation(customerId, name);

            // 2 => performTransaction functionality
            else if (command == "2")
            {
                PerformTransaction(customerId, name);
            }
            else if (command == "3")
            {
                // loop through the all customer list and find the matching Id
                Bank.allCustomers.ForEach(item => { if (item.CustomerId == customerId) item.LoggedOut(); });
                HomePage();

            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red; // set the text color to red
                Console.WriteLine($"\nInvalid option.. \n");

                Console.ResetColor(); // Reset the console text color to default
                Console.WriteLine($"\nPress any key to try again.. ");

                Console.ReadLine();
                goto profileLabel;

            }


        }


        public static void selectAccountOperation(int customerId, string name)
        {
            //selectAccount - label to repeat process if invalid key selected
            selectAccount: Console.Clear();
            Console.WriteLine("--------------------------------------------------------");
            Console.WriteLine("\t\tSelect Account Type");
            Console.WriteLine("--------------------------------------------------------");
            Console.WriteLine("1: Current | Requires initial deposit of 1000 and above.\n");
            Console.WriteLine("2: Savings | Requires initial deposit of 100 and above.\n");

            Console.Write("\nEnter your option:");

            string selected = Console.ReadLine();

            if (selected == "1")
            {

                Console.ForegroundColor = ConsoleColor.Yellow; // set the text color to yellow

                // Enter initial deposit value
                enterAmount: Console.WriteLine("\n\nEnter Initial deposit Amount");
                Console.ResetColor(); // Reset the console text color to default

                string depositRead = Console.ReadLine(); // store the input value in depositRead

                //convert the string value to integer 
                bool response = decimal.TryParse(depositRead, out decimal amount); // .TryParse return boolean value

                if (response)
                {
                    if(amount < 1000)
                    {
                        Console.ForegroundColor = ConsoleColor.Red; // set the text color to red
                        Console.WriteLine($"\n\nInitial deposit for a Current account must be 1000 and above.. \n"); // error message for less than required amount

                        Console.ResetColor(); // Reset the console text color to default
                        
                        Console.ReadLine();
                        Console.Clear();
                        goto enterAmount;

                    }
                    else CreateAccount(AccountType.Current, amount, customerId);

                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red; // set the text color to red
                    Console.WriteLine($"\n\nInvalid amount, Enter only Digits.. \n"); // error message for non digit

                    Console.ResetColor(); // Reset the console text color to default
                    Console.WriteLine("\nPress * to select Account type || Press Any other key to try again");

                    // store the user input
                    string repeatEnterAmount = Console.ReadLine();

                    //clear the console exsiting text
                    Console.Clear();

                    //condition to redirect based on the key selected
                    if (repeatEnterAmount == "*") goto selectAccount;
                    else goto enterAmount;


                }


            }
            else if (selected == "2")
            {
                Console.ForegroundColor = ConsoleColor.Yellow; // set the text color to yellow

                // Enter initial deposit value
                enterAmount: Console.WriteLine("\n\nEnter Initial deposit Amount");
                Console.ResetColor(); // Reset the console text color to default

                string depositRead = Console.ReadLine(); // store the input value in depositRead

                //convert the string value to integer 
                bool response = decimal.TryParse(depositRead, out decimal amount); // .TryParse return boolean value

                if (response)
                {
                    if (amount < 100)
                    {
                        Console.ForegroundColor = ConsoleColor.Red; // set the text color to red
                        Console.WriteLine($"\n\nInitial deposit for a Current account must be 100 and above.. \n"); // error message for less than required amount

                        Console.ResetColor(); // Reset the console text color to default

                        Console.ReadLine();
                        Console.Clear();
                        goto enterAmount;

                    }
                    else CreateAccount(AccountType.Savings, amount, customerId);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red; // set the text color to red
                    Console.WriteLine($"\n\nInvalid amount, Enter only Digits.. \n"); // error message for non digit

                    Console.ResetColor(); // Reset the console text color to default
                    Console.WriteLine("\nPress * to select Account type || Press Any other key to try again");

                    // store the user input
                    string repeatEnterAmount = Console.ReadLine();

                    //clear the console exsiting text
                    Console.Clear();

                    //condition to redirect based on the key selected
                    if (repeatEnterAmount == "*") goto selectAccount;
                    else goto enterAmount;


                }
            }

            else
            {
                Console.ForegroundColor = ConsoleColor.Red; // set the text color to red
                Console.WriteLine($"\nInvalid option.. \n");

                Console.ResetColor(); // Reset the console text color to default
                Console.WriteLine($"\nPress any key to try again.. ");

                Console.ReadLine();

                // return back to labelled point
                goto selectAccount;

            }
        
        }



        public static void CreateAccount(AccountType type, decimal amount, int customerId)
        {

          
            Customer myCustomer = null;

            foreach (Customer item in Bank.allCustomers)
            {
                if (item.CustomerId == customerId)
                {
                    myCustomer = item;
                    break;
                }
            }


            BankAccount createAccount = new BankAccount(myCustomer, type, amount);

            Console.ForegroundColor = ConsoleColor.Yellow; // set the text color to red
            Console.WriteLine($"\n\nA {createAccount.TypeOfAcc} account {createAccount.AccountNumber} was created with N{createAccount.Balance} initial balance...\n");

            Console.ResetColor(); // Reset the console text color to default

            Console.ReadLine();

            Profile(myCustomer.CustomerId, myCustomer.FullName);
            

        }


        public static void PerformTransaction(int customerId, string name)
        {
            Console.Clear();
            Console.WriteLine("--------------------------------------------------------");
            Console.WriteLine("\t\tPerform Transaction");
            Console.WriteLine("--------------------------------------------------------");
            Console.WriteLine("1: Cash Deposit  \n");
            Console.WriteLine("2: Withdrawal  \n");
            Console.WriteLine("3: Transfer Fund  \n");
            Console.WriteLine("4: Balance Inquiry  \n");
            Console.WriteLine("5: Statement Of Account  \n\n");
            Console.WriteLine("Press * to Return to Main menu  \n");

            Console.WriteLine("--------------------------------------------------------");
            Console.ResetColor();

            Console.WriteLine("Note: Use left Numbering for Navigation...\n\n"); //instruction for the program use
            
            Console.Write("\nEnter your option:");

            string command = Console.ReadLine();
            Customer myCustomer = null;
            foreach (Customer item in Bank.allCustomers)
            {
                if (item.CustomerId == customerId && item.FullName == name)
                {
                    myCustomer = item;
                    break;
                }
            }
            
            // perform particular transaction selected using switch
            switch (command)
            {
                case "1":
                    // call the deposit method with the customer class
                    Deposit(myCustomer);
                    break;
                case "2":
                    // call the withdraw method with the customer class
                    Withdrawal(myCustomer);
                    break;
                case "3":
                    //call the transfer method with the customer class
                    Transfer(myCustomer);
                    break;
                case "4":
                    //call the checkbalance method with the customer class
                    CheckBalance(myCustomer);
                    break;
                case "5":
                    //call the statement of account method with the customer class
                    StatementOfAccount(myCustomer);
                    break;
                case "*":
                    // * to call the profile and go back
                    Profile(customerId, name);
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Red; // set the text color to red
                    Console.WriteLine($"\nInvalid option.. \n");

                    Console.ResetColor(); // Reset the console text color to default
                    Console.WriteLine($"\nPress any key to try again.. ");

                    Console.ReadLine();
                    //make a call to PerformTransaction with Id and name
                    PerformTransaction(customerId, name);
                    break;
            }





        }


        public static void Deposit(Customer customer)
        {
            var accountList = new List<BankAccount>();
            accountList = customer.customerAccountList();

            if (accountList.Count < 1)
            {
                Console.ForegroundColor = ConsoleColor.Red; // set the text color to yellow
                Console.WriteLine($"\n\nYou have not opened any account... \n");
                Console.ResetColor(); // Reset the console text color to default
                Console.ReadKey();
                Profile(customer.CustomerId, customer.FullName);

            }
            else
            {
                // a label for restore point
                depositLabel:  Console.Clear(); 
                Console.WriteLine("--------------------------------------------------------");
                Console.WriteLine("\t       Please select Deposit Account:");
                Console.WriteLine("--------------------------------------------------------");

                //loops through the list of bank account ownered by this customer
                int count = 1;
                foreach (var item in accountList)
                {
                    //Populate the console with the option of accounts available
                    Console.WriteLine($"\n{count}:\t{item.AccountNumber}\t|\t{item.TypeOfAcc} \n");
                    count++;
                }
                Console.WriteLine("\n\nPress * to Return to Main menu  \n");

                Console.WriteLine("--------------------------------------------------------");


                Console.Write("\nEnter your option:");
                string selected = Console.ReadLine();

                //chek that key entered is not * else call PerformTransaction 
                if (selected == "*") PerformTransaction(customer.CustomerId, customer.FullName);

                //convert the string value to integer 
                bool response = int.TryParse(selected, out int index ); // .TryParse return boolean value

                if (response && index < count && index > 0)
                {
                    // selected the actual account to be used 
                    var TransactionAccount = accountList[index - 1]; // "index - 1" list is Zero based index

                    depositChecklabel:  Console.Write("\nEnter the Amount you want to deposit: "); // check another label
                    string depositAmount = Console.ReadLine();

                    Console.Write("\nEnter Note: ");
                    string depositNote = Console.ReadLine(); //collect deposite note

                    // call the amountchecker method to parse and return amount if valid and zero if invalid
                    var parseCheck = amountChecker(depositAmount); 
                    if (parseCheck > 0)
                    {
                        // call the makedeposit method in the BankAccount class
                        TransactionAccount.MakeDeposit(parseCheck, DateTime.Now, depositNote);
                        Console.ForegroundColor = ConsoleColor.Yellow; // set the text color to yellow
                        Console.WriteLine($"\n\nTransaction successful... \n");
                        Console.ResetColor(); // Reset the console text color to default

                        Console.ReadLine();
                        PerformTransaction(customer.CustomerId, customer.FullName);
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red; // set the text color to red
                        Console.WriteLine($"\nInvalid Amount Entry. \n");

                        Console.ResetColor(); // Reset the console text color to default
                        Console.WriteLine($"\nPlease try again... ");

                        Console.ReadLine();
                        Console.Clear();
                        goto depositChecklabel;
                    }


                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red; // set the text color to red
                    Console.WriteLine($"\nInvalid option.. \n");

                    Console.ResetColor(); // Reset the console text color to default
                    Console.WriteLine($"\nPlease try again.. ");

                    Console.ReadLine();
                    // return to the accountPick point
                    goto depositLabel;
                }
                        
       

            }



        }

        public static void Withdrawal(Customer customer)
        {
            var accountList = new List<BankAccount>();
            accountList = customer.customerAccountList();

            if (accountList.Count < 1)
            {
                Console.ForegroundColor = ConsoleColor.Red; // set the text color to yellow
                Console.WriteLine($"\n\nYou have not opened any account... \n");
                Console.ResetColor(); // Reset the console text color to default
                Console.ReadKey();
                Profile(customer.CustomerId, customer.FullName);

            }
            else
            {
                // a label for restore point
                withdrawalLabel: Console.Clear();
                Console.WriteLine("--------------------------------------------------------");
                Console.WriteLine("\t       Please select Withdrawal Account:");
                Console.WriteLine("--------------------------------------------------------");

                //loops through the list of bank account ownered by this customer
                int count = 1;
                foreach (var item in accountList)
                {
                    //Populate the console with the option of accounts available
                    Console.WriteLine($"\n{count}:\t{item.AccountNumber}\t|\t{item.TypeOfAcc} \n");
                    count++;
                }

                Console.WriteLine("\n\nPress * to Return to Main menu  \n");

                Console.WriteLine("--------------------------------------------------------");


                Console.Write("\nEnter your option:");
                string selected = Console.ReadLine();

                if (selected == "*") PerformTransaction(customer.CustomerId, customer.FullName);

                //convert the string value to integer 
                bool response = int.TryParse(selected, out int index); // .TryParse return boolean value

                if (response && index < count && index > 0)
                {
                    // selected the actual account to be used 
                    var TransactionAccount = accountList[index - 1]; // "index - 1" list is Zero based index

                    withdrawalChecklabel: Console.Write("\nEnter the Amount you want to withdraw: "); // check another label
                    string withdrawAmount = Console.ReadLine();

                    Console.Write("\nEnter Note: ");
                    string withdrawNote = Console.ReadLine(); //collect withdrawal note

                    // call the amountchecker method to parse and return withdrawAmount if valid and zero if invalid
                    var parseCheck = amountChecker(withdrawAmount);

                    //parsecheck now contains the .tryParse return of withdrawAmount 
                    if (parseCheck > 0)
                    {
                        //check that the account type is savings
                        // check that the account balance will remain 100 after transaction
                        if (TransactionAccount.TypeOfAcc == AccountType.Savings && TransactionAccount.Balance - parseCheck < 100)
                        {
                            Console.ForegroundColor = ConsoleColor.Red; // set the text color to red
                            Console.WriteLine($"\nInsufficient funds for this transaction. \n");

                            Console.ResetColor(); // Reset the console text color to default

                            Console.ReadLine();
                            Console.Clear();
                            goto withdrawalLabel;
                        }
                        else 
                        { 
                            TransactionAccount.MakeWithdrawal(parseCheck, DateTime.Now, withdrawNote);
                            Console.ForegroundColor = ConsoleColor.Yellow; // set the text color to yellow
                            Console.WriteLine($"\n\nTransaction successful... \n");
                            Console.ResetColor(); // Reset the console text color to default

                            Console.ReadLine();
                            PerformTransaction(customer.CustomerId, customer.FullName);

                        }
                        

                        if (TransactionAccount.TypeOfAcc == AccountType.Current && TransactionAccount.Balance - parseCheck < 0)
                        {
                            Console.ForegroundColor = ConsoleColor.Red; // set the text color to red
                            Console.WriteLine($"\nInsufficient funds for this transaction. \n");

                            Console.ResetColor(); // Reset the console text color to default

                            Console.ReadLine();
                            Console.Clear();
                            goto withdrawalLabel;
                        }
                        else

                        {
                            TransactionAccount.MakeWithdrawal(parseCheck, DateTime.Now, withdrawNote);
                            Console.ForegroundColor = ConsoleColor.Yellow; // set the text color to yellow
                            Console.WriteLine($"\n\nTransaction successful... \n");
                            Console.ResetColor(); // Reset the console text color to default

                            Console.ReadLine();
                            PerformTransaction(customer.CustomerId, customer.FullName);
                        }
                            
                        

                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red; // set the text color to red
                        Console.WriteLine($"\nInvalid Amount Entry. \n");

                        Console.ResetColor(); // Reset the console text color to default
                        Console.WriteLine($"\nPlease try again... ");

                        Console.ReadLine();
                        Console.Clear();
                        goto withdrawalChecklabel;
                    }


                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red; // set the text color to red
                    Console.WriteLine($"\nInvalid option.. \n");

                    Console.ResetColor(); // Reset the console text color to default
                    Console.WriteLine($"\nPlease try again.. ");

                    Console.ReadLine();
                    // return to the accountPick point
                    goto withdrawalLabel;
                }



            }



        }


        public static void Transfer(Customer customer)
        {
            var accountList = new List<BankAccount>();
            accountList = customer.customerAccountList();

            if (accountList.Count < 1)
            {
                Console.ForegroundColor = ConsoleColor.Red; // set the text color to yellow
                Console.WriteLine($"\n\nYou have not opened any account... \n");
                Console.ResetColor(); // Reset the console text color to default
                Console.ReadKey();
                Profile(customer.CustomerId, customer.FullName);

            }
            else
            {
                transferLabel: Console.Clear();
                Console.WriteLine("--------------------------------------------------------");
                Console.WriteLine("\t       Please select funds transfer type:");
                Console.WriteLine("--------------------------------------------------------");

                Console.WriteLine("1: To My Account  \n"); // customer transfer between owned accounts
                Console.WriteLine("2: To Other Customer  \n\n"); // transfer between customers
                Console.WriteLine("\nPress * to Return to Main menu  \n");

                Console.WriteLine("--------------------------------------------------------");
                Console.ResetColor();

                Console.WriteLine("Note: Use left Numbering for Navigation...\n\n"); //instruction for the program use

                Console.Write("\nEnter your option:");

                string command = Console.ReadLine();

                if (command == "*") PerformTransaction(customer.CustomerId, customer.FullName);


                switch (command)
                {
                    case "1":
                        if (accountList.Count > 1)
                        {
                            transferCheckLabel: Console.Clear();
                            Console.WriteLine("--------------------------------------------------------");
                            Console.WriteLine("\t       Please select a payment source:");
                            Console.WriteLine("--------------------------------------------------------");

                            //loops through the list of bank account ownered by this customer
                            int count = 1;
                            foreach (var item in accountList)
                            {
                                //Populate the console with the option of accounts available
                                Console.WriteLine($"\n{count}:\t{item.AccountNumber}\t|\t{item.TypeOfAcc} \n");
                                count++;
                            }

                            Console.WriteLine("\n\nPress * to Return to Main menu  \n");

                            Console.WriteLine("--------------------------------------------------------");


                            Console.Write("\nEnter your option:");
                            string selected = Console.ReadLine();

                            //option entered check
                            if (selected == "*") goto transferLabel;

                            //convert the string value to integer 
                            bool response = int.TryParse(selected, out int index); // .TryParse return boolean value

                            if (response && index < count && index > 0)//check that the option selected is in the list of account
                            {
                                // pick the payment source 
                                var transferFromAccount = accountList[index - 1]; // "index - 1" list is Zero based index

                            transferDestinationCheckLabel: Console.Clear();
                                Console.WriteLine("--------------------------------------------------------");
                                Console.WriteLine("\t       Please select the destination account:");
                                Console.WriteLine("--------------------------------------------------------");

                                int secondCount = 1; // secondcount for list numbering

                                //loops through the list of bank account ownered by this customer
                                foreach (var item in accountList)
                                {
                                    // print all other account except the account selected as payment source
                                    if(item != transferFromAccount)
                                    {
                                        //Populate the console with the option of accounts available
                                        Console.WriteLine($"\n{secondCount}:\t{item.AccountNumber}\t|\t{item.TypeOfAcc} \n");
                                        secondCount++;
                                    }
                                     
                                }

                                Console.WriteLine("\n\nPress * to Return to Main menu  \n");

                                Console.WriteLine("--------------------------------------------------------");


                                Console.Write("\nEnter your option:");
                                string secondSelected = Console.ReadLine();
                                //option entered check
                                if (secondSelected == "*") goto transferCheckLabel;

                                //convert the string value to integer 
                                bool secondResponse = int.TryParse(secondSelected, out int secondIndex); // .TryParse return boolean value

                                if (secondResponse && secondIndex < secondCount && secondIndex > 0)//check that the option selected is in the list of account
                                {
                                    BankAccount transferDestinationAccount; //initialize a new variable to store the destination account

                                    //account transfering has been removed from the list, the index will reduce by 1
                                    if (secondIndex < index) transferDestinationAccount = accountList[secondIndex - 1]; // "index - 1" list is Zero based index

                                    // the selected reciever has the same index with acccount transfering use the index directly
                                    else if (secondIndex == index) transferDestinationAccount = accountList[secondIndex];
                                    
                                    // the selected reciever has the greater index with acccount transfering use the index directly
                                    else transferDestinationAccount = accountList[secondIndex];

                                    transferDestinationLabel: Console.WriteLine($"\n{transferDestinationAccount.AccountNumber}");

                                    Console.Write("\nEnter the Amount you want to transfer: "); // check another label
                                    string transferAmount = Console.ReadLine();

                                    Console.Write("\nEnter Note: ");
                                    string transferNote = Console.ReadLine(); //collect withdrawal note

                                    // call the amountchecker method to parse and return withdrawAmount if valid and zero if invalid
                                    var parseCheck = amountChecker(transferAmount);

                                    if(parseCheck > 0)
                                    {
                                        //check that the account type is savings
                                        // check that the account balance will remain 100 after transaction
                                        if (transferFromAccount.TypeOfAcc == AccountType.Savings && transferFromAccount.Balance - parseCheck < 100)
                                        {
                                            Console.ForegroundColor = ConsoleColor.Red; // set the text color to red
                                            Console.WriteLine($"\nInsufficient funds for this transaction. \n");

                                            Console.ResetColor(); // Reset the console text color to default

                                            Console.ReadLine();
                                            Console.Clear();
                                            goto transferCheckLabel;
                                        }
                                        else
                                        {
                                            transferFromAccount.MakeTransfer(transferDestinationAccount, parseCheck, DateTime.Now, transferNote);
                                            Console.ForegroundColor = ConsoleColor.Yellow; // set the text color to yellow
                                            Console.WriteLine($"\n\nTransaction successful... \n");
                                            Console.ResetColor(); // Reset the console text color to default

                                            Console.ReadLine();
                                            PerformTransaction(customer.CustomerId, customer.FullName);

                                        }


                                        if (transferFromAccount.TypeOfAcc == AccountType.Current && transferFromAccount.Balance - parseCheck < 0)
                                        {
                                            Console.ForegroundColor = ConsoleColor.Red; // set the text color to red
                                            Console.WriteLine($"\nInsufficient funds for this transaction. \n");

                                            Console.ResetColor(); // Reset the console text color to default

                                            Console.ReadLine();
                                            Console.Clear();
                                            goto transferCheckLabel;
                                        }
                                        else

                                        {
                                            transferFromAccount.MakeTransfer(transferDestinationAccount, parseCheck, DateTime.Now, transferNote);
                                            Console.ForegroundColor = ConsoleColor.Yellow; // set the text color to yellow
                                            Console.WriteLine($"\n\nTransaction successful... \n");
                                            Console.ResetColor(); // Reset the console text color to default

                                            Console.ReadLine();
                                            PerformTransaction(customer.CustomerId, customer.FullName);
                                        }




                                    }
                                    else
                                    {
                                        Console.ForegroundColor = ConsoleColor.Red; // set the text color to red
                                        Console.WriteLine($"\nInvalid Amount Entry. \n");

                                        Console.ResetColor(); // Reset the console text color to default
                                        Console.WriteLine($"\nPlease try again... ");

                                        Console.ReadLine();
                                        Console.Clear();
                                        goto transferDestinationLabel;// continue call to return to the label
                                    }


                                }
                                else
                                {
                                    Console.ForegroundColor = ConsoleColor.Red; // set the text color to red
                                    Console.WriteLine($"\nInvalid option.. \n");

                                    Console.ResetColor(); // Reset the console text color to default
                                    Console.WriteLine($"\nPlease try again.. ");

                                    Console.ReadLine();
                                    // return to the accountPick point
                                    goto transferDestinationCheckLabel;

                                }

                            }
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.Red; // set the text color to red
                                Console.WriteLine($"\nInvalid option.. \n");

                                Console.ResetColor(); // Reset the console text color to default
                                Console.WriteLine($"\nPlease try again.. ");

                                Console.ReadLine();
                                // return to the accountPick point
                                goto transferCheckLabel;
                            }

                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red; // set the text color to red
                            Console.WriteLine($"\nInvalid option, You only have one account... \n");

                            Console.ResetColor(); // Reset the console text color to default

                            Console.ReadLine();
                            goto transferLabel;

                        }
                        break;
                    case "2":

                        case2TransferLabel: Console.Clear();
                        Console.WriteLine("--------------------------------------------------------");
                        Console.WriteLine("\t       Please select a payment source:");
                        Console.WriteLine("--------------------------------------------------------");

                        //loops through the list of bank account ownered by this customer
                        int custCount = 1;
                        foreach (var item in accountList)
                        {
                            //Populate the console with the option of accounts available
                            Console.WriteLine($"\n{custCount}:\t{item.AccountNumber}\t|\t{item.TypeOfAcc} \n");
                            custCount++;
                        }

                        Console.WriteLine("\n\nPress * to Return to Main menu  \n");

                        Console.WriteLine("--------------------------------------------------------");


                        Console.Write("\nEnter your option:");
                        string input = Console.ReadLine();

                        //option entered check
                        if (input == "*") goto transferLabel;

                        //convert the string value to integer 
                        bool inputResponse = int.TryParse(input, out int custIndex); // .TryParse return boolean value
                        if (inputResponse && custIndex < custCount && custIndex > 0)//check that the option selected is in the list of account
                        {
                            var secondTransferFromAccount = accountList[custIndex - 1];
                            case2SecondTransferLabel: Console.Write("\nEnter the Account Number of the Reciever: "); // check another label
                            string transferDestinationCustomer = Console.ReadLine();

                            bool resp = int.TryParse(transferDestinationCustomer, out int transferDestinationCustomerParse);


                            BankAccount myCustomerAccount = null;
                            foreach (BankAccount item in Bank.allCustomersBankAccount)
                            {
                                if (item.AccountNumber == transferDestinationCustomerParse.ToString())
                                {
                                    myCustomerAccount = item;
                                    break;
                                }
                            }
                            //check if found

                            if (myCustomerAccount == null)
                            {
                                Console.ForegroundColor = ConsoleColor.Red; // set the text color to red
                                Console.WriteLine($"\nAccount number does not exist with us.. \n");
                                Console.ResetColor(); // Reset the console text color to default
                                Console.ReadKey();
                                Console.Clear();
                                goto case2TransferLabel;

                            }
                            else
                            {
                                Console.Write("\nEnter Amount to transfer: ");
                                string transferDestinationCustomerAmount = Console.ReadLine();
                                Console.Write("\nEnter Amount to transfer: ");
                                var parseCheck = amountChecker(transferDestinationCustomerAmount);

                                if (parseCheck > 0)
                                {
                                    Console.Write("\nEnter Note: "); // check another label
                                    string transferCustomerNote = Console.ReadLine();

                                    //check that the account type is savings
                                    // check that the account balance will remain 100 after transaction
                                    if (secondTransferFromAccount.TypeOfAcc == AccountType.Savings && secondTransferFromAccount.Balance - parseCheck < 100)
                                    {
                                        Console.ForegroundColor = ConsoleColor.Red; // set the text color to red
                                        Console.WriteLine($"\nInsufficient funds for this transaction. \n");

                                        Console.ResetColor(); // Reset the console text color to default

                                        Console.ReadLine();
                                        Console.Clear();
                                        goto case2TransferLabel;
                                    }
                                    else
                                    {
                                        //All has been met call the bank account withdrawal method
                                        secondTransferFromAccount.MakeTransfer(myCustomerAccount, parseCheck, DateTime.Now, transferCustomerNote);
                                        Console.ForegroundColor = ConsoleColor.Yellow; // set the text color to yellow
                                        Console.WriteLine($"\n\nTransaction successful... \n");
                                        Console.ResetColor(); // Reset the console text color to default

                                        Console.ReadKey();
                                        PerformTransaction(customer.CustomerId, customer.FullName);

                                    }

                                        // check that the account type is current
                                        // check that the account balance will be enough to carry out the transaction
                                    if (secondTransferFromAccount.TypeOfAcc == AccountType.Current && secondTransferFromAccount.Balance - parseCheck < 0)
                                    {
                                        Console.ForegroundColor = ConsoleColor.Red; // set the text color to red
                                        Console.WriteLine($"\nInsufficient funds for this transaction. \n");

                                        Console.ResetColor(); // Reset the console text color to default

                                        Console.ReadLine();
                                        Console.Clear();
                                        //goto transferCheckLabel;
                                    }
                                    else

                                    {
                                        secondTransferFromAccount.MakeTransfer(myCustomerAccount, parseCheck, DateTime.Now, transferCustomerNote);
                                        Console.ForegroundColor = ConsoleColor.Yellow; // set the text color to yellow
                                        Console.WriteLine($"\n\nTransaction successful... \n");
                                        Console.ResetColor(); // Reset the console text color to default

                                        Console.ReadLine();
                                        PerformTransaction(customer.CustomerId, customer.FullName);
                                    }


                                }
                                else
                                {
                                    Console.ForegroundColor = ConsoleColor.Red; // set the text color to red
                                    Console.WriteLine($"\nInvalid Amount Entry. \n");

                                    Console.ResetColor(); // Reset the console text color to default
                                    Console.WriteLine($"\nPlease try again... ");

                                    Console.ReadLine();
                                    Console.Clear();
                                    goto case2TransferLabel;
                                }






                            }



                            /*

                            

*/

                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red; // set the text color to red
                            Console.WriteLine($"\nInvalid option.. \n");

                            Console.ResetColor(); // Reset the console text color to default
                            Console.WriteLine($"\nPlease try again.. ");

                            Console.ReadLine();
                            Console.Clear();

                            goto case2TransferLabel;
                        }


                            break;
                    case "*":
                        PerformTransaction(customer.CustomerId, customer.FullName);
                        break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red; // set the text color to red
                        Console.WriteLine($"\nInvalid option.. \n");

                        Console.ResetColor(); // Reset the console text color to default
                        Console.WriteLine($"\nPlease try again.. ");

                        Console.ReadLine();
                        Console.Clear();
                        Transfer(customer);
                        break;

                }

            }


        }

        public static void CheckBalance(Customer customer)
        {

            var accountList = new List<BankAccount>();
            accountList = customer.customerAccountList();

            if (accountList.Count < 1)
            {
                Console.ForegroundColor = ConsoleColor.Red; // set the text color to yellow
                Console.WriteLine($"\n\nYou have not opened any account... \n");
                Console.ResetColor(); // Reset the console text color to default
                Console.ReadKey();
                Profile(customer.CustomerId, customer.FullName);

            }
            else
            {
                balanceCheckLabel:  Console.Clear();
                Console.WriteLine("--------------------------------------------------------");
                Console.WriteLine("\t       Please select the transaction source:");
                Console.WriteLine("--------------------------------------------------------");

                //loops through the list of bank account ownered by this customer
                int count = 1;
                foreach (var item in accountList)
                {
                    //Populate the console with the option of accounts available
                    Console.WriteLine($"\n{count}:\t{item.AccountNumber}\t|\t{item.TypeOfAcc} \n");
                    count++;
                }
                
               
                Console.WriteLine("\n\nPress * to Return to Main menu  \n");

                Console.WriteLine("--------------------------------------------------------");


                Console.Write("\nEnter your option:");
                string selected = Console.ReadLine();
                //chek that key entered is not * else call PerformTransaction 
                if (selected == "*") PerformTransaction(customer.CustomerId, customer.FullName);

                //convert the string value to integer 
                bool response = int.TryParse(selected, out int index); // .TryParse return boolean value

                if (response && index < count && index > 0)
                {
                    // selected the actual account to be used 
                    var BalanceCheckAccount = accountList[index - 1]; // "index - 1" list is Zero based index

                    Console.ForegroundColor = ConsoleColor.Yellow; // set the text color to yellow
                    Console.WriteLine($"\n\nGreat! Your account balance is N{BalanceCheckAccount.Balance} \n");
                    Console.ResetColor(); // Reset the console text color to default

                    Console.ReadLine();
                    PerformTransaction(customer.CustomerId, customer.FullName);

                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red; // set the text color to red
                    Console.WriteLine($"\nInvalid option.. \n");

                    Console.ResetColor(); // Reset the console text color to default
                    Console.WriteLine($"\nPlease try again.. ");

                    Console.ReadLine();
                    Console.Clear();
                    goto balanceCheckLabel;
                }


            }

        }

        
        public static void StatementOfAccount(Customer customer)
        {
            var accountList = new List<BankAccount>();
            accountList = customer.customerAccountList();

            if (accountList.Count < 1)
            {
                Console.ForegroundColor = ConsoleColor.Red; // set the text color to yellow
                Console.WriteLine($"\n\nYou have not opened any account... \n");
                Console.ResetColor(); // Reset the console text color to default
                Console.ReadKey();
                Profile(customer.CustomerId, customer.FullName);

            }
            else
            {
            statementCheckLabel: Console.Clear();
                Console.WriteLine("--------------------------------------------------------");
                Console.WriteLine("\t       Please select the transaction source:");
                Console.WriteLine("--------------------------------------------------------");

                //loops through the list of bank account ownered by this customer
                int count = 1;
                foreach (var item in accountList)
                {
                    //Populate the console with the option of accounts available
                    Console.WriteLine($"\n{count}:\t{item.AccountNumber}\t|\t{item.TypeOfAcc} \n");
                    count++;
                }


                Console.WriteLine("\n\n\nPress * to Return to Main menu  \n");

                Console.WriteLine("--------------------------------------------------------");


                Console.Write("\nEnter your option:");
                string selected = Console.ReadLine();
                //chek that key entered is not * else call PerformTransaction 
                if (selected == "*") PerformTransaction(customer.CustomerId, customer.FullName);

                //convert the string value to integer 
                bool response = int.TryParse(selected, out int index); // .TryParse return boolean value

                if (response && index < count && index > 0)
                {
                    // selected the actual account to be used 
                    var statementCheckAccount = accountList[index - 1]; // "index - 1" list is Zero based index

                    Console.Clear();
                    Console.WriteLine($"\n\t\tSTATEMENT OF ACCOUNT");
                    Console.ForegroundColor = ConsoleColor.Yellow; // set the text color to red

                    Console.WriteLine($"{statementCheckAccount.GetAccountHistory()}");
                    Console.ResetColor(); // Reset the console text color to default

                    Console.ReadKey();
                    PerformTransaction(customer.CustomerId, customer.FullName);


                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red; // set the text color to red
                    Console.WriteLine($"\nInvalid option.. \n");

                    Console.ResetColor(); // Reset the console text color to default
                    Console.WriteLine($"\nPlease try again.. ");

                    Console.ReadLine();
                    Console.Clear();
                    goto statementCheckLabel;
                }


            }
        }

        //method to parse amount before transaction
        public static decimal amountChecker(string amount)
        {
            bool response = decimal.TryParse(amount, out decimal amountChecked); // .TryParse return boolean value

            if (response) return amountChecked;
            
            else return 0;
        }
       
        























    }
}
