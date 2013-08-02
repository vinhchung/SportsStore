﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportsStore.Domain.Abstract;

namespace SportsStore.WebUI.Controllers
{
    public class NavController : Controller
    {
        private readonly IProductRepository _productRepository;
        public NavController(IProductRepository repo)
        {
            _productRepository = repo;
        }

        [ChildActionOnly]
        public PartialViewResult Menu(string category = null)
        {
            ViewBag.SelectedCategory = category;
            IEnumerable<string> categories = _productRepository.Products.Select(x => x.Category).Distinct().OrderBy(x=>x);
            return PartialView(categories);
        }

    }
}
