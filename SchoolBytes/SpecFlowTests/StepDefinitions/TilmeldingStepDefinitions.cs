using System;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using SchoolBytes.Controllers;
using SchoolBytes.Models;
using TechTalk.SpecFlow;

namespace SpecFlowTests.StepDefinitions
{
    [Binding]
    public class TilmeldingStepDefinitions
    {
        private static DBConnection _context = DBConnection.getDBContext();
        private static Participant bob = new Participant("Bob", "69695512");
        private static Teacher teacher1 = new Teacher() { Name = "teacher1", Id = 55435 };
        private static Course course1 = new Course() { Name = "Course1", Id = 666, Teacher=teacher1 };
        private static CourseModule cm1 = new CourseModule() { Name="cm1", Id=888, Teacher = teacher1, Course=course1 };

        private Participant bobNonStatic = new Participant("Bob", "69695512");

        [BeforeFeature]
        public static void BeforeTilmeldingFeature(FeatureContext featureContext)
        {
            _context.Add(bob);
            _context.Add(teacher1);
            _context.Add(course1);
            _context.Add(cm1);
            _context.SaveChanges();
        }

        [AfterFeature]
        public static void AfterTilmeldingFeature(FeatureContext featureContext)
        {
            _context.Remove(_context.courseModules.Find(cm1.Id));
            _context.Remove(_context.courses.Find(course1.Id));
            _context.Remove(_context.teachers.Find(teacher1.Id));
            _context.Remove(_context.participants.Find(bob.Id));
            _context.SaveChanges();
        }

        int erBobAdded;

        [Given(@"\[Participant bob tilmeldes modul tilhørende rigtige course]")]
        public void GivenParticipantBobTilmeldesModulTilhorendeRigtigeCourse()
        {
            DBConnection.SubscribeTest(666, 888, bob);
        }

        
        [When(@"\[Oprettes en instans i databasen]")]
        public void WhenOprettesEnInstansIDatabasen()
        {
            erBobAdded = DBConnection.GetSubscribeCount(bob);
            
        }

        [Then(@"\[At bob er tilmeldt modul]")]
        public void ThenAtBobErTilmeldtModul()
        {
            erBobAdded.Should().Be(1); 
        }

        [Given(@"\[Participant kan ikke oprettes to gange]")]
        public void GivenParticipantKanIkkeOprettesToGange()
        {
            _context.courseModules.Find(cm1.Id).Registrations.Count().Should().Be(1);
            DBConnection.IsParticipantSubscribed(bobNonStatic).Should().Be(false);

        }


        [Then(@"\[At bob kun kan gave (.*) tilmeldinger]")]
        public void ThenAtBobKunKanGaveTilmeldinger(int p0)
        {
            throw new PendingStepException();
        }

        [Then(@"\[At datoer er i dag eller efter]")]
        public void ThenAtDatoerErIDagEllerEfter()
        {
            throw new PendingStepException();
        }

    }
}
