using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortLink.Domain
{
    public class Log
    {
        public string Type { get; set; }
        public bool IsError { get; set; }
        public string StackTrace { get; set; }
        public string Message { get; set; }
        public string MyProperty { get; set; }
    }
}
