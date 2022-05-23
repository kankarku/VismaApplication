using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VismaInternalMeeting.Models;
using VismaInternalMeeting.Models.enums;

namespace VismaInternalMeeting.Models
{
    public class Meeting
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ResponsiblePerson { get; set; }
        public string Description { get; set; }
        public enums.Category Category { get; set; }
        public enums.Type Type { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<Person> Persons { get; set; }

        public Meeting(int id, string name, int responsiblePerson, string description,
            Category category, enums.Type type, DateTime startDate, DateTime endDate)
        {
            Id = id;
            Name = name;
            ResponsiblePerson = responsiblePerson;
            Description = description;
            Category = category;
            Type = type;
            StartDate = startDate;
            EndDate = endDate;
            Persons = new List<Person>();
        }

        public override string ToString()
        {
            return string.Format("| {0,-7} | {1,20} | {2,15} | {3,25} | {4,10} | {5,7} | {6,12:yyyy-MM-dd} | {7,12:yyyy-MM-dd} |",
                this.Id, this.Name, this.ResponsiblePerson, this.Description, this.Category, this.Type, this.StartDate, this.EndDate);
        }
    }
}
