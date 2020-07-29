using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Transactions;

namespace SlimBank
{
    class Program
    {
        static void Main(string[] args)
        {
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


        public static void CustomerSignUp()
        {
            Console.Clear();
            Console.WriteLine("----------------------------------");

            Console.ForegroundColor = ConsoleColor.Cyan; // set the text color to yellow
            Console.WriteLine("   --- Create new Customer ---  ");
            Console.ResetColor(); // Reset the console text color to default

            Console.WriteLine("----------------------------------");

            Console.ForegroundColor = ConsoleColor.White; // set the text color to white

            Console.Write("\nEnter Username: ");
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


            Console.Write("\nEnter username: ");
            string userName = Console.ReadLine();

            Console.Write("\nEnter password: ");
            string password = Console.ReadLine();

            Customer myCustomer = null;
            foreach (Customer item in Bank.allCustomers)
            {
                if (item.Username == userName && item.Password == password)
                {
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

                Console.WriteLine("\nPress * to Sign Up  || Press Any other key to try again");
                string command = Console.ReadLine();

                if (command == "*") CustomerSignUp();
                else  CustomerLogIn();
            } 
        }



        public static void Profile(int customerId, string name)
        {
            Console.Clear(); // clear the Console
            Console.WriteLine("---------------------------------------");

            Console.ForegroundColor = ConsoleColor.Cyan; // set the text color to cyan
            Console.WriteLine($"\t      Welcome {name}  ");
            Console.ResetColor(); // Reset the console text color to default

            Console.WriteLine("---------------------------------------");

            Console.ForegroundColor = ConsoleColor.White; // set the text color to white
            Console.WriteLine("\n1: Create Account\n");
            Console.WriteLine("2: Perform Transaction \n");
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
                PerformTransaction();
            }
            else if (command == "3")
            {
                // loop through the all customer list and find the matching Id
                Bank.allCustomers.ForEach(item => { if (item.CustomerId == customerId) item.LoggedOut(); });
                HomePage();

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

                if (response) CreateAccount(AccountType.Current, amount, customerId);

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

                if (response) CreateAccount(AccountType.Current, amount, customerId);

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


            try
            {
                BankAccount createAccount = new BankAccount(myCustomer, type, amount);

            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red; // set the text color to red
                Console.WriteLine($"\n\n{e.Message}\n");

                Console.ResetColor(); // Reset the console text color to default
                Console.Write("\nPress Any key to try again...");
                Console.ReadLine();
                selectAccountOperation(myCustomer.CustomerId, myCustomer.FullName);

            }
              
                Console.WriteLine("Your account created successfully");
                Console.ReadLine();
                PerformTransaction();
            

        }


        public static void PerformTransaction()
        {
            Console.Clear();
        }





















    
    }
}
