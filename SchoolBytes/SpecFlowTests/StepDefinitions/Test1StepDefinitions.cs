using System;
using TechTalk.SpecFlow;

namespace SpecFlowTests.StepDefinitions
{
    [Binding]
    public class Test1StepDefinitions
    {
        private readonly Course _course = new Course();
        private string _courseName;

        [Given(@"the course name is deez")]
        public void GivenTheCourseNameIsDeez()
        {
            _courseName = "deez";
        }

        [When(@"the course is created")]
        public void WhenTheCourseIsCreated()
        {
            _course.Name = _courseName;

        }

        [Then(@"the course name should be deez")]
        public void ThenTheCourseNameShouldBeDeez()
        {
            _course.Name.Should().Be("deez");
        }
    }
}
