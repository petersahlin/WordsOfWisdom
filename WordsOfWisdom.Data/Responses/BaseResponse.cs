using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordsOfWisdom.Data.Responses
{
    public class BaseResponse
    {
        public int Id { get; set; }
        public bool Result { get; set; } = true;
        public int ErrorCode { get; set; } = 0;
        public string Message { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
