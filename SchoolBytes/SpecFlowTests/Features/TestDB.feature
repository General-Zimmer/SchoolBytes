Feature: TestDB

A short summary of the feature

@tag2
Scenario: TestCreateAndReadCourse
	Given a course named 'test123'
	When saved to database
	Then it should be found in database
