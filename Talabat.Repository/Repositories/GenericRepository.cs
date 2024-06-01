using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models;
using Talabat.Core.Repositories.Interfaces;
using Talabat.Core.Specifications;
using Talabat.Repository.Data;
using Talabat.Repository.Specifications;

namespace Talabat.Repository.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseModel
    {
        private readonly StoreDbContext _context;

        public GenericRepository(StoreDbContext context) // Ask CLR Create Object From StoreDbContext Implicity
        {
            _context = context;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            if(typeof(T) == typeof(Product))
            {
               return (IEnumerable<T>)await _context.Products.Include(P=>P.Brand).Include(P=>P.Category).ToListAsync();
            }
           return await  _context.Set<T>().ToListAsync();
        }
        public async Task<T?> GetAsync(int id)
        {
            if (typeof(T) == typeof(Product))
            {
                return await _context.Products.Where(P => P.Id == id).Include(P => P.Brand).Include(P => P.Category).FirstOrDefaultAsync() as T;
            }
            return await _context.Set<T>().FindAsync(id);
        }



        public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecifications<T> spec)
        {
             return await ApplySpecifications(spec).ToListAsync();
        }

        public async Task<T?> GetwithSpecAsync(ISpecifications<T> spec)
        {
            return await ApplySpecifications(spec).FirstOrDefaultAsync();
        }

        public async Task<int> GetCountAsync(ISpecifications<T> spec)
        {
             return await ApplySpecifications(spec).CountAsync();
        }



        private IQueryable<T> ApplySpecifications(ISpecifications<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(_context.Set<T>(), spec);
        }



        public async Task AddAsync(T item) => await _context.Set<T>().AddAsync(item);
  

        public  void Delete(T item) =>  _context.Set<T>().Remove(item);


        public void Update(T item) => _context.Set<T>().Update(item);
    
    }
}
