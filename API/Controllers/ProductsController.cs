using API.Dtos;
using API.Helpers;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Pagination<ProductDto>))]
        public async Task<ActionResult<Pagination<ProductDto>>> Get([FromQuery] ProductSpecParams productParams)
        {
            var spec = new ProductPaginationSpecification(productParams);

            var countSpec = new ProductCountSpecification(productParams);

            var total = await _unitOfWork.Products.CountAsync(countSpec);

            var products = await _unitOfWork.Products.GetAllBySpecification(spec);

            var data = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductDto>>(products);

            var page = new Pagination<ProductDto>(productParams.PageIndex, productParams.PageSize, total, data);

            return Ok(page);
        }

        [HttpGet("MostPopular")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ProductDto>))]
        public async Task<ActionResult<IEnumerable<ProductDto>>> MostPopular()
        {
            var spec = new ProductMostPopularSpecification();

            var products = await _unitOfWork.Products.GetAllBySpecification(spec);

            var data = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductDto>>(products);

            return Ok(data);
        }
    }
}
