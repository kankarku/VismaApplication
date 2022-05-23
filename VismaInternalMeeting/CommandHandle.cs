using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VismaInternalMeeting.Interfaces;
using VismaInternalMeeting.Services;
using VismaInternalMeeting.Models;
using VismaInternalMeeting.Models.enums;
using System.Globalization;

namespace VismaInternalMeeting
{
    public class CommandHandle
    {
        private static IMeetingService _meetingService = new MeetingService();
        public static void Handle(string input)
        {
            try
            {
                switch (input.ToLower().TrimEnd())
                {
                    case "help":
                        ListCommands();
                        break;
                    case "list":
                        ListMeetings();
                        break;
                    case "create" or "add":
                        CreateMeeting();
                        break;
                    case "delete" or "remove":
                        DeleteMeeting();
                        break;
                    case "remove-person":
                        RemovePersonFromMeeting();
                        break;
                    case "add-person":
                        AddPersonToMeeting();
                        break;
                    case "filter":
                        FilterMeetings();
                        break;
                    default:
                        Console.WriteLine("Command not found. Type 'help' to list the available commands");
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Failure:");
                Console.WriteLine(e.Message);
            }
        }

        private static void ListCommands()
        {
            var commandDictionary = new Dictionary<string, string>()
            {
                {"add or create", "Displays all the meetings so far created" },
                {"remove or delete", "Removes a created meeting" },
                {"add person", "Adds a person to a specific meeting" },
                {"remove-person", "Removes a person from a meeting" },
                {"add-person", "Adds a person to a meeting" },
                {"filter", "Filters the list according to the specifie property and value" },
                {"exit", "Exits the program" }
            };

            foreach (var (command, description) in commandDictionary)
            {
                Console.WriteLine($"{command} - {description}");
            }
        }

        private static void ListMeetings()
        {
            Console.WriteLine("Listing meetings...");
            var header = string.Format("| {0, -7} | {1,20} | {2,15} | {3,25} | {4,10} | {5,7} | {6,12} | {7,12} |",
                "ID", "Name", "resp. person ID" , "Description", "Category", "Type", "Start date", "End date");
            Console.WriteLine(header);
            foreach (var meeting in _meetingService.ListMeetings())
            {
                Console.WriteLine(meeting.ToString());
            }
        }

        private static void CreateMeeting()
        {
            Console.WriteLine("Write the required details");
            Console.WriteLine("Enter a unique meeting ID: ");
            var id = int.Parse(Console.ReadLine());

            Console.WriteLine("Name of the meeting: ");
            var name = Console.ReadLine();

            Console.WriteLine("Enter your ID: ");
            var personId = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter your name: ");
            var personName = Console.ReadLine();

            Console.WriteLine("Write a description: ");
            var description = Console.ReadLine();

            Console.WriteLine("Choose a category between as a number: CodeMonkey (0), Hub (1), Short (2), Teambuilding(3)");
            var category = (Category)Enum.Parse(typeof(Category), Console.ReadLine());

            Console.WriteLine("Choose a type between as a number: Live(0), InPerson(1)");
            var type = (Models.enums.Type)Enum.Parse(typeof(Models.enums.Type), Console.ReadLine());

            Console.WriteLine("Enter starting date in format yyyy/MM/dd: ");
            var startDate = DateTime.ParseExact(Console.ReadLine(), "yyyy/MM/dd", CultureInfo.InvariantCulture);

            Console.WriteLine("Enter ending date in format yyyy/MM/dd: "); ;
            var endDate = DateTime.ParseExact(Console.ReadLine(), "yyyy/MM/dd", CultureInfo.InvariantCulture);

            Person person = new Person(personId, personName);
            Meeting meeting = new Meeting(id, name, personId, description, category, type, startDate, endDate);
            meeting.Persons.Add(person);
            var result = _meetingService.CreateMeeting(meeting);

            Console.WriteLine($"A meeting with the id {id} has been created successfully");
        }

        private static void DeleteMeeting()
        {
            Console.WriteLine("Enter the ID of the meeting you want to delete");
            var id = int.Parse(Console.ReadLine());

            //Console.WriteLine("Enter the your name");
            //var name = Console.ReadLine();

            Console.WriteLine("Enter your ID in the meeting");
            var personId = int.Parse(Console.ReadLine());

            if (_meetingService.DeleteMeeting(id, personId))
            {
                Console.WriteLine($"Meeting with the id {id} has been removed");
            }
        }

        private static void RemovePersonFromMeeting()
        {
            Console.WriteLine("Enter the ID of the meeting who'se members you want to edit");
            var id = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter the ID of person you want to remove");
            var personId = int.Parse(Console.ReadLine());

            if (_meetingService.RemovePersonFromMeeting(id, personId))
            {
                Console.WriteLine($"Person with the ID {personId} has been removed from meeting {id}");
            }
        }

        private static void AddPersonToMeeting()
        {
            Console.WriteLine("Enter the ID of the meeting who'se members you want to edit");
            var id = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter the ID of the person you want to add");
            var personId = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter the name of the person you want to add");
            var name = Console.ReadLine();

            if (_meetingService.AddPersonToMeeting(id, personId, name))
            {
                Console.WriteLine($"{name} with the id {personId} has been added to the meeting with ID {id}");
            }
        }

        private static void FilterMeetings()
        {
            Console.WriteLine("You can filter the list by these options");

            string property = "responsible, description, max-people, min-people, type, category, start-from, end-to, date-between, none";
            Console.WriteLine(property);

            string choice = Console.ReadLine();
            string value = "";

            switch (choice.ToLower().TrimEnd())
            {
                case "responsible":
                    Console.WriteLine("Write the person's who is responsible ID");
                    value = Console.ReadLine();
                    break;
                case "category":
                    Console.WriteLine("Write the category as a number: CodeMonkey (0), Hub (1), Short (2), Teambuilding(3)");
                    value = Console.ReadLine();
                    break;
                case "type":
                    Console.WriteLine("Write the category as a number: Live(0), InPerson(1)");
                    value = Console.ReadLine();
                    break;
                case "max-people":
                    Console.WriteLine("Choose the maximum amount of people");
                    value = Console.ReadLine();
                    break;
                case "min-people":
                    Console.WriteLine("Choose the minimum amount of people");
                    value = Console.ReadLine();
                    break;
                case "description":
                    Console.WriteLine("Write a part of the description");
                    value = Console.ReadLine();
                    break;
                case "start-from":
                    Console.WriteLine("Enter the minimum date in format yyyy/MM/dd:");
                    value = Console.ReadLine();
                    break;
                case "end-to":
                    Console.WriteLine("Enter the maximum date in format yyyy/MM/dd:");
                    value = Console.ReadLine();
                    break;
                case "date-between":
                    Console.WriteLine("Enter the dates in format yyyy/MM/dd yyyy/MM/dd:");
                    value = Console.ReadLine();
                    break;
                default:
                    Console.WriteLine("This option does not exist(yet)");
                    break;
            }

            var header = string.Format("| {0, -7} | {1,20} | {2,15} | {3,25} | {4,10} | {5,7} | {6,12} | {7,12} |",
    "ID", "Name", "resp. person ID", "Description", "Category", "Type", "Start date", "End date");
            Console.WriteLine(header);
            foreach (var meeting in _meetingService.ListMeetings(choice, value))
            {
                Console.WriteLine(meeting.ToString());
            }
        }
    }
}
