using System;
using System.Collections.Generic;
using System.IO;

namespace SchoolGradingSystem
{
    // Student Class
    public class Student
    {
        public int Id { get; }
        public string FullName { get; }
        public int Score { get; }

        public Student(int id, string fullName, int score)
        {
            Id = id;
            FullName = fullName;
            Score = score;
        }

        public string GetGrade()
        {
            return Score switch
            {
                >= 80 and <= 100 => "A",
                >= 70 and < 80 => "B",
                >= 60 and < 70 => "C",
                >= 50 and < 60 => "D",
                _ => "F"
            };
        }

        public override string ToString() => 
            $"{FullName} (ID: {Id}): Score = {Score}, Grade = {GetGrade()}";
    }

    // Custom Exceptions
    public class InvalidScoreFormatException : Exception
    {
        public InvalidScoreFormatException(string message) : base(message) { }
    }

    public class MissingFieldException : Exception
    {
        public MissingFieldException(string message) : base(message) { }
    }

    // Student Result Processor
    public class GradingProcessor
    {
        public List<Student> ReadStudentsFromFile(string filePath)
        {
            var students = new List<Student>();
            int lineNumber = 0;

            try
            {
                using var reader = new StreamReader(filePath);
                while (!reader.EndOfStream)
                {
                    lineNumber++;
                    var line = reader.ReadLine();
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    try
                    {
                        var fields = line.Split(',');
                        if (fields.Length != 3)
                        {
                            throw new MissingFieldException(
                                $"Line {lineNumber}: Expected 3 fields but found {fields.Length}");
                        }

                        if (!int.TryParse(fields[0].Trim(), out int id))
                        {
                            throw new InvalidScoreFormatException(
                                $"Line {lineNumber}: Invalid ID format");
                        }

                        if (!int.TryParse(fields[2].Trim(), out int score))
                        {
                            throw new InvalidScoreFormatException(
                                $"Line {lineNumber}: Invalid score format");
                        }

                        students.Add(new Student(
                            id, 
                            fields[1].Trim(), 
                            score));
                    }
                    catch (Exception ex) when (
                        ex is MissingFieldException || 
                        ex is InvalidScoreFormatException)
                    {
                        Console.WriteLine($"Skipping line {lineNumber}: {ex.Message}");
                    }
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Error: Input file not found");
            }
            
            return students;
        }

        public void WriteReportToFile(List<Student> students, string outputPath)
        {
            try
            {
                using var writer = new StreamWriter(outputPath);
                foreach (var student in students)
                {
                    writer.WriteLine(student.ToString());
                }
                Console.WriteLine($"Report generated successfully at: {outputPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing report: {ex.Message}");
            }
        }

        public void CreateSampleInputFile()
        {
            try
            {
                using var writer = new StreamWriter("students_input.txt");
                writer.WriteLine("101, John Smith, 85");
                writer.WriteLine("102, Sarah Johnson, 72");
                writer.WriteLine("103, Michael Brown, 91");
                writer.WriteLine("104, Emily Davis, 64");
                writer.WriteLine("105, David Wilson, 55");
                Console.WriteLine("Sample input file created: students_input.txt");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating sample file: {ex.Message}");
            }
        }
    }

    // Main Program
    class Program
    {
        static void Main(string[] args)
        {
            var processor = new GradingProcessor();
            
            Console.WriteLine("=== School Grading System ===");
            Console.WriteLine("1. Process grades from file");
            Console.WriteLine("2. Create sample input file");
            Console.Write("Select option: ");

            switch (Console.ReadLine())
            {
                case "1":
                    ProcessGrades(processor);
                    break;
                case "2":
                    processor.CreateSampleInputFile();
                    break;
                default:
                    Console.WriteLine("Invalid option");
                    break;
            }
        }

        static void ProcessGrades(GradingProcessor processor)
        {
            try
            {
                Console.Write("\nEnter input file path (default: students_input.txt): ");
                var inputPath = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(inputPath))
                {
                    inputPath = "students_input.txt";
                }

                Console.Write("Enter output file path (default: grade_report.txt): ");
                var outputPath = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(outputPath))
                {
                    outputPath = "grade_report.txt";
                }

                var students = processor.ReadStudentsFromFile(inputPath);
                
                if (students.Count > 0)
                {
                    Console.WriteLine("\n=== Grading Results ===");
                    foreach (var student in students)
                    {
                        Console.WriteLine(student);
                    }

                    processor.WriteReportToFile(students, outputPath);
                }
                else
                {
                    Console.WriteLine("No valid student records found in input file.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}