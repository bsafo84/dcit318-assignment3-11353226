using System;
using System.Collections.Generic;

namespace FinanceManagementSystem
{
    // Transaction record (immutable)
    public record Transaction(int Id, DateTime Date, decimal Amount, string Category);

    // Transaction Processor Interface
    public interface ITransactionProcessor
    {
        void Process(Transaction transaction);
    }

    // Processor Implementations
    public class BankTransferProcessor : ITransactionProcessor
    {
        public void Process(Transaction transaction)
        {
            Console.WriteLine($"\n[Bank Transfer] Processed ${transaction.Amount} for {transaction.Category}");
        }
    }

    public class MobileMoneyProcessor : ITransactionProcessor
    {
        public void Process(Transaction transaction)
        {
            Console.WriteLine($"\n[Mobile Money] Processed ${transaction.Amount} for {transaction.Category}");
        }
    }

    public class CryptoWalletProcessor : ITransactionProcessor
    {
        public void Process(Transaction transaction)
        {
            Console.WriteLine($"\n[Crypto Wallet] Processed ${transaction.Amount} for {transaction.Category}");
        }
    }

    // Account Base Class
    public class Account
    {
        public string AccountNumber { get; }
        public decimal Balance { get; protected set; }

        public Account(string accountNumber, decimal initialBalance)
        {
            AccountNumber = accountNumber;
            Balance = initialBalance;
        }

        public virtual void ApplyTransaction(Transaction transaction)
        {
            Balance -= transaction.Amount;
            Console.WriteLine($"Deducted ${transaction.Amount} from account");
        }
    }

    // Sealed Savings Account
    public sealed class SavingsAccount : Account
    {
        public SavingsAccount(string accountNumber, decimal initialBalance) 
            : base(accountNumber, initialBalance) { }

        public override void ApplyTransaction(Transaction transaction)
        {
            if (transaction.Amount > Balance)
            {
                Console.WriteLine("\n⚠️ Transaction failed: Insufficient funds!");
            }
            else
            {
                base.ApplyTransaction(transaction);
                Console.WriteLine($"💰 New balance: ${Balance}");
            }
        }
    }

    // Main Program
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Finance Management System ===");
            
            // Create account
            var account = new SavingsAccount("ACC-123456", 1000m);
            Console.WriteLine($"Account created: {account.AccountNumber}");
            Console.WriteLine($"Initial balance: ${account.Balance}\n");

            // Create sample transactions
            var transactions = new List<Transaction>
            {
                new(1, DateTime.Now, 150m, "Groceries"),
                new(2, DateTime.Now, 75.50m, "Utilities"),
                new(3, DateTime.Now, 200m, "Entertainment")
            };

            // Create processors
            var processors = new Dictionary<int, ITransactionProcessor>
            {
                { 1, new MobileMoneyProcessor() },
                { 2, new BankTransferProcessor() },
                { 3, new CryptoWalletProcessor() }
            };

            // Process transactions
            for (int i = 0; i < transactions.Count; i++)
            {
                Console.WriteLine($"\nProcessing Transaction {i + 1}:");
                Console.WriteLine($"- Amount: ${transactions[i].Amount}");
                Console.WriteLine($"- Category: {transactions[i].Category}");

                processors[i + 1].Process(transactions[i]);
                account.ApplyTransaction(transactions[i]);
            }

            Console.WriteLine("\n=== Final Account Status ===");
            Console.WriteLine($"Remaining balance: ${account.Balance}");
        }
    }
}