using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortLink.Application.ExternalServices.VirusTotal.Interface
{
    public interface IVirusTotalService
    {
        Task<bool> SendRlForAnalysis(string url);
    }
}
