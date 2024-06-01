using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Models
{
    public class Product : BaseModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string PictureUrl { get; set; }
        public decimal Price { get; set;}


        // RelationShip One to Many 
        public int BrandId { get; set; } // FK
        public ProductBrand Brand { get; set; }

        public int CategoryId { get; set; } // Fk
        public ProductCategory Category { get; set; }
    }
}
