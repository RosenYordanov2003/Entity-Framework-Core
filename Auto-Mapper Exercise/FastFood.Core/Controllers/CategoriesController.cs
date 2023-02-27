namespace FastFood.Core.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AutoMapper;
    using Data;
    using FastFood.Services.Contracts;
    using FastFood.Services.Models.Categories;
    using Microsoft.AspNetCore.Mvc;
    using ViewModels.Categories;

    public class CategoriesController : Controller
    {
        private readonly ICategoryService categoryService;
        private readonly IMapper mapper;

        public CategoriesController(ICategoryService categoryService, IMapper mapper)
        {
            this.categoryService = categoryService;
            this.mapper = mapper;
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCategoryInputModel model)
        {
            if (!ModelState.IsValid)
            {
                return this.RedirectToAction("Create", "Categories");
            }

            CategoryDTO dto = mapper.Map<CategoryDTO>(model);
            await categoryService.AddCategory(dto);

            return this.RedirectToAction("All", "Categories");
        }

        public async Task<IActionResult> All()
        {

            ICollection<ListCategoryDto> categories = await categoryService.GetAllCategories();
            IList<CategoryAllViewModel>viewCategories = new List<CategoryAllViewModel>();
            foreach (ListCategoryDto category in categories)
            {
                CategoryAllViewModel categoryToConvert = this.mapper.Map<CategoryAllViewModel>(category);
                viewCategories.Add(categoryToConvert);
            }
            return this.View(viewCategories);
        }
    }
}
