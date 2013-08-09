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
            Product p1 = new Product { Name = "P1", ProductId = 1 };
            Product p2 = new Product { Name = "P2", ProductId = 2 };
            Cart cart = new Cart();
            cart.AddItem(p1, 2);
            cart.AddItem(p2, 1);
            CartLine[] lineCollection = cart.Lines.ToArray();
            Assert.IsTrue(lineCollection.Length == 2);
            Assert.AreEqual(p1.Name, lineCollection[0].Product.Name);
            Assert.AreEqual(p2.Name, lineCollection[1].Product.Name);
        }

        [TestMethod]
        public void AddItem_AddExistingItem_QtyIncreased()
        {
            Product p1 = new Product { Name = "P1", ProductId = 1 };
            Cart cart = new Cart();
            cart.AddItem(p1, 2);
            cart.AddItem(p1, 1);
            CartLine[] lineCollection = cart.Lines.ToArray();
            Assert.IsTrue(lineCollection.Length == 1);
            Assert.AreEqual(p1.Name, lineCollection[0].Product.Name);
            Assert.AreEqual(3, lineCollection[0].Quantity);
        }

        [TestMethod]
        public void RemoveLine_Item_Removed()
        {
            Product p1 = new Product { Name = "P1", ProductId = 1 };
            Product p2 = new Product { Name = "P2", ProductId = 2 };
            Cart cart = new Cart();
            cart.AddItem(p1, 2);
            cart.AddItem(p2, 1);
            cart.RemoveLine(p1.ProductId);
            CartLine[] lineCollection = cart.Lines.ToArray();
            Assert.IsTrue(lineCollection.Length == 1);
            Assert.AreEqual(p2.Name, lineCollection[0].Product.Name);
        }

        [TestMethod]
        public void Clear_AllItem_Removed()
        {
            Product p1 = new Product { Name = "P1", ProductId = 1 };
            Product p2 = new Product { Name = "P2", ProductId = 2 };
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
            Product p1 = new Product { Name = "P1", ProductId = 1, Price=10m };
            Product p2 = new Product { Name = "P2", ProductId = 2, Price=20m};
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
                new Product {Name="P1", Category="Cat1", ProductId=1, Price=10m},
                new Product {Name="P2", Category="Cat2", ProductId=2, Price=10m}
            }.AsQueryable());
            Cart cart = new Cart();
            CartController controller = new CartController(mock.Object, null);
            var result = controller.AddItem(cart, 1, "/cart/index");
            CartLine [] lines = cart.Lines.ToArray();
            Assert.AreEqual("P1", lines[0].Product.Name);
        }

        [TestMethod]
        public void CartContoller_RemoveItem_Added()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(p => p.Products).Returns(new Product[] {
                new Product {Name="P1", ProductId=1},
                new Product {Name="P2", ProductId=2}
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
