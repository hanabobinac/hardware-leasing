namespace CleaseSolution
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft;
    using RestSharp;

    public class Program
    {
        private static readonly IPleaseClient Client = new PleaseClient("http://172.17.0.1:5000");

        public static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Hardware lease service!");
            Console.WriteLine($"The current time is {DateTime.Now}");

            while (true)
            {
                PrintHeader();
                var choice = Console.ReadLine();
                var docontinue = ExecuteChoice(choice.ToLower());
                if (!docontinue)
                {
                    break;
                }
            }
        }

        private static bool ExecuteChoice(string choice)
        {
            switch (choice)
            {
                case "1":
                    ExecuteList();
                    break;
                case "2":
                    ExecuteListPerPlatform();
                    break;
                case "3":
                    ExecuteListLeasedHardware();
                    break;
                case "4":
                    ExecuteAddHardware();
                    break;
                case "5":
                    ExecuteLeaseHardware();
                    break;
                case "q":
                case "quit":
                    Console.WriteLine("Goodbye!");
                    return false;
                default:
                    Console.WriteLine("Choice not valid, please try again.");
                    break;
            }

            return true;
        }

        private static void ExecuteList()
        {
            var hardwareList = Client.GetHardwareList();
            Console.WriteLine("\nList of all hardware:");
            PrintHardwareList(hardwareList);
        }

        private static void ExecuteListPerPlatform()
        {
            Console.Write("Enter platform: ");
            var platform = Console.ReadLine();

            var hardwareList = Client.GetHardwareList(platform);
            Console.WriteLine($"\nList of all {platform}s:");
            PrintHardwareList(hardwareList);
        }

        private static void ExecuteListLeasedHardware()
        {
            var hardwareList = Client.GetLeasedHardwareList();
            Console.WriteLine("\nList of all leased hardware:");
            PrintHardwareList(hardwareList);
        }

        private static void ExecuteAddHardware()
        {
            Console.Write("Enter name: ");
            var name = Console.ReadLine();

            Console.Write("Enter ip: ");
            var ip = Console.ReadLine();

            Console.Write("Enter platform: ");
            var platformString = Console.ReadLine();

            Platform platform = default(Platform);
            var isValid = Enum.TryParse<Platform>(platformString, out platform);
            if (!isValid)
            {
                Console.WriteLine("Choice not valid, please try again.");
                return;
            }

            var hw = new Hardware
            {
                Name = name,
                Ip = ip,
                Platform = platform
            };

            var response = Client.AddHardware(hw);
            Console.WriteLine($"Add hardware: {response}");
        }

        private static void ExecuteLeaseHardware()
        {
            Console.Write("Enter platform: ");
            var platformString = Console.ReadLine();

            Console.Write("Enter lease duration: ");
            var leaseDurationString = Console.ReadLine();

            Platform platform = default(Platform);
            var isValid = Enum.TryParse<Platform>(platformString, out platform);
            if (!isValid)
            {
                Console.WriteLine("Choice not valid, please try again.");
                return;
            }

            var leaseDuration = default(int);
            isValid = int.TryParse(leaseDurationString, out leaseDuration);
            if (!isValid || leaseDuration <= 0)
            {
                Console.WriteLine("Choice not valid, please try again.");
                return;
            }

            var response = Client.LeaseHardware(platform, leaseDuration);
            Console.WriteLine($"Lease hardware: {response}");
        }

        private static void PrintHeader()
        {
            Console.WriteLine();
            Console.WriteLine("Please choose:");
            Console.WriteLine("-------------------------------");
            Console.WriteLine("(1) List all hardware");
            Console.WriteLine("(2) List hardware per platform");
            Console.WriteLine("(3) List all leased hardware");
            Console.WriteLine("(4) Add hardware");
            Console.WriteLine("(5) Lease hardware");
            Console.WriteLine("(q) Quit");
            Console.WriteLine("-------------------------------");
        }

        private static void PrintHardwareList(List<Hardware> hardwareList)
        {
            foreach (var item in hardwareList)
            {
                Console.WriteLine($"Hardware name: {item.Name}, platform: {item.Platform}, ip: {item.Ip}, lease duration: {item.LeaseDuration}, lease date: {item.LeaseDate} is leased: {item.IsLeased}");
            }

            Console.WriteLine();
        }
    }
}
