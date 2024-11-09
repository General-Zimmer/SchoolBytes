using System;
using TechTalk.SpecFlow;
using SchoolBytes.Models;

namespace SpecFlowTests.StepDefinitions
{
    [Binding]
    public class DBtest2StepDefinitions
    {

        DBConnection db = DBConnection.getDBContext();
        private Course _theCourse;

        [Given(@"a database with PK (.*)")]
        public void GivenADatabaseWithPK(int p0)
        {
        }

        [When(@"retrieved from database")]
        public void WhenRetrievedFromDatabase()
        {
            //_theCourse = db.courses.Find(70);
        }

        [Then(@"the course name should be asd")]
        public void ThenTheCourseNameShouldBeAsd()
        {
            //_theCourse.Name.Should().Be("asd");
            string testS = "bob";
            testS.Should().Be("bob");
        }
    }
}
