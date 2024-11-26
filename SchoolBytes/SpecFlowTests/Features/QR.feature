Feature: QR

Scenario: ID for check in changes when new QR Code is generated
	Given I have access to the Generation Webpage
	When I generate a new QR Code
	Then the ID for Attendance Check In should change

Scenario: Attendance changes when checking in
	Given I have a valid registration for a course module with id 888 for course id 666 for a participant with phone number "12345678"
	When I check in the participant with phone number "12345678" from course id 666 and module id 888
	Then the attendance for my registration should change to true

Scenario: Proper error code when registration not found
	Given I have a valid course module and a phone number
	When I look for a registration with the given phonenumber
	Then I should receive the error code 404

Scenario: Proper response when attendance has already been registered
	Given a registration that has already been checked in
	When I try to check in again
	Then I should receive the status code 200 and the statusDescription "Fremmøde allerede registreret."