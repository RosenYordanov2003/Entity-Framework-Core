using AutoMapper;
using AutoMapper.QueryableExtensions;
using FastFood.Data;
using FastFood.Models;
using FastFood.Services.Contracts;
using FastFood.Services.Models.Categories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FastFood.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly FastFoodContext fastFoodContext;

        private readonly IMapper mapper;

        public CategoryService(FastFoodContext fastFoodContext, IMapper mapper)
        {
            this.fastFoodContext = fastFoodContext;
            this.mapper = mapper;
        }

        public async Task AddCategory(CategoryDTO categoryDTO)
        {
            Category categoryToAdd = this.mapper.Map<Category>(categoryDTO);
            fastFoodContext.Add(categoryToAdd);
            await fastFoodContext.SaveChangesAsync();
        }

        public async Task<ICollection<ListCategoryDto>> GetAllCategories()
        {
            ICollection<ListCategoryDto> categories = await fastFoodContext
                  .Categories.ProjectTo<ListCategoryDto>(mapper.ConfigurationProvider)
                  .ToArrayAsync();
            return categories;
        }
    }
}
