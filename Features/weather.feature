Feature: Weather API Testing
  To ensure the weather API works correctly
  I want to perform Read operation

  @order1
  Scenario: Check current weather
    Given I have the API endpoint for checking a weather
    When I send a POST request with valid weather check details
    Then the response should contain the current weather details
