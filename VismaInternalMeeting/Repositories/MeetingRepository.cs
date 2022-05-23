using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using VismaInternalMeeting.Interfaces;
using VismaInternalMeeting.Models;

namespace VismaInternalMeeting.Repositories
{
    public class MeetingRepository : IMeetingRepository
    {
        private readonly string _meetingDatabase;


        public MeetingRepository(string meetingDatabaseFile = "MeetingDatabase.json")
        {
            var rootDir = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent;

            _meetingDatabase = rootDir + "/" + meetingDatabaseFile;
        }
        public void AddPersonToMeetingDB(int id, Person person)
        {
            var meetings = GetMeetingsFromDB().ToList();

            if (meetings.Count() < 1)
            {
                throw new Exception("There are no created meetings");
            }

            var filteredMeeting = meetings.Where(x => x.Id == id);
            var meeting = filteredMeeting.ElementAt(0);
            var persons = meeting.Persons;

            var duplicate = persons.Find(x => x.Id == person.Id);
            if (duplicate != null)
            {
                throw new Exception("This person is already in the meeting");
            }

            //as the exercise mentioned it should take about 4 hours, there wasn't enough time to implement to check whether a person is already in another meeting
            //var parralelMeetingPersons = new List<Meeting>(meetings);
            //int personIndex = 0;
            //foreach (var item in parralelMeetingPersons)
            //{
            //    for (int i = 0; i < parralelMeetingPersons[personIndex].Persons.Count(); i++)
            //    {
            //        if (parralelMeetingPersons[personIndex].Persons[i] == person &&
            //            (parralelMeetingPersons[personIndex].StartDate <= meeting.StartDate &&
            //            parralelMeetingPersons[personIndex].EndDate >= meeting.EndDate))
            //        {
            //            Console.WriteLine($"This persons meetings might intercept with another meeting with the ID {parralelMeetingPersons[personIndex].Id}");
            //        }
            //    }
            //    personIndex++;
            //}

            persons.Add(person);
            meeting.Persons = persons;

            int index = 0;
            var parralelMeetings = new List<Meeting>(meetings);
            foreach (var item in parralelMeetings)
            {
                if (meetings[index].Id == id)
                {
                    meetings[index] = meeting;
                }
                index++;
            }

            var jsonString = JsonSerializer.Serialize(meetings);
            File.WriteAllText(_meetingDatabase, jsonString);
        }

        public int AddMeetingToDB(Meeting meeting)
        {
            var meetings = GetMeetingsFromDB().ToList();
            meetings.Add(meeting);
            var jsonSerialized = JsonSerializer.Serialize(meetings);
            File.WriteAllText(_meetingDatabase, jsonSerialized);

            return meeting.Id;
        }

        public void DeleteMeetingFromDB(int id)
        {
            var meetings = GetMeetingsFromDB();

            if (meetings.Count() < 1)
            {
                throw new Exception("There are no created meetings");
            }

            meetings = meetings.Where(x => x.Id != id);
            var jsonString = JsonSerializer.Serialize(meetings);
            File.WriteAllText(_meetingDatabase, jsonString);
        }

        public IEnumerable<Meeting> GetMeetingsFromDB()
        {
            if (File.Exists(_meetingDatabase))
            {
                var jsonString = File.ReadAllText(_meetingDatabase);
                var result = JsonSerializer.Deserialize<IEnumerable<Meeting>>(jsonString);

                return result;
            }

            return new List<Meeting>();
        }

        public void RemovePersonFromMeetingDB(int id, int personId)
        {
            var meetings = GetMeetingsFromDB().ToList();

            if (meetings.Count() < 1)
            {
                throw new Exception("There are no created meetings");
            }
            
            var filteredMeeting = meetings.Where(x => x.Id == id);
            var meeting = filteredMeeting.ElementAt(0);
            var persons = meeting.Persons;
            var personToRemove = persons.Find(x => x.Id == personId);

            persons.Remove(personToRemove);
            meeting.Persons = persons;

            int index = 0;
            var parralelMeetings = new List<Meeting>(meetings);
            foreach (var item in parralelMeetings)
            {
                if (meetings[index].Id == id)
                {
                    meetings[index] = meeting;
                }
                index++;
            }

            var jsonString = JsonSerializer.Serialize(meetings);
            File.WriteAllText(_meetingDatabase, jsonString);
        }
    }
}
