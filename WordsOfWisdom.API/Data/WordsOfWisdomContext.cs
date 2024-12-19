using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using WordsOfWisdom.API.Models;

namespace WordsOfWisdom.API.Data
{
    public class WordsOfWisdomContext : DbContext
    {


        public WordsOfWisdomContext(DbContextOptions<WordsOfWisdomContext> options)
            : base(options)
        {
        }

        public DbSet<Quote> Quotes { get; set; }


    }
}
