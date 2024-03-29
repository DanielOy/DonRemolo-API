﻿using API.Dtos.Promotion;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
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
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<PromotionViewDto>))]
        public async Task<ActionResult<IEnumerable<PromotionViewDto>>> Get()
        {
            var promotions = await _unitOfWork.Promotions.GetAll();

            var data = _mapper.Map<IEnumerable<Promotion>, IEnumerable<PromotionViewDto>>(promotions);

            return Ok(data);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PromotionViewDto))]
        public async Task<ActionResult<PromotionViewDto>> Get(int id)
        {
            var promotion = await _unitOfWork.Promotions.GetByID(id);

            var promotionDto = _mapper.Map<Promotion, PromotionViewDto>(promotion);

            return Ok(promotionDto);
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

        [HttpPut]
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

        [HttpGet("Rules/{id}")]
        public async Task<ActionResult<PromotionRuleDto>> GetPromotionRules(int id)
        {
            var spec = new PromotionWithRulesSpec(id);

            var promotion = await _unitOfWork.Promotions.GetBySpecification(spec);

            if (promotion?.RuleItems?.Any(x => x.CategoryId > 0) ?? false)
            {
                foreach (var rule in promotion.RuleItems.Where(x => x.CategoryId > 0))
                {
                    var productSpec = new ProductByCategoryIdSpec(rule.CategoryId.Value);

                    var products = (await _unitOfWork.Products
                         .GetAllBySpecification(productSpec))
                         .Select(x => new PromotionRuleProduct { ProductId = x.Id, Product = x })
                         .ToList();

                    rule.Products = products;
                }
            }

            var promotionDto = _mapper.Map<PromotionRuleDto>(promotion);

            return Ok(promotionDto);
        }
    }
}
