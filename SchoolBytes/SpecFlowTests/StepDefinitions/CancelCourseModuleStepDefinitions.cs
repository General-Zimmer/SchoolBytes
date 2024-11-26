using SchoolBytes.Models;
using static SchoolBytes.util.VentelisteUtil;
using System;
using TechTalk.SpecFlow;

namespace SpecFlowTests.StepDefinitions
{
    [Binding]
    public class CancelCourseModuleStepDefinitions
    {
        private static DBConnection _context = DBConnection.getDBContext();
        private static Teacher teacher1 = new Teacher() { Name = "teacher1", Id = 55435 };
        private static Course course1 = new Course() { Name = "Course1", Id = 666, Teacher = teacher1 };
        private static CourseModule cm1 = new CourseModule() { Name = "cm1", Id = 888, Teacher = teacher1, Course = course1, StartTime = DateTime.Now.AddDays(1), Capacity = 5, MaxCapacity = 5 };
        private static CourseModule cm2 = new CourseModule() { Name = "cm2", Id = 123132232, Teacher = teacher1, Course = course1, StartTime = DateTime.Now.AddDays(-1), Capacity = 10, MaxCapacity = 10 };

        [BeforeFeature]
        public static void BeforeTilmeldingFeature(FeatureContext featureContext)
        {
            _context.Add(teacher1);
            _context.Add(course1);
            _context.Add(cm1);
            _context.Add(cm2);

            _context.SaveChanges();
        }

        [AfterFeature]
        public static void AfterTilmeldingFeature(FeatureContext featureContext)
        {
            _context.Remove(_context.courseModules.Find(cm1.Id));
            _context.Remove(_context.courseModules.Find(cm2.Id));
            _context.Remove(_context.courses.Find(course1.Id));
            _context.Remove(_context.teachers.Find(teacher1.Id));

            _context.SaveChanges();
        }

        [Given(@"CourseModule cm, der ligger i fremtiden")]
        public void GivenCourseModuleCmDerLiggerIFremtiden()
        {
            // se statiske felter og BeforeScenario øverst
        }

        [When(@"Admin vil aflyse CourseModule")]
        public void WhenAdminVilAflyseCourseModule()
        {
            _context.CancelModule(cm1);
        }

        [Then(@"CourseModule bliver aflyst")]
        public void ThenCourseModuleBliverAflyst()
        {
            _context.courseModules.Find(cm1.Id).IsCancelled.Should().BeTrue();
        }

        [Given(@"CourseModule cm, der ligger i fortiden")]
        public void GivenCourseModuleCmDerLiggerIFortiden()
        {
            // se statiske felter og BeforeScenario øverst
        }

        [When(@"Admin prøver at aflyse CourseModule")]
        public void WhenAdminProverAtAflyseCourseModule()
        {
            _context.CancelModule(cm2);
        }

        [Then(@"CourseModule bliver ikke aflyst")]
        public void ThenCourseModuleBliverIkkeAflyst()
        {
            _context.courseModules.Find(cm2.Id).IsCancelled.Should().BeFalse();
        }
    }
}
