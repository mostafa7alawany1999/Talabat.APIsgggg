using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Talabat.Core.Models;
using Talabat.Core.Models.Order;

namespace Talabat.Repository.Data
{
    public static class StoreDbContextSeed
    {

        // Seed Data 

        public static async Task SeedAsync(StoreDbContext _context)
        {

            // 1. Brands
            if(_context.ProductBrands.Count() == 0)
            {
                // 1. Read Data From Json File....
                var BrandData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/brands.json");
                // 2. Convert Json String To The Needed Type...
                var Brands = JsonSerializer.Deserialize<List<ProductBrand>>(BrandData);


                if (Brands?.Count() > 0)
                {
                    foreach (var brand in Brands)
                    {
                        _context.Set<ProductBrand>().Add(brand);
                    }
                    await _context.SaveChangesAsync();
                }
            }

            // =============================================================

            // 2. Categories
            if (_context.ProductCategories.Count() == 0)
            {
                // 1. Read Data From Json File....
                var CategoryData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/categories.json");
                // 2. Convert Json String To The Needed Type...
                var Categories = JsonSerializer.Deserialize<List<ProductCategory>>(CategoryData);

                if (Categories?.Count() > 0)
                {
                    foreach (var category in Categories)
                    {
                        _context.Set<ProductCategory>().Add(category);
                    }
                    await _context.SaveChangesAsync();
                }
            }

            // =============================================================

            // 3. Products
            if (_context.Products.Count() == 0)
            {
                // 1. Read Data From Json File....
                var ProductData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/products.json");
                // 2. Convert Json String To The Needed Type...
                var products = JsonSerializer.Deserialize<List<Product>>(ProductData);


                if (products?.Count() > 0)
                {
                    foreach (var product in products)
                    {
                        _context.Set<Product>().Add(product);
                    }
                    await _context.SaveChangesAsync();
                }
            }



            // =============================================================

            // 4. Delivery Method
            if (_context.DeliveryMethod.Count() == 0)
            {
                // 1. Read Data From Json File....
                var DeliveryData = File.ReadAllText("../Talabat.Repository/Data/DataSeed/delivery.json");
                // 2. Convert Json String To The Needed Type...
                var delivers = JsonSerializer.Deserialize<List<DeliveryMethod>>(DeliveryData);


                if (delivers?.Count() > 0)
                {
                    foreach (var delivery in delivers)
                    {
                        _context.Set<DeliveryMethod>().Add(delivery);
                    }
                    await _context.SaveChangesAsync();
                }
            }

        }


    }
}
