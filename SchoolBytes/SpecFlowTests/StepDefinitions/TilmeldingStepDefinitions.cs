using SchoolBytes.Models;
using System;
using System.Runtime.Remoting.Contexts;
using TechTalk.SpecFlow;

namespace SpecFlowTests.StepDefinitions
{
    [Binding]
    public class TilmeldingStepDefinitions
    {

        private static DBConnection _context = DBConnection.getDBContext();
        private static Participant bob = new Participant("Bob", "69695512");
        private static Teacher teacher1 = new Teacher() { Name = "teacher1", Id = 55435 };
        private static Course course1 = new Course() { Name = "Course1", Id = 4235252, Teacher = teacher1 };
        private static CourseModule cm1 = new CourseModule() { Name = "cm1", Id = 423112221, Teacher = teacher1, Course = course1, StartTime = DateTime.Now.AddDays(1) };
        private static CourseModule cmOutdated = new CourseModule() { Name = "Outdated coursemodule", Id = 62343, Teacher = teacher1, Course = course1, Date = DateTime.Now.AddDays(-1), StartTime = DateTime.Now.AddDays(-1) };

        private static CourseModule cm2 = new CourseModule() { Name = "cm2", Id = 123132232, Teacher = teacher1, Course = course1, StartTime = DateTime.Now.AddDays(1) };
        private static CourseModule cm3 = new CourseModule() { Name = "cm3", Id = 423524, Teacher = teacher1, Course = course1, StartTime = DateTime.Now.AddDays(1) };
        private static CourseModule cm4 = new CourseModule() { Name = "cm4", Id = 68368833, Teacher = teacher1, Course = course1, StartTime = DateTime.Now.AddDays(1) };
        private static CourseModule cm5 = new CourseModule() { Name = "cm5", Id = 345372, Teacher = teacher1, Course = course1, StartTime = DateTime.Now.AddDays(1) };
        private static CourseModule cm6 = new CourseModule() { Name = "cm6", Id = 323411114, Teacher = teacher1, Course = course1, StartTime = DateTime.Now.AddDays(1) };

        private static Participant bobby = new Participant("Bobby", "A69695512");
        private static Participant bob2 = new Participant("Bob2", "69695512") { Id = 47284 };

        [BeforeFeature]
        public static void BeforeTilmeldingFeature(FeatureContext featureContext)
        {
            _context.Add(bob);
            _context.Add(bobby);
            //_context.Add(bob2);  Ideen er at bob2 aldrig oprettes i db'en da hans tlf nummer er brugt
            _context.Add(teacher1);
            _context.Add(course1);
            _context.Add(cm1);
            _context.Add(cm2);
            _context.Add(cm3);
            _context.Add(cm4);
            _context.Add(cm5);
            _context.Add(cm6);
            _context.Add(cmOutdated);

            _context.SaveChanges();
        }

        [AfterFeature]
        public static void AfterTilmeldingFeature(FeatureContext featureContext)
        {
            _context.Remove(_context.courseModules.Find(cm1.Id));
            _context.Remove(_context.courseModules.Find(cm2.Id));
            _context.Remove(_context.courseModules.Find(cm3.Id));
            _context.Remove(_context.courseModules.Find(cm4.Id));
            _context.Remove(_context.courseModules.Find(cm5.Id));
            _context.Remove(_context.courseModules.Find(cm6.Id));
            _context.Remove(_context.courseModules.Find(cmOutdated.Id));
            _context.Remove(_context.courses.Find(course1.Id));
            _context.Remove(_context.teachers.Find(teacher1.Id));
            _context.Remove(_context.participants.Find(bob.Id));
            _context.Remove(_context.participants.Find(bobby.Id));
            _context.Remove(_context.participants.Find(bob2.Id));

            _context.SaveChanges();
        }


        [Given(@"Participant Bob og et CourseModule cm1")]
        public void GivenParticipantBobOgEtCourseModuleCm1()
        {
            //Se statiske felter og BeforeFeature øverst
        }

        [When(@"Bob bliver tilmeldt cm1")]
        public void WhenBobBliverTilmeldtCm1()
        {
            DBConnection.SubscribeTest(course1.Id, cm1.Id, bob);
        }

        [Then(@"Er Bob tilmeldt og relationerne findes i DB")]
        public void ThenErBobTilmeldtOgRelationerneFindesIDB()
        {
            DBConnection.GetSubscribeCount(bob).Should().Be(1);
            Registration theRegistration = _context.courseModules.Find(cm1.Id).Registrations[0];
            theRegistration.participant.Id.Should().Be(bob.Id);
            theRegistration.CourseModule.Id.Should().Be(cm1.Id);
        }

        [Given(@"Participant Bob der er tilmeldt CourseModule cm1")]
        public void GivenParticipantBobDerErTilmeldtCourseModuleCm1()
        {
            //Se statiske felter og BeforeFeature øverst
        }

        [When(@"Bob bliver tilmeldt cm1 igen")]
        public void WhenBobBliverTilmeldtCm1Igen()
        {
            DBConnection.SubscribeTest(course1.Id, cm1.Id, bob);
        }

        [Then(@"Er der kun en registration i DB")]
        public void ThenErDerKunEnRegistrationIDB()
        {
            DBConnection.GetSubscribeCount(bob).Should().Be(1);
        }

        [Given(@"Participant Bob og et CourseModule med forbigået dato")]
        public void GivenParticipantBobOgEtCourseModuleMedForbigaetDato()
        {
            //Se statiske felter og BeforeFeature øverst
        }

        [When(@"Bob bliver tilmeldt A")]
        public void WhenBobBliverTilmeldtA()
        {
            DBConnection.SubscribeTest(course1.Id, cmOutdated.Id, bob);
        }

        [Then(@"Er der ikke en ny registration i DB A")]
        public void ThenErDerIkkeEnNyRegistrationIDBA()
        {
            _context.courseModules.Find(cmOutdated.Id).Registrations.Count().Should().Be(0);
        }

        [Given(@"Participant Bob med 5 aktive tilmeldte kurser")]
        public void GivenParticipantBobMed5AktiveTilmeldteKurser()
        {
            DBConnection.SubscribeTest(course1.Id, cm2.Id, bob);
            DBConnection.SubscribeTest(course1.Id, cm3.Id, bob);
            DBConnection.SubscribeTest(course1.Id, cm4.Id, bob);
            DBConnection.SubscribeTest(course1.Id, cm5.Id, bob);
        }

        [When(@"Bob bliver tilmeldt B")]
        public void WhenBobBliverTilmeldtB()
        {
            DBConnection.SubscribeTest(course1.Id, cm6.Id, bob);
        }

        [Then(@"Er der ikke en ny registration i DB B")]
        public void ThenErDerIkkeEnNyRegistrationIDBB()
        {
            _context.courseModules.Find(cm6.Id).Registrations.Count().Should().Be(0);
            DBConnection.GetSubscribeCount(bob).Should().Be(5);
        }

        [Given(@"Particpant Bobby med forkert format på properties")]
        public void GivenParticpantBobbyMedForkertFormatPaProperties()
        {
            //Se statiske felter og BeforeFeature øverst
        }

        [When(@"Bobby bliver tilmeldt")]
        public void WhenBobbyBliverTilmeldt()
        {
            DBConnection.SubscribeTest(course1.Id, cm6.Id, bobby);
        }

        [Then(@"Er der ikk en ny registration i DB C")]
        public void ThenErDerIkkEnNyRegistrationIDBC()
        {
            _context.courseModules.Find(cm6.Id).Registrations.Count().Should().Be(0);
            DBConnection.GetSubscribeCount(bobby).Should().Be(0);

        }

        [Given(@"Bob2 med samme telefonnummer som Bob")]
        public void GivenBob2MedSammeTelefonnummerSomBob()
        {
            //Se statiske felter og BeforeFeature øverst
        }

        [When(@"Bob2 bliver tilmeldt")]
        public void WhenBob2BliverTilmeldt()
        {
            DBConnection.SubscribeTest(course1.Id, cm6.Id, bob2);
        }

        [Then(@"Er der hverken en ny registration eller ny participant")]
        public void ThenErDerHverkenEnNyRegistrationEllerNyParticipant()
        {
            _context.courseModules.Find(cm6.Id).Registrations.Count().Should().Be(0);
            DBConnection.GetSubscribeCount(bob2).Should().Be(0);
            _context.participants.Find(bob2.Id).Should().Be(null);
        }
    }
}
