using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPService.Common.Entities
{
    public class ErrorResponse
    {
        public int Status { get; set; }
        public string ErrorCode { get; set; } = "UNKNOWN_ERROR";
        public string Message { get; set; } = "Something went wrong.";
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}
