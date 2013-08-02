using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportsStore.Domain.Entities;

namespace SportsStore.WebUI.Binders
{
    public class CartModelBinder : IModelBinder
    {
        private const string cartName = "Cart";
        public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            Cart cart = (Cart)controllerContext.RequestContext.HttpContext.Session[cartName];
            if(cart == null) {
                cart = new Cart();
                controllerContext.RequestContext.HttpContext.Session[cartName] = cart;
            }
            return cart;
        }
    }
}