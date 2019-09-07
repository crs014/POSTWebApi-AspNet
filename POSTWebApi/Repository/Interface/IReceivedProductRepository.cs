using POSTWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSTWebApi.Repository.Interface
{
    public interface IReceivedProductRepository
    {
        IEnumerable<ReceivedProduct> GetAll(int skip, int limit);

        IEnumerable<ReceivedProduct> Get(Guid id);

        Task<ReceivedProduct> Create(ReceivedProduct data);

        Task<ReceivedProduct> Update(Guid id, ReceivedProduct data);

        Task<ReceivedProduct> Delete(Guid id);
    }
}
