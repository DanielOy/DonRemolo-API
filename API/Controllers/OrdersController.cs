using API.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrdersController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        //[Authorize(Roles = "Administrator")] //TODO: Implement roles
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<OrderDto>))]
        public async Task<ActionResult<IEnumerable<OrderDto>>> Get()
        {
            var spec = new OrderWithProductsSpec();
            var orders = await _unitOfWork.Orders.GetAllBySpecification(spec);

            var data = _mapper.Map<IEnumerable<Order>, IEnumerable<OrderDto>>(orders);

            return Ok(data);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderDto))]
        public async Task<ActionResult<OrderDto>> Get(string id)
        {
            var spec = new OrderWithProductsSpec(new Guid(id));

            var order = await _unitOfWork.Orders.GetBySpecification(spec);

            var orderDto = _mapper.Map<Order, OrderDto>(order);

            return Ok(orderDto);
        }


        [HttpPut("UpdateOrderStatus")]
        //[Authorize(Roles = "Administrator")] //TODO: Implement roles
        public async Task<ActionResult> UpdateOrderStatus(UpdateOrderStatusDto orderStatusDto)
        {
            var spec = new OrderWithProductsSpec(new Guid(orderStatusDto.Id));

            var order = await _unitOfWork.Orders.GetBySpecification(spec);

            order.Status = orderStatusDto.Status;

            _unitOfWork.Orders.Update(order);

            await _unitOfWork.Save();

            return Ok();
        }
    }
}
