using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models;

namespace Talabat.Core.Specifications
{
    public class BaseSpecifications<T> : ISpecifications<T> where T : BaseModel
    {
        public Expression<Func<T, bool>> Criteira { get; set; } = null;
        public List<Expression<Func<T, object>>> Includes { get ; set ; } = new List<Expression<Func<T, object>>>();


        public Expression<Func<T, object>> OrderBy { get; set; } = null;
        public Expression<Func<T, object>> OrderByDesc { get; set; } = null;


        public int Skip { get ; set ; }
        public int Take { get ; set ; }
        public bool IsPagiantionEnalbed { get ; set; }

        public BaseSpecifications()
        {
           //    Includes = new List<Expression<Func<T, object>>> ();
          //    Criteira = null;
        }

        public BaseSpecifications(Expression<Func<T, bool>> criteira)
        {
            Criteira = criteira; // P => P.Id == 10 
        }

        
        public void AddorderBy(Expression<Func<T, object>> orderByExpression)
        {
            OrderBy = orderByExpression;
        }

        public void AddorderByDesc(Expression<Func<T, object>> orderByDescExpression)
        {
            OrderByDesc = orderByDescExpression;
        }



        public void ApplyPagiantion(int skip , int take)
        {
            Skip = skip;
            Take = take;
            IsPagiantionEnalbed = true;
        }
    }
}
