using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.DTO;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talabat.Core.Models;
using Talabat.Core.Repositories.Interfaces;
using Talabat.Core.Specifications.Products_Spec;

namespace Talabat.APIs.Controllers
{

    public class ProductsController : BaseApiController
    {
        private readonly IGenericRepository<Product> _productRepo;
        private readonly IMapper _mapper;
        private readonly IGenericRepository<ProductBrand> _brandsRepo;
        private readonly IGenericRepository<ProductCategory> _categoriesRepo;

        public ProductsController(IGenericRepository<Product> ProductRepo,IMapper mapper,
                                  IGenericRepository<ProductBrand> brandsRepo,
                                  IGenericRepository<ProductCategory>categoriesRepo)
        {
            _productRepo = ProductRepo;
            _mapper = mapper;
            _brandsRepo = brandsRepo;
            _categoriesRepo = categoriesRepo;
        }



        #region GetAll_Product

        
        [Authorize]
        [HttpGet]  
        public async Task<ActionResult<IReadOnlyList<ProductToReturnDto>>> GetProduct([FromQuery]ProductSpecParams productSpec)
        {
            // var products =  await _productRepo.GetAllAsync();

            var spec = new ProductWithBrandAndCategorySpecifications(productSpec);
            var products = await _productRepo.GetAllWithSpecAsync(spec);


            var data = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products);


            var countSpec = new ProductWithFilterionForCountSpecifications(productSpec);
            int count =await _productRepo.GetCountAsync(countSpec);

            return Ok(new Pagination<ProductToReturnDto>(productSpec.PageSize,productSpec.PageIndex, count, data));

        }
        #endregion

        #region GetById_Product
        [Authorize]
        [ProducesResponseType(typeof(ProductToReturnDto),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductToReturnDto>> GetProductId(int id)
        {

            // var product =  await _productRepo.GetAsync(id);
            var spec = new ProductWithBrandAndCategorySpecifications(id);
            var product = await _productRepo.GetwithSpecAsync(spec);
            if (product is null)
                return NotFound(new ApiResponse(404));

           // var result = _mapper.Map<Product,ProductToReturnDto>(product);
            return Ok(_mapper.Map<Product, ProductToReturnDto>(product));

        }
        #endregion

        #region GetAll_Brand

        [Authorize]
        [HttpGet("brands")] // Get : /api/products/brands
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
        {
            var brands = await _brandsRepo.GetAllAsync();
            return Ok(brands);
        }
        #endregion

        #region GetAll_Category

        [Authorize]
        [HttpGet("categories")] // Get : /api/products/categories
        public async Task<ActionResult<IReadOnlyList<ProductCategory>>> Getcategories()
        {
            var categories = await _categoriesRepo.GetAllAsync();
            return Ok(categories);
        }

        #endregion


    }
}
