using SchoolBytes.Models;
using static SchoolBytes.util.VentelisteUtil;
using System;
using TechTalk.SpecFlow;

namespace SpecFlowTests.StepDefinitions
{
    [Binding]
    public class VentelisteStepDefinitions
    {
        private static DBConnection _context = DBConnection.getDBContext();
        private static Participant bob = new Participant("Bob", "69695512");
        private static Teacher teacher1 = new Teacher() { Name = "teacher1", Id = 55435 };
        private static Course course1 = new Course() { Name = "Course1", Id = 666, Teacher = teacher1 };
        private static CourseModule cm1 = new CourseModule() { Name = "cm1", Id = 888, Teacher = teacher1, Course = course1, StartTime = DateTime.Now.AddDays(1), Capacity = 5, MaxCapacity = 5 };
        private static CourseModule cm2 = new CourseModule() { Name = "cm2", Id = 123132232, Teacher = teacher1, Course = course1, StartTime = DateTime.Now.AddDays(1), Capacity = 10, MaxCapacity = 10 };
        private static CourseModule cm3 = new CourseModule() { Name = "cm3", Id = 423524, Teacher = teacher1, Course = course1, StartTime = DateTime.Now.AddDays(1), Capacity = 5, MaxCapacity = 10 };
        private static CourseModule cm4 = new CourseModule() { Name = "cm4", Id = 68368833, Teacher = teacher1, Course = course1, StartTime = DateTime.Now.AddDays(1), Capacity = 9, MaxCapacity = 10 };
        private static Participant jim = new Participant("Jim", "90785634") { Id = 47284 };

        [BeforeFeature]
        public static void BeforeTilmeldingFeature(FeatureContext featureContext)
        {
            _context.Add(bob);
            _context.Add(jim);
            _context.Add(teacher1);
            _context.Add(course1);
            _context.Add(cm1);
            _context.Add(cm2);
            _context.Add(cm3);
            _context.Add(cm4);

            _context.SaveChanges();
        }

        [AfterFeature]
        public static void AfterTilmeldingFeature(FeatureContext featureContext)
        {
            _context.Remove(_context.courseModules.Find(cm1.Id));
            _context.Remove(_context.courseModules.Find(cm2.Id));
            _context.Remove(_context.courseModules.Find(cm3.Id));
            _context.Remove(_context.courseModules.Find(cm4.Id));
            _context.Remove(_context.courses.Find(course1.Id));
            _context.Remove(_context.teachers.Find(teacher1.Id));
            _context.Remove(_context.participants.Find(bob.Id));
            _context.Remove(_context.participants.Find(jim.Id));

            _context.SaveChanges();
        }


        [Given(@"Participant Bob, et CourseModule cm og venteliste på cm med anden participant")]
        public void GivenParticipantBobEtCourseModuleCmOgVentelistePaCmMedAndenParticipant()
        {
            // se statiske felter og BeforeScenario øverst
            cm1.Waitlist.AddLast(new WaitRegistration(jim, cm1));
        }

        [When(@"Bob bliver tilmeldt ventelisten")]
        public void WhenBobBliverTilmeldtVentelisten()
        {
            AddToWaitlist(cm1, bob);
        }

        [Then(@"Bob bliver tilmeldt ventelisten og relationerne findes i DB")]
        public void ThenBobBliverTilmeldtVentelistenOgRelationerneFindesIDB()
        {
            int counter = 0;
            foreach (var wait in cm1.Waitlist)
            {
                if (wait.participant.PhoneNumber.Equals(bob.PhoneNumber))
                {
                    counter++;
                }
            }
            counter.Should().Be(1);

            WaitRegistration theRegistration = _context.courseModules.Find(cm1.Id).Waitlist.Last();
            theRegistration.participant.Id.Should().Be(bob.Id);
            theRegistration.CourseModule.Id.Should().Be(cm1.Id);

            // cleanup
            cm1.Waitlist.RemoveLast();
        }

        [Given(@"Participant Bob der er tilmeldt ventelisten")]
        public void GivenParticipantBobDerErTilmeldtVentelisten()
        {
            // se statiske felter og BeforeScenario øverst
            cm1.Waitlist.AddLast(new WaitRegistration(bob, cm1));
        }

        [When(@"Bob vil tilmelde sig ventelisten igen")]
        public void WhenBobVilTilmeldeSigVentelistenIgen()
        {
            AddToWaitlist(cm1, bob);
        }

        [Then(@"Bliver den nye registration ikke oprettet i DB")]
        public void ThenBliverDenNyeRegistrationIkkeOprettetIDB()
        {
            int counter = 0;
            foreach (var wait in cm1.Waitlist)
            {
                if (wait.participant.PhoneNumber.Equals(bob.PhoneNumber))
                {
                    counter++;
                }
            }
            counter.Should().Be(1);

            // cleanup
            cm1.Waitlist.RemoveLast();
        }

        [Given(@"Participant Bob og et cm der er fuld booket")]
        public void GivenParticipantBobOgEtCmDerErFuldBooket()
        {
            // se statiske felter og BeforeScenario øverst
        }

        [When(@"Bob vil blive tilmeldt cm")]
        public void WhenBobVilBliveTilmeldtCm()
        {
            testScenario3(cm2, bob);
        }

        [Then(@"Bob bliver tilmeldt ventelisten og står som den første")]
        public void ThenBobBliverTilmeldtVentelistenOgStarSomDenForste()
        {
            cm2.Waitlist.First().participant.Should().Be(bob);
        }

        [Given(@"Participant Bob og cm med ledige pladser")]
        public void GivenParticipantBobOgCmMedLedigePladser()
        {
            // se statiske felter og BeforeScenario øverst
        }

        [When(@"Bob vil gerne tilmelde sig cm")]
        public void WhenBobVilGerneTilmeldeSigCm()
        {
            testScenario3(cm3, bob);
        }

        [Then(@"Bob bliver ikke tilmeldt ventelisten men cm")]
        public void ThenBobBliverIkkeTilmeldtVentelistenMenCm()
        {
            cm3.Registrations.Last().participant.Should().Be(bob);
            cm3.Waitlist.Should().BeEmpty();
        }

        [Given(@"Participant Bob, der allerede er på ventelisten")]
        public void GivenParticipantBobDerAlleredeErPaVentelisten()
        {
            cm4.Registrations.Add(new Registration(jim, cm4));
            cm4.Waitlist.AddFirst(new WaitRegistration(bob, cm4));
        }

        [When(@"Der er en ledig plads")]
        public void WhenDerErEnLedigPlads()
        {
            Unsub(cm4, jim);
        }

        [Then(@"Bob er tilmeldt cm og står ikke længere på ventelisten")]
        public void ThenBobErTilmeldtCmOgStarIkkeLaengerePaVentelisten()
        {
            cm4.Registrations.Last().participant.Should().Be(bob);
            cm4.Waitlist.Should().BeEmpty();
        }
    }
}
