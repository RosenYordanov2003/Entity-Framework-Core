using FastFood.Services.Models.Orders;
using System.Threading.Tasks;

namespace FastFood.Services.Contracts
{
    public interface IOrderService
    {
        Task CreateOrder(OrderDto orderDto);
    }
}
