using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models;

namespace Talabat.Core.Specifications.Products_Spec
{
    public class ProductWithFilterionForCountSpecifications : BaseSpecifications<Product>
    {


        public ProductWithFilterionForCountSpecifications(ProductSpecParams productSpec)
           : base(P =>
                       (!productSpec.BrandId.HasValue || P.BrandId == productSpec.BrandId.Value)
                       &&
                       (!productSpec.CategoryId.HasValue || P.CategoryId == productSpec.CategoryId.Value)
                 )
        {
            
        }



    }
}
