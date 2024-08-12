using ShortLink.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortLink.Infra.Interfaces
{
    public interface ILinkRepository
    {
        Task<List<Link>> GetUserLinks(string userId);
        Task UpdadteLink(Link link);
        Task DeleteLink(string linkId);
        Task CreateLink(Link link);
    }
}
