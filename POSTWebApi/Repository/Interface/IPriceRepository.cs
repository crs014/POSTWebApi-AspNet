using POSTWebApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POSTWebApi.Repository.Interface
{
    public interface IPriceRepository
    {
        Task<Price> Create(Price price);

        IEnumerable<Price> GetAll(PriceTypeEnum priceType, int skip, int limit);
    }
}
