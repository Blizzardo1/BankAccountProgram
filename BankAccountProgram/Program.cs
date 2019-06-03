using System;
using System.IO;

namespace BankAccountProgram
{
    internal static class Program
    {
        private static BankAccount _account;

        private static void Main(string[] args)
        {
            Console.WriteLine("Hello There!");

            while (true)
            {

                try
                {
                    Console.Write("> ");
                    string[] arguments = Console.ReadLine()?.Split(' ');

                    switch (arguments[ 0 ].ToLower())
                    {
                        case "open":
                            string file = $"{arguments[ 1 ]}.account";
                            _account = BankAccount.OpenAccount(file);
                            break;

                        case "createifnotexist":
                            string newAccount = $"{arguments[ 1 ]}.account";
                            if (File.Exists(newAccount))
                            {
                                Console.WriteLine($"\"{Path.GetFullPath(newAccount)}\" exists already");
                                break;
                            }

                            Console.WriteLine($"Creating new Account \"{newAccount}\"");
                            decimal.TryParse(arguments[ 2 ], out decimal bal);
                            _account = BankAccount.CreateAccount(newAccount, bal);
                            _account.CloseAccount();
                            break;
                        
                        case "balance":
                            case "bal":
                            Console.WriteLine($"\"{_account.Name}\" Balance is \"${_account.Balance}\"");
                            break;

                        case "deposit":
                        case "dep":
                        case "add":
                            decimal.TryParse(arguments[ 1 ], out decimal b);
                            _account.Deposit(b);
                            break;
                        case "withdraw":
                        case "remove":
                        case "rem":

                            decimal.TryParse(arguments[ 1 ], out decimal d);
                            _account.Withdraw(d);
                            break;
                        
                        case "close":
                            _account.CloseAccount();
                            break;
                        
                        default:
                            Console.WriteLine($"\"{arguments[0]}\" is not a command");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }

                if (!Console.KeyAvailable) continue;
                if (Console.ReadKey(true).Key == ConsoleKey.Escape)
                {
                    break;
                }
            }

        }
    }
}