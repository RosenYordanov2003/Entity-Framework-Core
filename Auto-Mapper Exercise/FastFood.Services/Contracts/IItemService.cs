using FastFood.Services.Models.Items;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FastFood.Services.Contracts
{
    public interface IItemService
    {
        Task Create(CreateItemDto item);

        Task<ICollection<ListItemDto>> GetAllItems();
    }
}
