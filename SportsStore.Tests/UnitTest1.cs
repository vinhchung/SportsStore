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
        private ProductsListViewModel GetViewModel(int pageSize, int currentPage, string category)
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product {ProductId=1, Name="P1", Category="Cat1"},
                new Product {ProductId=2, Name="P2", Category="Cat2"},
                new Product {ProductId=2, Name="P3", Category="Cat1"},
                new Product {ProductId=2, Name="P4", Category="Cat1"},
                new Product {ProductId=2, Name="P5", Category="Cat2"}
            }.AsQueryable());
            var controller = new ProductController(mock.Object);
            controller.PageSize = pageSize;
            return (ProductsListViewModel)controller.List(category, currentPage).Model;   
        }

        [TestMethod]
        public void Can_Paginate()
        {
            int pageSize = 3, currentPage = 2;
            ProductsListViewModel result = GetViewModel(pageSize, currentPage, null);
            Product[] prodArray = result.Products.ToArray();
            Assert.IsTrue(prodArray.Length == 2);
            Assert.AreEqual("P4", prodArray[0].Name);
            Assert.AreEqual("P5", prodArray[1].Name);
        }

        [TestMethod]
        public void Can_Send_Pagination_View_Model()
        {
            int pageSize = 3, currentPage = 2;
            ProductsListViewModel result = GetViewModel(pageSize, currentPage, null);
            PageInfo pageInfo = result.PageInfo;
            Assert.AreEqual(2, pageInfo.CurrentPage);
            Assert.AreEqual(3, pageInfo.ItemsPerPage);
            Assert.AreEqual(2, pageInfo.TotalPages);
            Assert.AreEqual(5, pageInfo.TotalItems);
        }

        [TestMethod]
        public void Can_Filter_Products_By_Category()
        {
            int pageSize = 3, currentPage = 1;
            ProductsListViewModel result = GetViewModel(pageSize, currentPage, "Cat1");
            Product[] prodArray = result.Products.ToArray();
            PageInfo pageInfo = result.PageInfo;
            Assert.AreEqual(3, pageInfo.TotalItems);
            Assert.IsTrue("Cat1"==prodArray[0].Category && "P1" == prodArray[0].Name);
            Assert.IsTrue("Cat1"==prodArray[1].Category && "P3" == prodArray[1].Name);
            Assert.IsTrue("Cat1"==prodArray[2].Category && "P4" == prodArray[2].Name);
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
