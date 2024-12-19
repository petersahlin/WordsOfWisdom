using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WordsOfWisdom.API.Data;
using WordsOfWisdom.API.Models;
using WordsOfWisdom.API.Repositories;
using WordsOfWisdom.Data.DTOs;
using WordsOfWisdom.Data.Requests;
using WordsOfWisdom.Data.Responses;

namespace WordsOfWisdom.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuotesController : ControllerBase
    {
        private readonly IQuoteRepository _quoteRepository;

        public QuotesController(IQuoteRepository quoteRepository)
        {
            this._quoteRepository = quoteRepository;
        }

        // GET: api/Quotes
        [HttpGet]
        public async Task<GetQuotesResponse> GetQuotes()
        {
            var response = new GetQuotesResponse();
            try
            {
                var quotesList = await _quoteRepository.GetTenQuotesAsync();

                if (quotesList == null)
                {
                    response.Message = "QuotesList is null";
                    response.Result = false;
                    return response;
                }


                var quoteDTOList = new List<QuoteDTO>();
                foreach (var quote in quotesList)
                {
                    var quoteDTO = new QuoteDTO
                    {
                        Id = quote.Id,
                        Author = quote.Author,
                        Text = quote.Text,
                    };
                    quoteDTOList.Add(quoteDTO);
                }

                response.Quotes = quoteDTOList;


            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Result = false;
            }
                return response;
            
        }



        //    // PUT: api/Quotes/5
        //    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //    [HttpPut("{id}")]
        //    public async Task<IActionResult> PutQuote(int id, Quote quote)
        //    {
        //        if (id != quote.Id)
        //        {
        //            return BadRequest();
        //        }

        //        _context.Entry(quote).State = EntityState.Modified;

        //        try
        //        {
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!QuoteExists(id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }

        //        return NoContent();
        //    }

        //    // POST: api/Quotes
        //    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //    [HttpPost]
        //    public async Task<ActionResult<Quote>> PostQuote(Quote quote)
        //    {
        //        _context.Quotes.Add(quote);
        //        await _context.SaveChangesAsync();

        //        return CreatedAtAction("GetQuote", new { id = quote.Id }, quote);
        //    }

        //    // DELETE: api/Quotes/5
        //    [HttpDelete("{id}")]
        //    public async Task<IActionResult> DeleteQuote(int id)
        //    {
        //        var quote = await _context.Quotes.FindAsync(id);
        //        if (quote == null)
        //        {
        //            return NotFound();
        //        }

        //        _context.Quotes.Remove(quote);
        //        await _context.SaveChangesAsync();

        //        return NoContent();
        //    }

        //    private bool QuoteExists(int id)
        //    {
        //        return _context.Quotes.Any(e => e.Id == id);
        //    }
        //}
    }
}
