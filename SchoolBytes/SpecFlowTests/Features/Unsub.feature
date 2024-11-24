Feature: Unsubscribing a participant from a course module

  Scenario: Successfully unsubscribe a participant from a course module
    Given a course module exists with id 123 that has a participant with phone number "1234567890"
    When I unsubscribe the participant with phone number "1234567890" from course id 1 and module id 123
    Then the participant with phone number "1234567890" should no longer be registered in the course module
    And no error should be returned

  Scenario: Attempt to unsubscribe a participant not registered in a course module
    Given a course module exists with id 123 that does not have a participant with phone number "9876543210"
    When I unsubscribe the participant with phone number "9876543210" from course id 1 and module id 123
    Then an error with status code 400 and message "No registration found for the course with the phone number." should be returned

  Scenario: Unsubscribing from a course module allows waiting list participants to be added (future feature)
    Given a course module exists with id 123 with a waiting list participant
    And the module has a participant with phone number "1234567890"
    When I unsubscribe the participant with phone number "1234567890" from course id 1 and module id 123
    Then the participant from the waiting list should be added to the course module
