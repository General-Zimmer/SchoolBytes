Feature: CancelCourseModule

A short summary of the feature

@tagCancel
Scenario: A CourseModule bliver aflyst
	Given CourseModule cm, der ligger i fremtiden
	When Admin vil aflyse CourseModule
	Then CourseModule bliver aflyst

Scenario: B CourseModule ikke bliver aflyst
	Given CourseModule cm, der ligger i fortiden
	When Admin prøver at aflyse CourseModule
	Then CourseModule bliver ikke aflyst
