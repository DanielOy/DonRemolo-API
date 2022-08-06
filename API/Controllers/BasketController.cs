using API.Dtos;
using API.Extensions;
using API.Helpers;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IBasketService _basketService;
        private readonly UserManager<User> _userManager;

        public BasketController(IMapper mapper, IBasketService basketService, UserManager<User> userManager)
        {
            _mapper = mapper;
            _basketService = basketService;
            _userManager = userManager;
        }

        [HttpGet("GetUserBasket")]
        [Authorize]
        public async Task<ActionResult<BasketDto>> GetUserBasket()
        {
            string userId = await User.GetCurrentUserId(_userManager);

            var order = await _basketService.GetBasketByUserId(userId);

            if (order is null)
                return Ok(await CreateUserBasket(userId));

            var basket = _mapper.Map<BasketDto>(order);

            return Ok(basket);
        }

        private async Task<BasketDto> CreateUserBasket(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            var basket = new BasketDto
            {
                Id = Guid.NewGuid().ToString(),
                UserId = userId,
                Address = user.Address,
                AtHome = !string.IsNullOrEmpty(user.Address),
                ContactName = user.FullName,
                ContactNumber = user.PhoneNumber
            };

            return basket;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BasketDto>> GetBasketById(string id)
        {
            var order = await _basketService.GetBasketById(new Guid(id));

            if (order is null)
                return Ok(new BasketDto(id));

            var basket = _mapper.Map<BasketDto>(order);

            return Ok(basket);
        }

        [HttpPut]
        public async Task<ActionResult<BasketDto>> UpdateBasket(BasketDto basket)
        {
            if (!CurrentUserCanUseBasket(basket).Result)
                return Unauthorized(
                    new ApiErrorResponse(HttpStatusCode.Unauthorized, "User can't update this basket"));

            var order = _mapper.Map<BasketDto, Basket>(basket);

            var updatedOrder = await _basketService.UpdateBasket(order);

            basket = _mapper.Map<BasketDto>(updatedOrder);

            return Ok(basket);
        }

        private async Task<bool> CurrentUserCanUseBasket(BasketDto basket)
        {
            if (!string.IsNullOrEmpty(basket.UserId))
            {
                string userId = await User.GetCurrentUserId(_userManager);

                return basket.UserId.Equals(userId, StringComparison.InvariantCultureIgnoreCase);
            }
            return true;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBasket(string id)
        {
            var guid = new Guid(id);
            var basket = await _basketService.GetBasketById(guid);

            if (!CurrentUserCanUseBasket(basket).Result)
            {
                return Unauthorized(
                    new ApiErrorResponse(HttpStatusCode.Unauthorized, "User can't delete this basket"));
            }

            await _basketService.DeleteBasket(guid);
            return Ok();
        }

        private async Task<bool> CurrentUserCanUseBasket(Basket order)
        {
            var basket = _mapper.Map<BasketDto>(order);

            return await CurrentUserCanUseBasket(basket);
        }

        [HttpPost("OrderBasket")]
        public async Task<ActionResult<string>> ConfirmBasket(string basketId)
        {
            string orderId = await _basketService.ConfirmBasket(new Guid(basketId));

            if (orderId == string.Empty)
                return NotFound();

            return Ok(orderId);
        }
    }
}
