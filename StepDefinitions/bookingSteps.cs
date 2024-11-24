using NUnit.Framework;
using RestSharp;
using System.Threading.Tasks;

namespace tests_lab3.StepDefinitions
{
    [Binding]
    public sealed class BookingSteps
    {
        private readonly RestClient client = new RestClient("https://restful-booker.herokuapp.com");
        private RestResponse response;
        private static string bookingId;

        [Given(@"I have the API endpoint for creating a booking")]
        public void GivenIHaveTheAPIEndpointForCreatingABooking()
        {
            // API endpoint is already configured in the RestClient
        }

        [When(@"I send a POST request with valid booking details")]
        public async Task WhenISendAPOSTRequestWithValidBookingDetails()
        {
            var request = new RestRequest("/booking", Method.Post);

            var bookingDetails = new
            {
                firstname = "Jim",
                lastname = "Brown",
                totalprice = 111,
                depositpaid = true, // Boolean, not string
                bookingdates = new { checkin = "2018-01-01", checkout = "2019-01-01" },
                additionalneeds = "Breakfast"
            };
            var jsonBody = JsonSerializer.Serialize(bookingDetails);
            request.AddStringBody(jsonBody, ContentType.Json)
                .AddOrUpdateHeader("Content-Type", "application/json")
                .AddOrUpdateHeader("Accept", "application/json");
            response = await client.ExecuteAsync(request);

            Assert.AreEqual(200, (int)response.StatusCode, "POST request failed.");

            dynamic jsonResponse = Newtonsoft.Json.JsonConvert.DeserializeObject(response.Content);
            bookingId = jsonResponse.bookingid;

        }

        [Then(@"the response should contain the booking details")]
        public void ThenTheResponseShouldContainTheBookingDetails()
        {
            Assert.IsTrue(response.Content.Contains("firstname"), "Response does not contain 'firstname'.");
            Assert.IsTrue(response.Content.Contains("lastname"), "Response does not contain 'lastname'.");
        }

        [Given(@"I have a valid booking ID")]
        public void GivenIHaveAValidBookingID()
        {
            Assert.IsNotNull(bookingId, "Booking ID should not be null.");
        }

        [When(@"I send a GET request to fetch the booking")]
        public async Task WhenISendAGETRequestToFetchTheBooking()
        {
            var request = new RestRequest($"/booking/{bookingId}", Method.Get)
                .AddOrUpdateHeader("Content-Type", "application/json")
                .AddOrUpdateHeader("Accept", "application/json");
            response = await client.ExecuteAsync(request);

            Assert.AreEqual(200, (int)response.StatusCode, "GET request failed.");
        }

        [Then(@"the response should contain the fetched booking details")]
        public void ThenTheResponseShouldContainTheFetchedBookingDetails()
        {
            Assert.IsTrue(response.Content.Contains("John"), "Response does not contain 'John'.");
            Assert.IsTrue(response.Content.Contains("Doe"), "Response does not contain 'Doe'.");
        }

        [Given(@"I have a valid booking ID and updated details")]
        public void GivenIHaveAValidBookingIDAndUpdatedDetails()
        {
            Assert.IsNotNull(bookingId, "Booking ID should not be null.");
        }

        [When(@"I send a PUT request to update the booking")]
        public async Task WhenISendAPUTRequestToUpdateTheBooking()
        {
            var request = new RestRequest($"/booking/{1}", Method.Put)
             .AddOrUpdateHeader("Content-Type", "application/json")
             .AddOrUpdateHeader("Accept", "application/json")
             .AddOrUpdateHeader("Authorization", "Basic YWRtaW46cGFzc3dvcmQxMjM=");
            request.AddJsonBody(new
            {
                firstname = "Jane",
                lastname = "Smith",
                totalprice = 200,
                depositpaid = false,
                bookingdates = new { checkin = "2024-02-01", checkout = "2024-02-10" },
                additionalneeds = "Dinner"
            });
            response = await client.ExecuteAsync(request);

            Assert.AreEqual(200, (int)response.StatusCode, "PUT request failed.");
        }

        [Then(@"the response should reflect the updated booking details")]
        public void ThenTheResponseShouldReflectTheUpdatedBookingDetails()
        {
            Assert.IsTrue(response.Content.Contains("Jane"), "Response does not contain 'Jane'.");
            Assert.IsTrue(response.Content.Contains("Smith"), "Response does not contain 'Smith'.");
        }

        [When(@"I send a DELETE request")]
        public async Task WhenISendADELETERequest()
        {
            var request = new RestRequest("/booking", Method.Post);
            var bookingDetails = new
            {
                firstname = "Jim",
                lastname = "Brown",
                totalprice = 111,
                depositpaid = true, 
                bookingdates = new { checkin = "2018-01-01", checkout = "2019-01-01" },
                additionalneeds = "Breakfast"
            };
            var jsonBody = JsonSerializer.Serialize(bookingDetails);
            request.AddStringBody(jsonBody, ContentType.Json)
                .AddOrUpdateHeader("Content-Type", "application/json")
                .AddOrUpdateHeader("Accept", "application/json");
            response = await client.ExecuteAsync(request);

            dynamic jsonResponse = Newtonsoft.Json.JsonConvert.DeserializeObject(response.Content);
            int bookingIdNew = jsonResponse.bookingid;

            request = new RestRequest($"/booking/{bookingIdNew}", Method.Delete)
                .AddOrUpdateHeader("Authorization", "Basic YWRtaW46cGFzc3dvcmQxMjM=");
            response = await client.ExecuteAsync(request);

            Assert.AreEqual(201, (int)response.StatusCode, "DELETE request failed.");
        }

        [Then(@"the response should have status (.*)")]
        public void ThenTheResponseShouldHaveStatus(int expectedStatusCode)
        {
            Assert.AreEqual(expectedStatusCode, (int)response.StatusCode, $"Expected status code {expectedStatusCode} but got {(int)response.StatusCode}.");
        }
    }
}
