using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using System.Linq;
using SportsStore.WebUI.Controllers;
using SportsStore.WebUI.HtmlHelpers;
using System.Collections.Generic;
using System.Web.Mvc;
using SportsStore.WebUI.Models;

namespace SportsStore.Tests
{
    [TestClass]
    public class UnitTest1
    {
        private ProductController SetupController(int pageSize)
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product {ProductId=1, Name="P1"},
                new Product {ProductId=2, Name="P2"},
                new Product {ProductId=2, Name="P3"},
                new Product {ProductId=2, Name="P4"},
                new Product {ProductId=2, Name="P5"}
            }.AsQueryable());
            var controller = new ProductController(mock.Object);
            controller.PageSize = pageSize;
            return controller;   
        }

        [TestMethod]
        public void Can_Paginate()
        {
            var controller = SetupController(3);
            ProductsListViewModel result = (ProductsListViewModel)controller.List(2).Model;
            Product[] prodArray = result.Products.ToArray();
            Assert.IsTrue(prodArray.Length == 2);
            Assert.AreEqual("P4", prodArray[0].Name);
            Assert.AreEqual("P5", prodArray[1].Name);
        }

        [TestMethod]
        public void Can_Send_Pagination_View_Model()
        {
            var controller = SetupController(3);
            ProductsListViewModel result = (ProductsListViewModel)controller.List(2).Model;
            PageInfo pageInfo = result.PageInfo;
            Assert.AreEqual(2, pageInfo.CurrentPage);
            Assert.AreEqual(3, pageInfo.ItemsPerPage);
            Assert.AreEqual(2, pageInfo.TotalPages);
            Assert.AreEqual(5, pageInfo.TotalItems);
        }

        [TestMethod]
        public void PageLinks_Generation_Display()
        {
            HtmlHelper myHelper = null;

            PageInfo pageInfo = new PageInfo { CurrentPage = 2, ItemsPerPage = 10, TotalItems = 28 };
            Func<int, string> pageUrlDelegate = i => "Page" + i;
            MvcHtmlString result = myHelper.PageLinks(pageInfo, pageUrlDelegate);
            Assert.AreEqual(@"<a href=""Page1"">1</a><a class=""selected"" href=""Page2"">2</a><a href=""Page3"">3</a>", result.ToString());
        }
    }
}
