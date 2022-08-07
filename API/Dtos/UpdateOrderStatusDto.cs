using static Core.Entities.Order;

namespace API.Dtos
{
    public class UpdateOrderStatusDto
    {
        public string Id { get; set; }
        public OrderStatus Status { get; set; }
    }
}
