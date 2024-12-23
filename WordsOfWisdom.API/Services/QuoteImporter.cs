using System;
using System.Diagnostics;
using WordsOfWisdom.API.Data;
using WordsOfWisdom.API.Models;
using WordsOfWisdom.API.Repositories;

namespace WordsOfWisdom.API.Services
{
    public class QuoteImporter
    {

        private readonly IQuoteRepository _quoteRepository;

        public QuoteImporter(IQuoteRepository quoteRepository)
        {

            this._quoteRepository = quoteRepository;
        }

        public async Task ImportQuotesAsync(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Debug.WriteLine("File not found.");
                return;
            }

            var lines = await File.ReadAllLinesAsync(filePath);

            var quotes = lines
                .Where(line => !string.IsNullOrWhiteSpace(line))
                .Select(line =>
                {
                    var parts = line.Split('-', 2);
                    return new Quote
                    {
                        Text = parts[0].Trim(),
                        Author = parts.Length > 1 ? parts[1].Trim() : "Unknown"
                    };
                }).ToList();

            //send to repo


            if (quotes.Any())
            {
                var successfulImport = await _quoteRepository.ImportQuotesAsync(quotes);
                if (successfulImport)
                {
                    Debug.WriteLine($"{quotes.Count} quotes imported.");
                }
            }
            else
            {
                Debug.WriteLine("No quotes found.");
            }
        }

    }
}
