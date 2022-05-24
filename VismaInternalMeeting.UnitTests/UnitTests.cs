using VismaInternalMeeting.Models;
using VismaInternalMeeting.Repositories;
using VismaInternalMeeting.Interfaces;
using VismaInternalMeeting.Services;
using Moq;

namespace VismaInternalMeeting.UnitTests
{
    [TestClass]
    public class VismaInternalMeetingUnitTests
    {
        private MeetingService _service;
        private Mock<IMeetingRepository> _repository;
        private Meeting _meeting;
        private Person _person;
        private List<Person> _personList = new List<Person>();

        [TestInitialize]
        public void Setup()
        {
            _repository = new Mock<IMeetingRepository>();
            _service = new MeetingService(_repository.Object);

            _person = new Person(789, "Test");

            _personList.Add(_person);

            _meeting = new Meeting(
                400,
                "TestMeeting",
                789,
                "TestDescription",
                Models.enums.Category.Short,
                Models.enums.Type.InPerson,
                DateTime.MinValue,
                DateTime.MaxValue);

            _meeting.Persons = _personList;
        }

        [TestMethod]
        public void Given_NewMeeting_When_AddMeetingToDB_Then_ReturnsMeetingID()
        {
            _repository.Setup(repository => repository.AddMeetingToDB(_meeting)).Returns(_meeting.Id);

            var result = _service.CreateMeeting(_meeting);

            _repository.Verify(mock => mock.AddMeetingToDB(It.IsAny<Meeting>()), Times.Once());

            Assert.IsTrue(result.Equals(_meeting.Id));
        }

        [TestMethod]
        public void AddingPersonToMeeting_When_MeetingDoesntExist_Then_ThrowsException()
        {
            _repository.Setup(repository => repository.GetMeetingsFromDB()).Returns(new List<Meeting>() {});

            var exception = Assert.ThrowsException<Exception>(() => _service.AddPersonToMeeting(123, _person.Id, _person.Name));

            Assert.AreEqual($"Meeting with id 123 does not exist", exception.Message);
        }

        [TestMethod]
        public void Given_SameMeeting_When_AddToMeeting_ToDB_Then_ThrowsException()
        {
            _repository.Setup(repository => repository.GetMeetingsFromDB()).Returns(new List<Meeting>() { _meeting });

            _repository.Verify(mock => mock.AddMeetingToDB(It.IsAny<Meeting>()), Times.Never());

            var exception = Assert.ThrowsException<Exception>(() => _service.CreateMeeting(_meeting));

            Assert.AreEqual("A meeting with this ID already exists, choose a different one.", exception.Message);
        }

        [TestMethod]
        public void AddingPersonToMeeting_Returns_True()
        {
            _repository.Setup(repository => repository.GetMeetingsFromDB()).Returns(new List<Meeting>() { _meeting});

            var result = _service.AddPersonToMeeting(_meeting.Id, 999, "tester");

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void DeletingMeeting_AsResponsiblePerson_Returns_True()
        {
            _repository.Setup(repository => repository.GetMeetingsFromDB()).Returns(new List<Meeting>() { _meeting });

            var result = _service.DeleteMeeting(_meeting.Id, _person.Id);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Removing_ResponsiblePerson_From_Meeting_Throws_Exception()
        {
            _repository.Setup(repository => repository.GetMeetingsFromDB()).Returns(new List<Meeting>() { _meeting });

            var exception = Assert.ThrowsException<Exception>(() => _service.RemovePersonFromMeeting(_meeting.Id, _person.Id));

            Assert.AreEqual("This is the responsible person of this meeting and cannot be removed as such", exception.Message);
        }
        //More tests couldnt be done due to time constraints
    }
}