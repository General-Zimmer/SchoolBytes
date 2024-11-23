using SchoolBytes.Models;
using System;
using TechTalk.SpecFlow;

namespace SpecFlowTests.StepDefinitions
{
    [Binding]
    public class TilmeldingStepDefinitions
    {

        private static DBConnection _context = DBConnection.getDBContext();
        private static Participant bob = new Participant("Bob", "69695512");
        private static Teacher teacher1 = new Teacher() { Name = "teacher1", Id = 55435 };
        private static Course course1 = new Course() { Name = "Course1", Id = 666, Teacher = teacher1 };
        private static CourseModule cm1 = new CourseModule() { Name = "cm1", Id = 888, Teacher = teacher1, Course = course1 };

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


        [Given(@"Participant Bob og et CourseModule cm1")]
        public void GivenParticipantBobOgEtCourseModuleCm1()
        {
            //Se statiske felter og BeforeFeature øverst
        }

        [When(@"Bob bliver tilmeldt cm1")]
        public void WhenBobBliverTilmeldtCm1()
        {
            DBConnection.SubscribeTest(666, 888, bob);
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
            throw new PendingStepException();
        }

        [When(@"Bob bliver tilmeldt cm1 igen")]
        public void WhenBobBliverTilmeldtCm1Igen()
        {
            throw new PendingStepException();
        }

        [Then(@"Er der kun en registration i DB")]
        public void ThenErDerKunEnRegistrationIDB()
        {
            throw new PendingStepException();
        }

        [Given(@"Participant Bob og et CourseModule med forbigået dato")]
        public void GivenParticipantBobOgEtCourseModuleMedForbigaetDato()
        {
            throw new PendingStepException();
        }

        [When(@"Bob bliver tilmeldt A")]
        public void WhenBobBliverTilmeldtA()
        {
            throw new PendingStepException();
        }

        [Then(@"Er der ikke en ny registration i DB A")]
        public void ThenErDerIkkeEnNyRegistrationIDBA()
        {
            throw new PendingStepException();
        }

        [Given(@"Participant Bob med 5 aktive tilmeldte kurser")]
        public void GivenParticipantBobMed5AktiveTilmeldteKurser()
        {
            throw new PendingStepException();
        }

        [When(@"Bob bliver tilmeldt B")]
        public void WhenBobBliverTilmeldtB()
        {
            throw new PendingStepException();
        }

        [Then(@"Er der ikke en ny registration i DB B")]
        public void ThenErDerIkkeEnNyRegistrationIDBB()
        {
            throw new PendingStepException();
        }

        [Given(@"Particpant Bobby med forkert format på properties")]
        public void GivenParticpantBobbyMedForkertFormatPaProperties()
        {
            throw new PendingStepException();
        }

        [When(@"Bobby bliver tilmeldt")]
        public void WhenBobbyBliverTilmeldt()
        {
            throw new PendingStepException();
        }

        [Then(@"Er der ikk en ny registration i DB C")]
        public void ThenErDerIkkEnNyRegistrationIDBC()
        {
            throw new PendingStepException();
        }

        [Given(@"Bob2 med samme telefonnummer som Bob")]
        public void GivenBob2MedSammeTelefonnummerSomBob()
        {
            throw new PendingStepException();
        }

        [When(@"Bob2 bliver tilmeldt")]
        public void WhenBob2BliverTilmeldt()
        {
            throw new PendingStepException();
        }

        [Then(@"Er der hverken en ny registration eller ny participant")]
        public void ThenErDerHverkenEnNyRegistrationEllerNyParticipant()
        {
            throw new PendingStepException();
        }
    }
}
