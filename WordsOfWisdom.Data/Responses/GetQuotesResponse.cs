using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WordsOfWisdom.Data.DTOs;

namespace WordsOfWisdom.Data.Responses
{
    public class GetQuotesResponse : BaseResponse
    {
        public ICollection<QuoteDTO> Quotes { get; set; }
    }
}
