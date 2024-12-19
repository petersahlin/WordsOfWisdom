using Microsoft.EntityFrameworkCore;
using WordsOfWisdom.API.Data;
using WordsOfWisdom.API.Models;





namespace WordsOfWisdom.API.Repositories
{
    public interface IQuoteRepository
    {
        public Task<IEnumerable<Quote>> GetTenQuotesAsync();
    }



    public class QuoteRepository : IQuoteRepository
    {
        private readonly WordsOfWisdomContext _wowContext;

        public QuoteRepository(WordsOfWisdomContext wordsOfWisdomContext)
        {
            _wowContext = wordsOfWisdomContext;
        }

        public async Task<IEnumerable<Quote>> GetTenQuotesAsync()
        {
           return await _wowContext.Quotes.Take(10).ToListAsync();
        }
    }
}
