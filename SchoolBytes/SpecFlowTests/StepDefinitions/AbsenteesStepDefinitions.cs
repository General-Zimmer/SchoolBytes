using Gherkin.CucumberMessages;
using SchoolBytes.Controllers;
using SchoolBytes.Models;
using System;
using System.Runtime.Remoting.Contexts;
using TechTalk.SpecFlow;

namespace SpecFlowTests.StepDefinitions
{

    [Binding]
    public class AbsenteesStepDefinitions
    {
        private static Random r = new Random();
        private static int rId = r.Next(10_000_000, 12_000_000);


        private static DBConnection _context = DBConnection.getDBContext();
        private static Participant bob = new Participant("Bob", "12341234") { Id = ++rId };
        private static Participant absentBob = new Participant("Absent Bob", "11223344") { Id = ++rId };
        private static Teacher teacher1 = new Teacher() { Name = "teacher1", Id = ++rId };
        private static Course course1 = new Course() { Name = "Course1", Id = ++rId, Teacher = teacher1 };

        private static CourseModule cm1 = new CourseModule() { Name = "cm1", Id = ++rId, Teacher = teacher1, Course = course1, StartTime = DateTime.Now.AddDays(-20) };
        private static CourseModule cm2 = new CourseModule() { Name = "cm2", Id = ++rId, Teacher = teacher1, Course = course1, StartTime = DateTime.Now.AddDays(-20) };
        private static CourseModule cm3 = new CourseModule() { Name = "cm3", Id = ++rId, Teacher = teacher1, Course = course1, StartTime = DateTime.Now.AddDays(-20) };

        private static CourseModule cmOutdated1 = new CourseModule() { Name = "Outdated coursemodule 1", Id = ++rId, Teacher = teacher1, Course = course1, StartTime = DateTime.Now.AddMonths(-3) };
        private static CourseModule cmOutdated2 = new CourseModule() { Name = "Outdated coursemodule 2", Id = ++rId, Teacher = teacher1, Course = course1, StartTime = DateTime.Now.AddMonths(-3) };
        private static CourseModule cmOutdated3 = new CourseModule() { Name = "Outdated coursemodule 3", Id = ++rId, Teacher = teacher1, Course = course1, StartTime = DateTime.Now.AddMonths(-3) };

        private static Registration r1 = new Registration() { Id = cm1.Id, participant = absentBob, CourseModule = cm1, Attendance = false };
        private static Registration r2 = new Registration() { Id = cm2.Id, participant = absentBob, CourseModule = cm2, Attendance = false };
        private static Registration r3 = new Registration() { Id = cm3.Id, participant = absentBob, CourseModule = cm3, Attendance = false };


        private static Registration r4 = new Registration() { Id = cmOutdated1.Id, participant = bob, CourseModule = cmOutdated1, Attendance = false };
        private static Registration r5 = new Registration() { Id = cmOutdated2.Id, participant = bob, CourseModule = cmOutdated2, Attendance = false };
        private static Registration r6 = new Registration() { Id = cmOutdated3.Id, participant = bob, CourseModule = cmOutdated3, Attendance = false };

        private string tempNotification = "";

        [BeforeFeature]
        public static void BeforeAbsenteesFeature(FeatureContext featureContext)
        {
            cm1.Registrations.Add(r1);
            cm2.Registrations.Add(r2);
            cm3.Registrations.Add(r3);

            cmOutdated1.Registrations.Add(r4);
            cmOutdated2.Registrations.Add(r5);
            cmOutdated3.Registrations.Add(r6);

            _context.Add(bob);
            _context.Add(absentBob);
            _context.Add(teacher1);
            _context.Add(course1);
            _context.Add(cm1);
            _context.Add(cm2);
            _context.Add(cm3);
            _context.Add(cmOutdated1);
            _context.Add(cmOutdated2);
            _context.Add(cmOutdated3);

            _context.SaveChanges();
        }

        [AfterFeature]
        public static void AfterAbsenteesFeature(FeatureContext featureContext)
        {
            _context.Remove(cmOutdated1);
            _context.Remove(cmOutdated2);
            _context.Remove(cmOutdated3);
            _context.Remove(cm1);
            _context.Remove(cm2);
            _context.Remove(cm3);
            _context.Remove(course1);
            _context.Remove(teacher1);
            _context.Remove(absentBob);
            _context.Remove(bob);

            _context.SaveChanges();
        }


        [Given(@"Participant med 3 udeblivelser")]
        public void GivenParticipantMedUdeblivelser()
        {
            //Se statiske felter og BeforeFeature
        }

        [When(@"A Notifikationer hentes fra HomeController")]
        public void WhenANotifikationerHentesFraHomeController()
        {
            tempNotification = DBConnection.CheckNotificationsTest();
        }

        [Then(@"Findes Participants navn i stringen")]
        public void ThenFindesParticipantsNavnIStringen()
        {
            tempNotification.Contains("absentBob").Should().BeTrue();
        }

        [Given(@"Participant med 3 gamle udeblivelser")]
        public void GivenParticipantMedGameUdeblivelser()
        {
            //Se statiske felter og BeforeFeature
        }

        [When(@"B Notifikationer hentes fra HomeController")]
        public void WhenBNotifikationerHentesFraHomeController()
        {
            tempNotification = DBConnection.CheckNotificationsTest();
        }

        [Then(@"Findes Participants navn ikke i stringen")]
        public void ThenFindesParticipantsNavnIkkeIStringen()
        {
            tempNotification.Contains(" Bob ").Should().BeFalse();
        }
    }
}
