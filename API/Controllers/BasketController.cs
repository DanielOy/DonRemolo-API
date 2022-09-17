using API.Dtos.Basket;
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
        public async Task<ActionResult<GetBasketDto>> GetUserBasket()
        {
            string userId = await User.GetCurrentUserId(_userManager);

            var basket = await _basketService.GetBasketByUserId(userId);

            if (basket is null)
                return Ok(await CreateUserBasket(userId));

            var basketDto = _mapper.Map<GetBasketDto>(basket);

            return Ok(basketDto);
        }

        private async Task<GetBasketDto> CreateUserBasket(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            var basket = new GetBasketDto
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
        public async Task<ActionResult<GetBasketDto>> GetBasketById(Guid id)
        {
            var order = await _basketService.GetBasketById(id);

            if (order is null)
                return Ok(new GetBasketDto(id.ToString()));

            var basket = _mapper.Map<GetBasketDto>(order);

            return Ok(basket);
        }

        [HttpPut]
        public async Task<ActionResult<GetBasketDto>> UpdateBasket(SaveBasketDto saveBasket)
        {
            if (!CurrentUserCanUseBasket(saveBasket).Result)
                return Unauthorized(
                    new ApiErrorResponse(HttpStatusCode.Unauthorized, "User can't update this basket"));

            var basket = _mapper.Map<Basket>(saveBasket);

            var updatedOrder = await _basketService.UpdateBasket(basket);

            var getBasket = _mapper.Map<GetBasketDto>(updatedOrder);

            return Ok(getBasket);
        }

        private async Task<bool> CurrentUserCanUseBasket(SaveBasketDto basket)
        {
            if (!string.IsNullOrEmpty(basket.UserId))
            {
                string userId = await User.GetCurrentUserId(_userManager);

                return basket.UserId.Equals(userId, StringComparison.InvariantCultureIgnoreCase);
            }
            return true;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteBasket(Guid id)
        {
            var basket = await _basketService.GetBasketById(id);

            if (!CurrentUserCanUseBasket(basket).Result)
            {
                return Unauthorized(
                    new ApiErrorResponse(HttpStatusCode.Unauthorized, "User can't delete this basket"));
            }

            await _basketService.DeleteBasket(id);
            return Ok();
        }

        private async Task<bool> CurrentUserCanUseBasket(Basket basket)
        {
            var basketDto = _mapper.Map<SaveBasketDto>(basket);

            return await CurrentUserCanUseBasket(basketDto);
        }

        [HttpPost("OrderBasket")]
        public async Task<ActionResult<string>> ConfirmBasket(Guid basketId)
        {
            string orderId = await _basketService.ConfirmBasket(basketId);

            if (orderId == string.Empty)
                return NotFound();

            return Ok(orderId);
        }

        [HttpPost("ProductsCount")]
        public async Task<ActionResult<int>> GetProductsCount(Guid basketId)
        {
            int count = await _basketService.GetProductsCount(basketId);

            return Ok(count);
        }
    }
}
