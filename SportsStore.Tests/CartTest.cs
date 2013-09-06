using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Controllers;

namespace SportsStore.Tests
{
    [TestClass]
    public class CartTest
    {
        [TestMethod]
        public void AddItem_NewItem_Added()
        {
            Product p1 = new Product { ProductName = "P1", ProductID = 1 };
            Product p2 = new Product { ProductName = "P2", ProductID = 2 };
            Cart cart = new Cart();
            cart.AddItem(p1, 2);
            cart.AddItem(p2, 1);
            CartLine[] lineCollection = cart.Lines.ToArray();
            Assert.IsTrue(lineCollection.Length == 2);
            Assert.AreEqual(p1.ProductName, lineCollection[0].Product.ProductName);
            Assert.AreEqual(p2.ProductName, lineCollection[1].Product.ProductName);
        }

        [TestMethod]
        public void AddItem_AddExistingItem_QtyIncreased()
        {
            Product p1 = new Product { ProductName = "P1", ProductID = 1 };
            Cart cart = new Cart();
            cart.AddItem(p1, 2);
            cart.AddItem(p1, 1);
            CartLine[] lineCollection = cart.Lines.ToArray();
            Assert.IsTrue(lineCollection.Length == 1);
            Assert.AreEqual(p1.ProductName, lineCollection[0].Product.ProductName);
            Assert.AreEqual(3, lineCollection[0].Quantity);
        }

        [TestMethod]
        public void RemoveLine_Item_Removed()
        {
            Product p1 = new Product { ProductName = "P1", ProductID = 1 };
            Product p2 = new Product { ProductName = "P2", ProductID = 2 };
            Cart cart = new Cart();
            cart.AddItem(p1, 2);
            cart.AddItem(p2, 1);
            cart.RemoveLine(p1.ProductID);
            CartLine[] lineCollection = cart.Lines.ToArray();
            Assert.IsTrue(lineCollection.Length == 1);
            Assert.AreEqual(p2.ProductName, lineCollection[0].Product.ProductName);
        }

        [TestMethod]
        public void Clear_AllItem_Removed()
        {
            Product p1 = new Product { ProductName = "P1", ProductID = 1 };
            Product p2 = new Product { ProductName = "P2", ProductID = 2 };
            Cart cart = new Cart();
            cart.AddItem(p1, 2);
            cart.AddItem(p2, 1);
            cart.Clear();
            CartLine[] lineCollection = cart.Lines.ToArray();
            Assert.IsTrue(lineCollection.Length == 0);
        }

        [TestMethod]
        public void ComputeTotalValue_TotalPriceInCart_Calculated()
        {
            Product p1 = new Product { ProductName = "P1", ProductID = 1, UnitPrice=10m };
            Product p2 = new Product { ProductName = "P2", ProductID = 2, UnitPrice=20m};
            Cart cart = new Cart();
            cart.AddItem(p1, 2);
            cart.AddItem(p2, 1);
            decimal total = cart.ComputeTotalValue();
            Assert.AreEqual(40m, total);
        }

        [TestMethod]
        public void CartController_AddItem_Added()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(p => p.Products).Returns(new Product[] {
                new Product {ProductName="P1", Category=new Category{CategoryID=1}, ProductID=1, UnitPrice=10m},
                new Product {ProductName="P2", Category=new Category{CategoryID=2}, ProductID=2, UnitPrice=10m}
            }.AsQueryable());
            Cart cart = new Cart();
            CartController controller = new CartController(mock.Object, null);
            var result = controller.AddItem(cart, 1, "/cart/index");
            CartLine [] lines = cart.Lines.ToArray();
            Assert.AreEqual("P1", lines[0].Product.ProductName);
        }

        [TestMethod]
        public void CartContoller_RemoveItem_Added()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(p => p.Products).Returns(new Product[] {
                new Product {ProductName="P1", ProductID=1},
                new Product {ProductName="P2", ProductID=2}
            }.AsQueryable());

            Cart cart = new Cart();
            CartController controller = new CartController(mock.Object, null);
            controller.AddItem(cart, 1, null);
            controller.AddItem(cart, 2, null);
            controller.RemoveItem(cart, 1, null);
            Assert.IsTrue(cart.Lines.Count() == 1);
        }
    }
}
