using Microsoft.EntityFrameworkCore;
using WordsOfWisdom.API.Data;
using WordsOfWisdom.API.Models;
using WordsOfWisdom.Data.Responses;





namespace WordsOfWisdom.API.Repositories
{
    public interface IQuoteRepository
    {
        public Task<IEnumerable<Quote>> GetTenQuotesAsync();

        public Task<bool> ImportQuotesAsync(List<Quote> quotes);
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
           return await _wowContext.Quotes
                .OrderBy(x => Guid.NewGuid()).
                Take(10).ToListAsync();
        }

        public async Task<bool> ImportQuotesAsync(List<Quote> quotes)
        {
            _wowContext.Quotes.RemoveRange(_wowContext.Quotes);
            await _wowContext.Quotes.AddRangeAsync(quotes);
            var numberOfQuotesAdded = quotes.Count();
            var saveChanges = await _wowContext.SaveChangesAsync();

            return saveChanges == numberOfQuotesAdded;
        }
    }
}
