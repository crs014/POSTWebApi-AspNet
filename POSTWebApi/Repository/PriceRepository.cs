using POSTWebApi.Models;
using POSTWebApi.Repository.Interface;
using POSTWebApi.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POSTWebApi.Repository
{
    public class PriceRepository : IPriceRepository
    {
        private IDbPOS _db;

        public PriceRepository(IDbPOS db)
        {
            _db = db;
        }

        public Task<Price> Create(Price price)
        {
            return Task.Run(() =>
            {
                Price newPrice = _db.Prices.Create();
                newPrice.ProductId = price.ProductId;
                newPrice.Value = price.Value;
                newPrice.Type = price.Type;
                _db.Prices.Add(newPrice);
                _db.SaveChanges();
                return newPrice;
            });
        }

        public IEnumerable<Price> GetAll(PriceTypeEnum priceType, int skip, int limit)
        {
            return _db.Prices.Where(e => e.Type == priceType)
                       .OrderBy(e => e.RowVersion)
                       .Skip(skip)
                       .Take(limit);
        }
    }
}