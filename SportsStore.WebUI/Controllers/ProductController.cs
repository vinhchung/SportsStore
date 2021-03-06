﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Models;

namespace SportsStore.WebUI.Controllers
{
    public class ProductController : Controller
    {
        private IProductRepository _repository;
        public int PageSize = 3;
        public ProductController(IProductRepository productRepository)
        {
            _repository = productRepository;
        }

        public ViewResult List(string category, int page=1)
        {
            var pageInfo = new PageInfo { 
                CurrentPage = page, 
                TotalItems = category==null? _repository.Products.Count() : _repository.Products.Where(p => p.Category.CategoryName == category).Count(), 
                ItemsPerPage = PageSize 
            };
            if (page < 1 || page > pageInfo.TotalPages)
            {
                pageInfo.CurrentPage = 1;
            }
            ProductsListViewModel viewModel = new ProductsListViewModel {
                Products = _repository.Products.Where(p => category==null || p.Category.CategoryName.Replace("/"," ")==category).OrderBy(p => p.ProductID).Skip((pageInfo.CurrentPage - 1) * PageSize).Take(PageSize),
                PageInfo = pageInfo,
                CurrentCategory = category
            };
            return View(viewModel);
        }
    }
}
