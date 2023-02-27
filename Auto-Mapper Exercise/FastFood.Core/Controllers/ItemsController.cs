namespace FastFood.Core.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Data;
    using FastFood.Services.Contracts;
    using FastFood.Services.Models.Items;
    using Microsoft.AspNetCore.Mvc;
    using ViewModels.Items;

    public class ItemsController : Controller
    {
        private readonly FastFoodContext context;
        private readonly IItemService itemService;
        private readonly IMapper mapper;

        public ItemsController(FastFoodContext context, IItemService itemService, IMapper mapper)
        {
            this.context = context;
            this.itemService = itemService;
            this.mapper = mapper;
        }

        public IActionResult Create()
        {
            var categories = this.context.Categories
                .ProjectTo<CreateItemViewModel>(mapper.ConfigurationProvider)
                .ToList();

            return this.View(categories);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateItemInputModel model)
        {
            CreateItemDto itemDto = this.mapper.Map<CreateItemDto>(model);

            await this.itemService.Create(itemDto);

            return this.RedirectToAction("All", "Items");
        }

        public async Task<IActionResult> All()
        {
            ICollection<ListItemDto> itemsToConvert = await this.itemService.GetAllItems();

            IList<ItemsAllViewModels> items = new List<ItemsAllViewModels>();

            foreach (ListItemDto item in itemsToConvert)
            {
                items.Add(this.mapper.Map<ItemsAllViewModels>(item));
            }
            return this.View(items);
        }
    }
}
