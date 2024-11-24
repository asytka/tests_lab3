using System;
using TechTalk.SpecFlow;

namespace tests_lab3.StepDefinitions
{
    [Binding]
    public class WeatherAPITestingStepDefinitions
    {
        private readonly RestClient client = new RestClient("http://api.weatherstack.com");
        string access_key = "de1e61ad118b14e7ffd102243ac8ff9e";
        string city = "Lviv";
        private RestResponse response;
        private string jsonResponse;
        string temp;
        string wind_speed;

        [Given(@"I have the API endpoint for checking a weather")]
        public void GivenIHaveTheAPIEndpointForCheckingAWeather()
        {
            // API endpoint is already configured in the RestClient
        }

        [When(@"I send a POST request with valid weather check details")]
        public async Task WhenISendAPOSTRequestWithValidWeatherCheckDetails()
        {
            var request = new RestRequest("/current", Method.Get);
            request.AddQueryParameter("access_key", access_key).AddQueryParameter("query", city);
            response = await client.ExecuteAsync(request);

            Assert.AreEqual(200, (int)response.StatusCode, "POST request failed.");
            dynamic jsonResponse = Newtonsoft.Json.JsonConvert.DeserializeObject(response.Content);
            temp = jsonResponse.current.temperature;
            wind_speed = jsonResponse.current.wind_speed;
        }

        [Then(@"the response should contain the current weather details")]
        public void ThenTheResponseShouldContainTheCurrentWeatherDetails()
        {
            Assert.IsTrue(temp != null, "Response does not contain 'current'.");
            Assert.IsTrue(wind_speed != null, "Response does not contain 'location'.");
        }
    }
}
