using ShortLink.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortLink.Application.Interfaces
{
    public interface ILinkService
    {
        Task<List<Link>> GetUserLinks(string userId);
        Task<Link> ShortenLink(string url);
        Task<Link> UpdateLink(Link link);
        Task<bool> DeleteLink(Link link);
    }
}
