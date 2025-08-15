using System;
using System.Collections.Generic;
using System.Linq;

namespace HealthcareSystem
{
    // Patient Class
    public class Patient
    {
        public int Id { get; }
        public string Name { get; }
        public int Age { get; }
        public string Gender { get; }

        public Patient(int id, string name, int age, string gender)
        {
            Id = id;
            Name = name;
            Age = age;
            Gender = gender;
        }

        public override string ToString() => 
            $"ID: {Id}, Name: {Name}, Age: {Age}, Gender: {Gender}";
    }

    // Prescription Class
    public class Prescription
    {
        public int Id { get; }
        public int PatientId { get; }
        public string MedicationName { get; }
        public DateTime DateIssued { get; }

        public Prescription(int id, int patientId, string medicationName, DateTime dateIssued)
        {
            Id = id;
            PatientId = patientId;
            MedicationName = medicationName;
            DateIssued = dateIssued;
        }

        public override string ToString() => 
            $"Medication: {MedicationName}, Date: {DateIssued:d}, For Patient ID: {PatientId}";
    }

    // Generic Repository
    public class Repository<T> where T : class
    {
        private readonly List<T> _items = new();

        public void Add(T item) => _items.Add(item);
        public List<T> GetAll() => _items;
        
        public T? GetById(Func<T, bool> predicate) => _items.FirstOrDefault(predicate);
        
        public bool Remove(Func<T, bool> predicate)
        {
            var item = GetById(predicate);
            return item != null && _items.Remove(item);
        }
    }

    // Health System Application
    public class HealthSystemApp
    {
        private readonly Repository<Patient> _patientRepo = new();
        private readonly Repository<Prescription> _prescriptionRepo = new();
        private Dictionary<int, List<Prescription>> _prescriptionMap = new();

        public void Run()
        {
            SeedData();
            BuildPrescriptionMap();

            Console.WriteLine("=== Healthcare Management System ===");
            
            while (true)
            {
                Console.WriteLine("\n1. View All Patients");
                Console.WriteLine("2. View Patient Prescriptions");
                Console.WriteLine("3. Add New Patient");
                Console.WriteLine("4. Add New Prescription");
                Console.WriteLine("5. Exit");
                Console.Write("Select option: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        PrintAllPatients();
                        break;
                    case "2":
                        ViewPatientPrescriptions();
                        break;
                    case "3":
                        AddNewPatient();
                        break;
                    case "4":
                        AddNewPrescription();
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
            // Add sample patients
            _patientRepo.Add(new Patient(1, "John Doe", 35, "Male"));
            _patientRepo.Add(new Patient(2, "Jane Smith", 28, "Female"));
            _patientRepo.Add(new Patient(3, "Michael Johnson", 45, "Male"));

            // Add sample prescriptions
            _prescriptionRepo.Add(new Prescription(1, 1, "Ibuprofen", DateTime.Now.AddDays(-7)));
            _prescriptionRepo.Add(new Prescription(2, 1, "Amoxicillin", DateTime.Now.AddDays(-3)));
            _prescriptionRepo.Add(new Prescription(3, 2, "Paracetamol", DateTime.Now.AddDays(-5)));
            _prescriptionRepo.Add(new Prescription(4, 3, "Lisinopril", DateTime.Now.AddDays(-1)));
        }

        private void BuildPrescriptionMap()
        {
            _prescriptionMap = _prescriptionRepo.GetAll()
                .GroupBy(p => p.PatientId)
                .ToDictionary(g => g.Key, g => g.ToList());
        }

        private void PrintAllPatients()
        {
            Console.WriteLine("\n=== Patient List ===");
            foreach (var patient in _patientRepo.GetAll())
            {
                Console.WriteLine(patient);
            }
        }

        private void ViewPatientPrescriptions()
        {
            Console.Write("\nEnter Patient ID: ");
            if (int.TryParse(Console.ReadLine(), out int patientId))
            {
                if (_prescriptionMap.TryGetValue(patientId, out var prescriptions))
                {
                    Console.WriteLine($"\nPrescriptions for Patient ID {patientId}:");
                    foreach (var prescription in prescriptions)
                    {
                        Console.WriteLine(prescription);
                    }
                }
                else
                {
                    Console.WriteLine("No prescriptions found for this patient.");
                }
            }
            else
            {
                Console.WriteLine("Invalid Patient ID!");
            }
        }

        private void AddNewPatient()
        {
            try
            {
                Console.Write("\nEnter Patient Name: ");
                var name = Console.ReadLine() ?? "";

                Console.Write("Enter Age: ");
                var age = int.Parse(Console.ReadLine() ?? "0");

                Console.Write("Enter Gender: ");
                var gender = Console.ReadLine() ?? "";

                var newId = _patientRepo.GetAll().Count + 1;
                _patientRepo.Add(new Patient(newId, name, age, gender));
                Console.WriteLine($"Patient added successfully with ID: {newId}");
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid input format!");
            }
        }

        private void AddNewPrescription()
        {
            try
            {
                Console.Write("\nEnter Patient ID: ");
                var patientId = int.Parse(Console.ReadLine() ?? "0");

                Console.Write("Enter Medication Name: ");
                var medication = Console.ReadLine() ?? "";

                var newId = _prescriptionRepo.GetAll().Count + 1;
                _prescriptionRepo.Add(new Prescription(newId, patientId, medication, DateTime.Now));
                BuildPrescriptionMap(); // Rebuild the mapping
                Console.WriteLine("Prescription added successfully!");
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid input format!");
            }
        }
    }

    // Main Program
    class Program
    {
        static void Main(string[] args)
        {
            var app = new HealthSystemApp();
            app.Run();
            Console.WriteLine("\nThank you for using the Healthcare System!");
        }
    }
}