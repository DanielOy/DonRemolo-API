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
    public class DoughsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public DoughsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<DoughDto>))]
        public async Task<ActionResult<IEnumerable<DoughDto>>> Get()
        {
            var doughs = await _unitOfWork.Doughs.GetAll();

            var data = _mapper.Map<IEnumerable<Dough>, IEnumerable<DoughDto>>(doughs);

            return Ok(data);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DoughDto))]
        public async Task<ActionResult<DoughDto>> Get(int id)
        {
            var dough = await _unitOfWork.Doughs.GetByID(id);

            var doughDto = _mapper.Map<Dough, DoughDto>(dough);

            return Ok(doughDto);
        }
    }
}
