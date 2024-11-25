Feature: RemoveParticipantFromCourse

A short summary of the feature

@tag1
Scenario: [Remove Participant From Course]
	Given [Participant is enrolled to course]
	When [When user removes participant from course]
	Then [Participant should be removed from the list in course]
	Then [Show error message that partcipant is not in course anymore]
