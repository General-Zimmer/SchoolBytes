Feature: QR

Scenario: ID for check in changes when new QR Code is generated
	Given I have access to the Generation Webpage
	When I generate a new QR Code
	Then the ID for Attendance Check In should change

Scenario: Attendance changes when checking in
	Given I have a valid registration for a course module with id 123 for course id 1 for a participant with phone number "1234567890"
	When I check in the participant with phone number "1234567890" from course id 1 and module id 123
	Then the attendance for my registration should change to true