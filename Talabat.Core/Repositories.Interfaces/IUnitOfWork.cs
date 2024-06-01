using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models;

namespace Talabat.Core.Repositories.Interfaces
{
    public interface IUnitOfWork : IAsyncDisposable
    {

        // Create Repo of Any Entity
        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseModel ;

        Task<int> CompleteAsync();
    }
}
