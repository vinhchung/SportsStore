using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Models;

namespace SportsStore.WebUI.Controllers
{
    public class CartController : Controller
    {
        private IProductRepository _productRepo;
        private IOrderProcessor _orderProcessor;

        public CartController(IProductRepository productRepo, IOrderProcessor orderProcessor)
        {
            _productRepo = productRepo;
            _orderProcessor = orderProcessor;
        }

        public ViewResult Index(Cart cart, string returnUrl)
        {
            return View(new CartIndexViewModel { Cart=cart, ReturnUrl=returnUrl});
        }

        public RedirectToRouteResult AddItem(Cart cart, int productId, string returnUrl)
        {
            Product prod = _productRepo.Products.Where(p => p.ProductId == productId).FirstOrDefault();
            if (prod != null)
            {
                cart.AddItem(prod, 1);
            }
            return RedirectToAction("Index", new {returnUrl});
        }

        public RedirectToRouteResult RemoveItem(Cart cart, int productId, string returnUrl)
        {
            cart.RemoveLine(productId);
            return RedirectToAction("Index", new { returnUrl });
        }

        public ViewResult Checkout()
        {
            return View(new ShippingDetails());
        }

        [HttpPost]
        public ViewResult Checkout(Cart cart, ShippingDetails shippingDetails)
        {
            if (cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Sorry, your cart is empty!");
            }

            if(ModelState.IsValid) {
                _orderProcessor.ProcessOrder(cart, shippingDetails);
                cart.Clear();
                return View("Completed");
            }
            else {
                return View(shippingDetails);
            }
        }

        public ActionResult CustomView()
        {
            return View();
        }

        [ChildActionOnly]
        public PartialViewResult Summary(Cart cart)
        {
            return PartialView("CartSummary", cart);
        }
    }
}
