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
    public class SizesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SizesController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<SizeDto>))]
        public async Task<ActionResult<IEnumerable<SizeDto>>> Get()
        {
            var Sizes = await _unitOfWork.Sizes.GetAll();

            var data = _mapper.Map<IEnumerable<Size>, IEnumerable<SizeDto>>(Sizes);

            return Ok(data);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SizeDto))]
        public async Task<ActionResult<SizeDto>> Get(int id)
        {
            var Size = await _unitOfWork.Sizes.GetByID(id);

            var SizeDto = _mapper.Map<Size, SizeDto>(Size);

            return Ok(SizeDto);
        }
    }
}
