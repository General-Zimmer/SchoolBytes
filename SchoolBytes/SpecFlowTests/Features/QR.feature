Feature: QR

@QRTag1
Scenario: ID for check in changes when new QR Code is generated
	Given I have access to the Generation Webpage
	When I generate a new QR Code
	Then the ID for Attendance Check In should change

Scenario: Attendance changes when checking in
	Given I have a valid registration
	When I check in
	Then the attendance for my registration online should change to true