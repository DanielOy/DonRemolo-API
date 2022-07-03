using API.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CategoriesController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET: api/<CategoriesController>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<CategoryDto>))]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> Get()
        {
            var categories = await _unitOfWork.Categories.GetAllByExpression(x => x.Id > 0); //TODO: Replace with a specification

            var data = _mapper.Map<IEnumerable<Category>, IEnumerable<CategoryDto>>(categories);

            return Ok(data);
        }

        // GET api/<CategoriesController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CategoryDto))]
        public async Task<ActionResult<CategoryDto>> Get(int id)
        {
            var category = await _unitOfWork.Categories.GetByID(id); 

            var categoryDto = _mapper.Map<CategoryDto>(category);

            return Ok(categoryDto);
        }
    }
}
