using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VismaInternalMeeting.Interfaces;
using VismaInternalMeeting.Models;
using VismaInternalMeeting.Repositories;
using VismaInternalMeeting.Models.enums;

namespace VismaInternalMeeting.Services
{
    public class MeetingService : IMeetingService
    {
        private IMeetingRepository _meetingRepository;
        private List<Meeting> _meets => ListMeetings().ToList();

        public MeetingService(IMeetingRepository repository = null)
        {
            _meetingRepository = repository ?? new MeetingRepository();
        }
        public bool AddPersonToMeeting(int id, int personId, string name)
        {
            var meeting = _meetingRepository.GetMeetingsFromDB().FirstOrDefault(x => x.Id == id);

            if (meeting == null)
            {
                throw new Exception($"Meeting with id {id} does not exist");
                return false;
            }

            Person person = new Person(personId, name);
            _meetingRepository.AddPersonToMeetingDB(id, person);
            return true;
        }

        public int CreateMeeting(Meeting meeting)
        {
            if (_meets.Any(x => x.Id.Equals(meeting.Id)))
            {
                throw new Exception("A meeting with this ID already exists, choose a different one.");
            }

            return _meetingRepository.AddMeetingToDB(meeting);
        }

        public bool DeleteMeeting(int id, int personId)
        {
            var meeting = _meetingRepository.GetMeetingsFromDB().FirstOrDefault(x => x.Id == id);

            if (meeting == null)
            {
                throw new Exception($"Meeting with ID {id} does not exist");
                return false;
            }

            if (meeting.ResponsiblePerson != personId)
            {
                throw new Exception("You are not the responsible person of this meeting");
            }

            _meetingRepository.DeleteMeetingFromDB(id);
            return true;
        }

        public List<Meeting> ListMeetings()
        {
            var meetings = _meetingRepository.GetMeetingsFromDB();

            return meetings.ToList();
        }

        public List<Meeting> ListMeetings(string property, string value)
        {
            var meetings = _meetingRepository.GetMeetingsFromDB();

            if (property != "none") 
            {
                switch (property.ToLower().TrimEnd())
                {
                    case "responsible":
                        meetings = meetings.AsQueryable().Where(x => x.ResponsiblePerson == int.Parse(value));
                        break;
                    case "category":
                        meetings = meetings.AsQueryable().Where(x => x.Category == (Category)Enum.Parse(typeof(Category), value));
                        break;
                    case "type":
                        meetings = meetings.AsQueryable().Where(x => x.Type == (Models.enums.Type)Enum.Parse(typeof(Models.enums.Type), value));
                        break;
                    case "max-people":
                        meetings = meetings.AsQueryable().Where(x => x.Persons.Count() <= int.Parse(value));
                        break;
                    case "min-people":
                        meetings = meetings.AsQueryable().Where(x => x.Persons.Count() >= int.Parse(value));
                        break;
                    case "description":
                        meetings = meetings.AsQueryable().Where(x => x.Description.ToLower().Contains(value.ToLower()));
                        break;
                    case "start-from":
                        meetings = meetings.AsQueryable().Where(x => x.StartDate >= DateTime.Parse(value));
                        break;
                    case "end-to":
                        meetings = meetings.AsQueryable().Where(x => x.EndDate <= DateTime.Parse(value));
                        break;
                    case "date-between":
                        var splitDates = value.Split(" ");
                        if (splitDates.Count() != 2) Console.WriteLine("Incorrect format");
                        meetings = meetings.AsQueryable().Where(x => x.StartDate >= DateTime.Parse(splitDates[0]) && x.EndDate <= DateTime.Parse(splitDates[1]));
                        break;
                    default:
                        Console.WriteLine("This property does not exist");
                        break;
                }
            }

            return meetings.ToList();
        }

        public bool RemovePersonFromMeeting(int id, int personId)
        {
            var meeting = _meetingRepository.GetMeetingsFromDB().FirstOrDefault(x => x.Id == id);

            if (meeting == null)
            {
                throw new Exception($"Meeting with id {id} does not exist");
                return false;
            }
            
            var person = meeting.Persons.Find(x => x.Id == personId);
            if (person == null)
            {
                throw new Exception($"Person with id {id} does not exist");
                return false;
            }

            if (meeting.ResponsiblePerson == personId)
            {
                throw new Exception("This is the responsible person of this meeting and cannot be removed as such");
            }

            _meetingRepository.RemovePersonFromMeetingDB(id, personId);
            return true;
        }
    }
}
