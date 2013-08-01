using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportsStore.Domain.Abstract;
using SportsStore.WebUI.Models;

namespace SportsStore.WebUI.Controllers
{
    public class ProductController : Controller
    {
        private IProductRepository _repository;
        public int PageSize = 4;
        public ProductController(IProductRepository productRepository)
        {
            _repository = productRepository;
        }

        public ViewResult List(string category, int page=1)
        {
            var pageInfo = new PageInfo { CurrentPage = page, TotalItems = _repository.Products.Where(p => category == null || p.Category == category).Count(), ItemsPerPage = PageSize };
            if (page < 1 || page > pageInfo.TotalPages)
            {
                pageInfo.CurrentPage = 1;
            }
            ProductsListViewModel viewModel = new ProductsListViewModel
            {
                Products = _repository.Products.Where(p => category==null || p.Category==category).OrderBy(p => p.ProductId).Skip((pageInfo.CurrentPage - 1) * PageSize).Take(PageSize),
                PageInfo = pageInfo,
                CurrentCategory = category
            };
            return View(viewModel);
        }
    }
}
