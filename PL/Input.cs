using System;
using System.Text.RegularExpressions;

namespace PL
{
    internal static class Input
    {
        private static string CheckInput(string data, string format)
        {
            Regex regex = new Regex(format);
            while (!regex.IsMatch(data))
            {
                Console.WriteLine("\nValue is incorrect, please try again\n");
                data = Console.ReadLine();
            }
            return data;
        }

        public static string InputName(string info)
        {
            Console.WriteLine($"Enter the {info}");
            string data = Console.ReadLine();
            return CheckInput(data, @"^[A-Z]{1}[a-z]+$");
        }

        public static string InputPassport()
        {
            Console.WriteLine($"Enter the passport number (9 digits)");
            string passport = Console.ReadLine();
            return CheckInput(passport, @"^\d{9}$");
        }

        public static string InputStudentID()
        {
            Console.WriteLine("Enter the student ID (AB-12345)");
            string id = Console.ReadLine();
            return CheckInput(id, @"^[A-Z]{2}-\d{5}$");
        }

        public static int InputStudentCourse()
        {
            Console.WriteLine("Enter the student course (1-5)");
            string course = Console.ReadLine();
            return int.Parse(CheckInput(course, @"^[1-5]{1}$"));
        }

        public static string InputMilitaryID()
        {
            while (true)
            {
                Console.WriteLine($"Does student have a military ID? (y/n)");
                string answer = Console.ReadLine();
                switch (answer.ToLower())
                {
                    case "y":
                        Console.WriteLine("Enter the militaryID (6 digits)");
                        string militaryID = Console.ReadLine();
                        return CheckInput(militaryID, @"^\d{6}$");
                    case "n":
                        return "N/A";
                    default:
                        Console.WriteLine("Input is incorrect, please try again");
                        break;
                }
            }
        }

        public static string InputString(string prompt)
        {
            Console.WriteLine(prompt);
            string input = Console.ReadLine();
            return CheckInput(input, @"^[A-Za-z0-9-]+$");
        }
    }
}