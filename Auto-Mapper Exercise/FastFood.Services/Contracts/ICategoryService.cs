using FastFood.Services.Models.Categories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FastFood.Services.Contracts
{
    public interface ICategoryService
    {
        Task AddCategory(CategoryDTO categoryDTO);

        Task<ICollection<ListCategoryDto>> GetAllCategories();
    }
}
