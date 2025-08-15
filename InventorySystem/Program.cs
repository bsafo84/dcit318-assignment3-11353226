using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace InventorySystem
{
    // Inventory Record (immutable)
    public record InventoryItem(int Id, string Name, int Quantity, DateTime DateAdded) : IInventoryEntity;

    // Marker Interface
    public interface IInventoryEntity
    {
        int Id { get; }
    }

    // Generic Inventory Logger
    public class InventoryLogger<T> where T : IInventoryEntity
    {
        private readonly List<T> _log = new();
        private readonly string _filePath;

        public InventoryLogger(string filePath)
        {
            _filePath = filePath;
        }

        public void Add(T item)
        {
            _log.Add(item);
            Console.WriteLine($"Added item: {item}");
        }

        public List<T> GetAll() => new List<T>(_log);

        public void SaveToFile()
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string json = JsonSerializer.Serialize(_log, options);
                File.WriteAllText(_filePath, json);
                Console.WriteLine($"\nSaved {_log.Count} items to {_filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving to file: {ex.Message}");
            }
        }

        public void LoadFromFile()
        {
            try
            {
                if (!File.Exists(_filePath))
                {
                    Console.WriteLine("No saved data found.");
                    return;
                }

                string json = File.ReadAllText(_filePath);
                var items = JsonSerializer.Deserialize<List<T>>(json);

                if (items != null && items.Count > 0)
                {
                    _log.Clear();
                    _log.AddRange(items);
                    Console.WriteLine($"\nLoaded {items.Count} items from {_filePath}");
                }
                else
                {
                    Console.WriteLine("No data found in file.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading from file: {ex.Message}");
            }
        }
    }

    // Inventory Application
    public class InventoryApp
    {
        private readonly InventoryLogger<InventoryItem> _logger;

        public InventoryApp()
        {
            _logger = new InventoryLogger<InventoryItem>("inventory.json");
        }

        public void Run()
        {
            Console.WriteLine("=== Inventory Management System ===");
            
            while (true)
            {
                Console.WriteLine("\n1. Add New Item");
                Console.WriteLine("2. View All Items");
                Console.WriteLine("3. Save to File");
                Console.WriteLine("4. Load from File");
                Console.WriteLine("5. Exit");
                Console.Write("Select option: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        AddNewItem();
                        break;
                    case "2":
                        ViewAllItems();
                        break;
                    case "3":
                        _logger.SaveToFile();
                        break;
                    case "4":
                        _logger.LoadFromFile();
                        break;
                    case "5":
                        return;
                    default:
                        Console.WriteLine("Invalid option!");
                        break;
                }
            }
        }

        private void AddNewItem()
        {
            try
            {
                Console.Write("\nEnter Item ID: ");
                int id = int.Parse(Console.ReadLine() ?? "0");

                Console.Write("Enter Item Name: ");
                string name = Console.ReadLine() ?? "Unnamed";

                Console.Write("Enter Quantity: ");
                int quantity = int.Parse(Console.ReadLine() ?? "0");

                var newItem = new InventoryItem(
                    id,
                    name,
                    quantity,
                    DateTime.Now
                );

                _logger.Add(newItem);
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid input format!");
            }
        }

        private void ViewAllItems()
        {
            var items = _logger.GetAll();
            if (items.Count == 0)
            {
                Console.WriteLine("\nNo items in inventory.");
                return;
            }

            Console.WriteLine("\n=== Current Inventory ===");
            Console.WriteLine($"{"ID",-5} {"Name",-20} {"Qty",-5} {"Date Added"}");
            foreach (var item in items)
            {
                Console.WriteLine($"{item.Id,-5} {item.Name,-20} {item.Quantity,-5} {item.DateAdded:g}");
            }
        }
    }

    // Main Program
    class Program
    {
        static void Main(string[] args)
        {
            var app = new InventoryApp();
            app.Run();
            Console.WriteLine("\nThank you for using the Inventory System!");
        }
    }
}