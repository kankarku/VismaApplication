using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VismaInternalMeeting.Models;

namespace VismaInternalMeeting.Interfaces
{
    public interface IMeetingService
    {
        public int CreateMeeting(Meeting meeting);
        public bool DeleteMeeting(int id, int personId);
        public bool AddPersonToMeeting(int id, int personId, string name);
        public bool RemovePersonFromMeeting(int id, int personId);
        public List<Meeting> ListMeetings();
        public List<Meeting> ListMeetings(string property, string value);
    }
}
