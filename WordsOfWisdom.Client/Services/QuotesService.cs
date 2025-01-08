using System.Net.Http.Json;
using WordsOfWisdom.Data.Responses;

namespace WordsOfWisdom.Client.Services
{
    public interface IQuotesService
    {
        public Task<GetQuotesResponse> GetQuotes();
    }

    public class QuotesService : IQuotesService
    {
        private readonly HttpClient _httpClient;

        public QuotesService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<GetQuotesResponse> GetQuotes()
        {
            var response = new GetQuotesResponse();

            try
            {
                response = await _httpClient.GetFromJsonAsync<GetQuotesResponse>("quotes");

                if (response == null || response.Result == false)
                {
                    response.Message = "An error occured when fetching the quotes.";
                    response.Result = false;
                }




            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Result = false;
            }

            return response;
        }
    }
}
