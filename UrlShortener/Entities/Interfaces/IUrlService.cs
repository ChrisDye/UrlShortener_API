using Entities.Models.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Interfaces
{
    public interface IUrlService
    {
        Task<Paginated<UrlDto>> GetAll(int? page, int? pageSize);
        Task<UrlDto> GetUrl(string shortened);

        Task<UrlDto> UpdateUrl(UrlDto url);

        Task<UrlDto> AddUrl(UrlCreate url);

        Task DeleteUrl(int id);

    }
}
