﻿using API.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromotionsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PromotionsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Promotion>))]
        public async Task<ActionResult<IEnumerable<Promotion>>> Get()
        {
            var promotions = await _unitOfWork.Promotions
                .GetAllByExpression(x => x.Id > 0); //TODO: change with a specification with pagination

            return Ok(promotions);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Promotion))]
        public async Task<ActionResult<Promotion>> Get(int id)
        {
            var promotion = await _unitOfWork.Promotions.GetByID(id);
            
            return Ok(promotion);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> Post([FromBody] PromotionDto promotionDto)
        {
            var promotion = _mapper.Map<Promotion>(promotionDto);

            _unitOfWork.Promotions.Insert(promotion);

            await _unitOfWork.Save();

            return Ok();
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> Put([FromBody] Promotion promotion)
        {
            _unitOfWork.Promotions.Update(promotion);

            await _unitOfWork.Save();

            return Ok();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> Delete(int id)
        {
            _unitOfWork.Promotions.Delete(id);

            await _unitOfWork.Save();

            return Ok();
        }
    }
}
