using System;

namespace VismaInternalMeeting
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("----------------------------------------------");
            Console.WriteLine("Welcome to the Visma internal meeting manager");
            Console.WriteLine("Type 'help' to see list of commands");
            Console.WriteLine("----------------------------------------------");

            while (true)
            {
                var input = Console.ReadLine();
                if (input == "exit")
                {
                    Console.WriteLine("Exitting the program.");
                    Environment.Exit(0);
                }
                else
                {
                    CommandHandle.Handle(input);
                }
            }
        }
    }
}