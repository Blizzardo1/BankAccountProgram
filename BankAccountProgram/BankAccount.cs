using System;
using System.IO;
using Newtonsoft.Json;

namespace BankAccountProgram
{
    public class BankAccount
    {
        public const string ActionWithdraw = "WITHDRAW";
        public const string ActionDeposit = "DEPOSIT";


        private string _accountFile;
        
        
        [JsonProperty(PropertyName ="account-number", DefaultValueHandling = DefaultValueHandling.Populate)]
        public Guid Number { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty( PropertyName = "balance", DefaultValueHandling = DefaultValueHandling.Include, NullValueHandling = NullValueHandling.Include)]
        public decimal Balance { get; set; }

        public BankAccount(Guid number, string name, decimal balance)
        {
            Number = number;
            Name = name;
            Balance = balance;
        }

        public static BankAccount CreateAccount(string name, decimal balance)
        {
            return new BankAccount(Guid.NewGuid(), name, balance) {_accountFile = name};
        }

        public static BankAccount OpenAccount(string accountFile)
        {
            if (!File.Exists(accountFile))
            {
                throw new FileNotFoundException(nameof(accountFile));
            }

            BankAccount ba= ToBankAccount(File.OpenRead(accountFile));
            ba._accountFile = accountFile;
            return ba;
        }

        private static BankAccount ToBankAccount(Stream stream)
        {
            using (var sr = new StreamReader(stream))
            using (var jtr = new JsonTextReader(sr))
            {
                var serializer = new JsonSerializer();
                return serializer.Deserialize< BankAccount >(jtr);
            }
        }

        public void CloseAccount()
        {
            using (var w = new StreamWriter(_accountFile))
            {
                string json = JsonConvert.SerializeObject(this, Formatting.Indented);
                w.WriteLine(json);
            }
        }

        public decimal Withdraw(decimal amount)
        {
            if (Balance - amount < 0) throw new ArgumentException("Insufficient funds");
            return Balance -= amount;
        }


        public decimal Deposit(decimal amount) => Balance += amount;

        public void ProcessInput(string action, decimal input)
        {
            if (action.ToUpper() == ActionWithdraw)
            {
                Withdraw(input);
            }
            else if (action.ToUpper() == ActionDeposit)
            {
                Deposit(input);
            }

        }

        public bool CheckInput(string input) => decimal.TryParse(input, out decimal _);

        public bool CheckAction(string input) => input.ToUpper() == ActionDeposit || input.ToUpper() == ActionWithdraw;
    }
}