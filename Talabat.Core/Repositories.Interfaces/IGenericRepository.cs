using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models;
using Talabat.Core.Specifications;

namespace Talabat.Core.Repositories.Interfaces
{
    public interface IGenericRepository<T> where T : BaseModel
    {
        // GetAll
        Task<IEnumerable<T>> GetAllAsync();

        //GetById
        Task<T?> GetAsync(int id);


        // GetAllSpec
        Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecifications<T> spec);
        //GetByIdSpec
        Task<T?> GetwithSpecAsync(ISpecifications<T> spec);

        Task<int> GetCountAsync (ISpecifications<T> spec);



        Task AddAsync(T item);
        void Delete(T item);
        void Update(T item);
    }

}
