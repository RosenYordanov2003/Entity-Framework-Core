using AutoMapper;
using AutoMapper.QueryableExtensions;
using FastFood.Data;
using FastFood.Models;
using FastFood.Services.Contracts;
using FastFood.Services.Models.Items;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FastFood.Services
{
    public class ItemService : IItemService
    {
        private readonly IMapper mapper;

        private readonly FastFoodContext fastFoodContext;

        public ItemService(IMapper mapper, FastFoodContext fastFoodContext)
        {
            this.mapper = mapper;
            this.fastFoodContext = fastFoodContext;
        }
        public async Task Create(CreateItemDto item)
        {
            Item itemToAdd = mapper.Map<Item>(item);

            this.fastFoodContext.Add(itemToAdd);

           await this.fastFoodContext.SaveChangesAsync();
        }

        public async Task<ICollection<ListItemDto>> GetAllItems()
        {
            ICollection<ListItemDto> items = await this.fastFoodContext.Items
                 .ProjectTo<ListItemDto>(this.mapper.ConfigurationProvider)
                 .ToArrayAsync();

            return items;
        }
    }
}
