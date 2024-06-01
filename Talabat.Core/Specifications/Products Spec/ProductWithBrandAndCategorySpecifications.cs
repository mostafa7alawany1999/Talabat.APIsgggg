using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models;

namespace Talabat.Core.Specifications.Products_Spec
{
    public class ProductWithBrandAndCategorySpecifications : BaseSpecifications<Product>
    {


        // This Constructor Will Be Used For Creating Object For Get All Products
        public ProductWithBrandAndCategorySpecifications(ProductSpecParams productSpec) 
            : base(P=>
                       (string.IsNullOrEmpty(productSpec.Search) || P.Name.ToLower().Contains(productSpec.Search))
                       &&
                       (!productSpec.BrandId.HasValue || P.BrandId == productSpec.BrandId.Value) 
                       &&
                       (!productSpec.CategoryId.HasValue || P.CategoryId == productSpec.CategoryId.Value)
                   )
        {
            Includes.Add(P => P.Brand);
            Includes.Add(P => P.Category);


            if (!string.IsNullOrEmpty(productSpec.sort))
            {
                switch (productSpec.sort)
                {
                    case "priceAsc":
                        AddorderBy(P => P.Price);
                        break;

                    case "priceDesc":
                        AddorderByDesc(P => P.Price);
                        break;

                    default:
                        AddorderBy(P => P.Name);
                        break;
                }
            }
            else
            {
                AddorderBy(P => P.Name);
            }

            // PageSize - PageIndex
            // Total = 18 
            // PageSize = 5 * 3
            // pageIndex = 4
            ApplyPagiantion(productSpec.PageSize*(productSpec.PageIndex-1),productSpec.PageSize);
        }
        public ProductWithBrandAndCategorySpecifications(int id) : base(P=>P.Id==id)
        {
            Includes.Add(P => P.Brand);
            Includes.Add(P => P.Category);
        }

    }
}
