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
        private ProductsListViewModel GetProductViewModel(int pageSize, int currentPage, string category)
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                new Product {ProductID=1, ProductName="P1", Category=new Category{CategoryID=1, CategoryName="Cat1"}},
                new Product {ProductID=2, ProductName="P2", Category=new Category{CategoryID=2, CategoryName="Cat2"}},
                new Product {ProductID=2, ProductName="P3", Category=new Category{CategoryID=1, CategoryName="Cat1"}},
                new Product {ProductID=2, ProductName="P4", Category=new Category{CategoryID=1, CategoryName="Cat1"}},
                new Product {ProductID=2, ProductName="P5", Category=new Category{CategoryID=5, CategoryName="Cat5"}}
            }.AsQueryable());
            var controller = new ProductController(mock.Object);
            controller.PageSize = pageSize;
            return (ProductsListViewModel)controller.List(category, currentPage).Model;   
        }

        [TestMethod]
        public void Can_Paginate()
        {
            int pageSize = 3, currentPage = 2;
            ProductsListViewModel result = GetProductViewModel(pageSize, currentPage, null);
            Product[] prodArray = result.Products.ToArray();
            Assert.IsTrue(prodArray.Length == 2);
            Assert.AreEqual("P4", prodArray[0].ProductName);
            Assert.AreEqual("P5", prodArray[1].ProductName);
        }

        [TestMethod]
        public void Can_Send_Pagination_View_Model()
        {
            int pageSize = 3, currentPage = 2;
            ProductsListViewModel result = GetProductViewModel(pageSize, currentPage, null);
            PageInfo pageInfo = result.PageInfo;
            Assert.AreEqual(2, pageInfo.CurrentPage);
            Assert.AreEqual(3, pageInfo.ItemsPerPage);
            Assert.AreEqual(2, pageInfo.TotalPages);
            Assert.AreEqual(5, pageInfo.TotalItems);
        }

        [TestMethod]
        public void Can_Filter_Products_By_CategoryID()
        {
            int pageSize = 3, currentPage = 1;
            ProductsListViewModel result = GetProductViewModel(pageSize, currentPage, "Cat1");
            Product[] prodArray = result.Products.ToArray();
            PageInfo pageInfo = result.PageInfo;
            Assert.AreEqual(3, pageInfo.TotalItems);
            Assert.IsTrue(1==prodArray[0].Category.CategoryID && "P1" == prodArray[0].ProductName);
            Assert.IsTrue(1==prodArray[1].Category.CategoryID && "P3" == prodArray[1].ProductName);
            Assert.IsTrue(1==prodArray[2].Category.CategoryID && "P4" == prodArray[2].ProductName);
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

        [TestMethod]
        public void Menu_GetCategories_Display()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(p => p.Products).Returns(
                new Product [] {
                   new Product {ProductID=1, ProductName="P1", Category=new Category{CategoryID=1, CategoryName="Cat1"}},
                new Product {ProductID=2, ProductName="P2", Category=new Category{CategoryID=2, CategoryName="Cat2"}},
                new Product {ProductID=2, ProductName="P3", Category=new Category{CategoryID=1, CategoryName="Cat1"}},
                new Product {ProductID=2, ProductName="P4", Category=new Category{CategoryID=1, CategoryName="Cat1"}},
                new Product {ProductID=2, ProductName="P5", Category=new Category{CategoryID=2, CategoryName="Cat2"}}
            }.AsQueryable());

            NavController controller = new NavController(mock.Object);
            string[] result = ((IEnumerable<string>)controller.Menu().Model).ToArray();
            Assert.AreEqual(1, result[0]);
            Assert.AreEqual(2, result[1]);
        }

        [TestMethod]
        public void Menu_SelectedCategoryID_Display()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(p => p.Products).Returns(
                new Product[] {
                   new Product {ProductID=1, ProductName="P1", Category=new Category{CategoryID=1}},
                new Product {ProductID=2, ProductName="P2", Category=new Category{CategoryID=1}},
                new Product {ProductID=2, ProductName="P3", Category=new Category{CategoryID=2}},
                new Product {ProductID=2, ProductName="P4", Category=new Category{CategoryID=1}},
                new Product {ProductID=2, ProductName="P5", Category=new Category{CategoryID=2}}
            }.AsQueryable());

            string selectedCategory = "Cat1";
            NavController controller = new NavController(mock.Object);
            Assert.AreEqual(2, (string)controller.Menu(selectedCategory).ViewBag.SelectedCategoryID);
        }
    }
}
