using FastFood.Services.Models.Orders;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FastFood.Services.Contracts
{
    public interface IOrderService
    {
        Task<ICollection<ListOrderDto>> GetAllOrders();
    }
}
