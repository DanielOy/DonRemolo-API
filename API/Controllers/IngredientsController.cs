using API.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IngredientsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public IngredientsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<IngredientDto>))]
        public async Task<ActionResult<IEnumerable<IngredientDto>>> Get()
        {
            var Ingredients = await _unitOfWork.Ingredients.GetAll();

            var data = _mapper.Map<IEnumerable<Ingredient>, IEnumerable<IngredientDto>>(Ingredients);

            return Ok(data);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IngredientDto))]
        public async Task<ActionResult<IngredientDto>> Get(int id)
        {
            var Ingredient = await _unitOfWork.Ingredients.GetByID(id);

            var IngredientDto = _mapper.Map<Ingredient, IngredientDto>(Ingredient);

            return Ok(IngredientDto);
        }
    }
}
