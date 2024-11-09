using System;
using TechTalk.SpecFlow;
using SchoolBytes.Models;

namespace SpecFlowTests.StepDefinitions
{

    [Binding]
    public class TestDBStepDefinitions
    {
        private string _courseName;
        DBConnection db = DBConnection.getDBContext();

        [Given(@"a course named '(.*)'")]
        public void GivenACourseNamed(string courseName)
        {
            _courseName = courseName;
        }

        [When(@"saved to database")]
        public void WhenSavedToDatabase()
        {
            Course newCourse = new Course();
            newCourse.Name = _courseName;
            db.courses.Add(newCourse);
            db.SaveChanges();
        }

        [Then(@"it should be found in database")]
        public void ThenItShouldBeFoundInDatabase()
        {

            db.courses.Find(70).Name.Should().Be("asd");

        }
    }
}
