Feature: Booking API Testing
  To ensure the booking API works correctly
  I want to perform CRUD operation

  @order1
  Scenario: Create a new booking
    Given I have the API endpoint for creating a booking
    When I send a POST request with valid booking details
    Then the response should have status 200
    And the response should contain the booking details

    @order2
  Scenario: Read an existing booking
    Given I have a valid booking ID
    When I send a GET request to fetch the booking
    Then the response should have status 200
    And the response should contain the booking details

    @order3
  Scenario: Update an existing booking
    Given I have a valid booking ID and updated details
    When I send a PUT request to update the booking
    Then the response should have status 200
    And the response should reflect the updated booking details

    @order4
  Scenario: Delete an existing booking
    Given I have a valid booking ID
    When I send a DELETE request
    Then the response should have status 201

