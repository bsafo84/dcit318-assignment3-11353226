# DCIT 318 - Programming II Assignment

This repository contains complete console application implementations for all 5 assignment questions in C#, demonstrating advanced programming concepts including:

- Records for immutable data
- Interfaces and polymorphism
- Generic classes and methods
- File I/O operations
- Exception handling
- Collections (List, Dictionary)
- LINQ operations

## Question 1: Finance Management System

**Key Features:**
- Transaction processing system with multiple payment methods
- Savings account with overdraft protection
- Immutable transaction records
- Interface-based payment processors
- Sealed class for account security

**How to Run:**
```bash
cd FinanceManagementSystem
dotnet run
```

## Question 2: Healthcare Management System

**Key Features:**
- Generic repository pattern for patients and prescriptions
- Dictionary-based prescription grouping
- CRUD operations for medical records
- LINQ queries for data retrieval
- Console-based menu system

**How to Run:**
```bash
cd HealthcareSystem
dotnet run
```

## Question 3: Warehouse Inventory System

**Key Features:**
- Generic inventory repository
- Custom exception classes
- Two product types (Electronics/Groceries)
- Quantity validation
- Interactive inventory management

**How to Run:**
```bash
cd WarehouseSystem
dotnet run
```

## Question 4: School Grading System

**Key Features:**
- File-based grade processing
- Custom exceptions for data validation
- Automatic grade calculation (A-F)
- Error recovery for malformed data
- Report generation

**How to Run:**
```bash
cd SchoolGradingSystem
dotnet run
```

## Question 5: Inventory System with File Persistence

**Key Features:**
- JSON serialization for data persistence
- Immutable inventory records
- Generic logger class
- Type-safe inventory operations
- Tabular data display

**How to Run:**
```bash
cd InventorySystem
dotnet run
```

## Common Requirements Demonstrated

All solutions implement:
1. Proper encapsulation and SOLID principles
2. Comprehensive error handling
3. Clean console interfaces
4. Data validation
5. Separation of concerns

## Setup Instructions

1. Ensure .NET 6+ SDK is installed
2. Clone this repository
3. Navigate to each project folder
4. Run `dotnet restore` to install dependencies
5. Execute `dotnet run` to start each application

## Customization Options

Each solution can be extended with:
- Additional validation rules
- More sophisticated reporting
- Database integration
- Web API endpoints
- Unit test projects

For questions or issues, please open a GitHub ticket.
