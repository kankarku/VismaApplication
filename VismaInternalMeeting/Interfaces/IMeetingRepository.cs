using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VismaInternalMeeting.Models;

namespace VismaInternalMeeting.Interfaces
{
    public interface IMeetingRepository
    {
        public int AddMeetingToDB(Meeting meeting);
        public void DeleteMeetingFromDB(int id);
        public void AddPersonToMeetingDB(int id, Person person);
        public void RemovePersonFromMeetingDB(int id, int personId);
        public IEnumerable<Meeting> GetMeetingsFromDB();
    }
}
