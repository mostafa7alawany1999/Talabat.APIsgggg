using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models;
using Talabat.Core.Specifications;

namespace Talabat.Repository.Specifications
{
    public class SpecificationEvaluator<TEntity> where TEntity : BaseModel
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery,ISpecifications<TEntity> spec)
        {

            var query = inputQuery; // _context.Set<Products>()

            if(spec.Criteira is not null)
                query = query.Where(spec.Criteira);
            // _context.Set<Products>().Where(P=>P.id == 10)

            if (spec.OrderBy is not null)
                query = query.OrderBy(spec.OrderBy);

            if (spec.OrderByDesc is not null)
                query = query.OrderByDescending(spec.OrderByDesc);

            if (spec.IsPagiantionEnalbed)
                query = query.Skip(spec.Skip).Take(spec.Take);
            



            query = spec.Includes.Aggregate(query,(currentQuery,includeExpression) => currentQuery.Include(includeExpression));


            //_context.Products.Include(P=>P.Brand).Include(P=>P.Category).ToListAsync();

            //_context.Products.Where(P => P.Id == id).Include(P => P.Brand).Include(P => P.Category).FirstOrDefaultAsync() as T;

            return query; 
        }
    }
}
