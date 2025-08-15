using System;
using System.Collections.Generic;

namespace WarehouseSystem
{
    // Marker Interface
    public interface IInventoryItem
    {
        int Id { get; }
        string Name { get; }
        int Quantity { get; set; }
    }

    // Product Classes
    public class ElectronicItem : IInventoryItem
    {
        public int Id { get; }
        public string Name { get; }
        public int Quantity { get; set; }
        public string Brand { get; }
        public int WarrantyMonths { get; }

        public ElectronicItem(int id, string name, int quantity, string brand, int warrantyMonths)
        {
            Id = id;
            Name = name;
            Quantity = quantity;
            Brand = brand;
            WarrantyMonths = warrantyMonths;
        }

        public override string ToString() => 
            $"[Electronics] ID: {Id}, {Name} ({Brand}), Qty: {Quantity}, Warranty: {WarrantyMonths} months";
    }

    public class GroceryItem : IInventoryItem
    {
        public int Id { get; }
        public string Name { get; }
        public int Quantity { get; set; }
        public DateTime ExpiryDate { get; }

        public GroceryItem(int id, string name, int quantity, DateTime expiryDate)
        {
            Id = id;
            Name = name;
            Quantity = quantity;
            ExpiryDate = expiryDate;
        }

        public override string ToString() => 
            $"[Grocery] ID: {Id}, {Name}, Qty: {Quantity}, Expires: {ExpiryDate:d}";
    }

    // Custom Exceptions
    public class DuplicateItemException : Exception
    {
        public DuplicateItemException(string message) : base(message) { }
    }

    public class ItemNotFoundException : Exception
    {
        public ItemNotFoundException(string message) : base(message) { }
    }

    public class InvalidQuantityException : Exception
    {
        public InvalidQuantityException(string message) : base(message) { }
    }

    // Generic Inventory Repository
    public class InventoryRepository<T> where T : IInventoryItem
    {
        private readonly Dictionary<int, T> _items = new();

        public void AddItem(T item)
        {
            if (_items.ContainsKey(item.Id))
            {
                throw new DuplicateItemException($"Item with ID {item.Id} already exists.");
            }
            _items.Add(item.Id, item);
        }

        public T GetItemById(int id)
        {
            if (!_items.TryGetValue(id, out var item))
            {
                throw new ItemNotFoundException($"Item with ID {id} not found.");
            }
            return item;
        }

        public void RemoveItem(int id)
        {
            if (!_items.ContainsKey(id))
            {
                throw new ItemNotFoundException($"Item with ID {id} not found.");
            }
            _items.Remove(id);
        }

        public List<T> GetAllItems() => new List<T>(_items.Values);

        public void UpdateQuantity(int id, int newQuantity)
        {
            if (newQuantity < 0)
            {
                throw new InvalidQuantityException("Quantity cannot be negative.");
            }
            var item = GetItemById(id);
            item.Quantity = newQuantity;
        }
    }

    // Warehouse Manager
    public class WarehouseManager
    {
        private readonly InventoryRepository<ElectronicItem> _electronics = new();
        private readonly InventoryRepository<GroceryItem> _groceries = new();

        public void Run()
        {
            SeedData();
            
            Console.WriteLine("=== Warehouse Inventory System ===");
            
            while (true)
            {
                Console.WriteLine("\n1. View All Inventory");
                Console.WriteLine("2. Add New Item");
                Console.WriteLine("3. Update Item Quantity");
                Console.WriteLine("4. Remove Item");
                Console.WriteLine("5. Exit");
                Console.Write("Select option: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        ViewAllInventory();
                        break;
                    case "2":
                        AddNewItem();
                        break;
                    case "3":
                        UpdateItemQuantity();
                        break;
                    case "4":
                        RemoveItem();
                        break;
                    case "5":
                        return;
                    default:
                        Console.WriteLine("Invalid option!");
                        break;
                }
            }
        }

        private void SeedData()
        {
            // Seed electronics
            _electronics.AddItem(new ElectronicItem(1, "Laptop", 10, "Dell", 24));
            _electronics.AddItem(new ElectronicItem(2, "Smartphone", 25, "Samsung", 12));

            // Seed groceries
            _groceries.AddItem(new GroceryItem(1, "Milk", 50, DateTime.Now.AddDays(7)));
            _groceries.AddItem(new GroceryItem(2, "Bread", 30, DateTime.Now.AddDays(3)));
        }

        private void ViewAllInventory()
        {
            Console.WriteLine("\n=== Electronics ===");
            foreach (var item in _electronics.GetAllItems())
            {
                Console.WriteLine(item);
            }

            Console.WriteLine("\n=== Groceries ===");
            foreach (var item in _groceries.GetAllItems())
            {
                Console.WriteLine(item);
            }
        }

        private void AddNewItem()
        {
            try
            {
                Console.WriteLine("\n1. Add Electronic Item");
                Console.WriteLine("2. Add Grocery Item");
                Console.Write("Select type: ");
                var typeChoice = Console.ReadLine();

                Console.Write("Enter Item ID: ");
                var id = int.Parse(Console.ReadLine() ?? "0");

                Console.Write("Enter Item Name: ");
                var name = Console.ReadLine() ?? "";

                Console.Write("Enter Quantity: ");
                var quantity = int.Parse(Console.ReadLine() ?? "0");

                if (typeChoice == "1")
                {
                    Console.Write("Enter Brand: ");
                    var brand = Console.ReadLine() ?? "";

                    Console.Write("Enter Warranty (months): ");
                    var warranty = int.Parse(Console.ReadLine() ?? "0");

                    _electronics.AddItem(new ElectronicItem(id, name, quantity, brand, warranty));
                }
                else if (typeChoice == "2")
                {
                    Console.Write("Enter Expiry Date (yyyy-mm-dd): ");
                    var expiry = DateTime.Parse(Console.ReadLine() ?? DateTime.Now.ToString());

                    _groceries.AddItem(new GroceryItem(id, name, quantity, expiry));
                }
                Console.WriteLine("Item added successfully!");
            }
            catch (Exception ex) when (
                ex is FormatException || 
                ex is DuplicateItemException)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private void UpdateItemQuantity()
        {
            try
            {
                Console.WriteLine("\n1. Update Electronics Quantity");
                Console.WriteLine("2. Update Groceries Quantity");
                Console.Write("Select type: ");
                var typeChoice = Console.ReadLine();

                Console.Write("Enter Item ID: ");
                var id = int.Parse(Console.ReadLine() ?? "0");

                Console.Write("Enter New Quantity: ");
                var newQty = int.Parse(Console.ReadLine() ?? "0");

                if (typeChoice == "1")
                {
                    _electronics.UpdateQuantity(id, newQty);
                }
                else if (typeChoice == "2")
                {
                    _groceries.UpdateQuantity(id, newQty);
                }
                Console.WriteLine("Quantity updated successfully!");
            }
            catch (Exception ex) when (
                ex is FormatException || 
                ex is ItemNotFoundException ||
                ex is InvalidQuantityException)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private void RemoveItem()
        {
            try
            {
                Console.WriteLine("\n1. Remove Electronic Item");
                Console.WriteLine("2. Remove Grocery Item");
                Console.Write("Select type: ");
                var typeChoice = Console.ReadLine();

                Console.Write("Enter Item ID: ");
                var id = int.Parse(Console.ReadLine() ?? "0");

                if (typeChoice == "1")
                {
                    _electronics.RemoveItem(id);
                }
                else if (typeChoice == "2")
                {
                    _groceries.RemoveItem(id);
                }
                Console.WriteLine("Item removed successfully!");
            }
            catch (Exception ex) when (
                ex is FormatException || 
                ex is ItemNotFoundException)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }

    // Main Program
    class Program
    {
        static void Main(string[] args)
        {
            var manager = new WarehouseManager();
            manager.Run();
            Console.WriteLine("\nThank you for using the Warehouse System!");
        }
    }
}