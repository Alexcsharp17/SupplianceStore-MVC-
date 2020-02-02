﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Collections.Generic;
using SupplianceStore.Domain.Entities;
using SupplianceStore.Domain.Abstract;
using Moq;
using GameStore.WebUI.Controllers;
using System.Web.Mvc;
using SupplianceSrore.WebUI.Models;
using SupplianceStore.WebUI.Controllers;

namespace GameStore.UnitTests
{
    [TestClass]
    public class CartTests
    {
        [TestMethod]
        public void Can_Add_New_Lines()
        {
            // Организация - создание нескольких тестовых products
            Product product1 = new Product { ProductId = 1, Name = "Product1" };
            Product product2 = new Product { ProductId = 2, Name = "Product2" };

            // Организация - создание корзины
            Cart cart = new Cart();

            // Действие
            cart.AddItem(product1, 1);
            cart.AddItem(product2, 1);
            List<CartLine> results = cart.Lines.ToList();

            // Утверждение
            Assert.AreEqual(results.Count(), 2);
            Assert.AreEqual(results[0].Product, product1);
            Assert.AreEqual(results[1].Product, product2);
        }
        [TestMethod]
        public void Can_Add_Quantity_For_Existing_Lines()
        {
            // Организация - создание нескольких тестовых products
            Product product1 = new Product { ProductId = 1, Name = "Product1" };
            Product product2 = new Product { ProductId = 2, Name = "Product2" };

            // Организация - создание корзины
            Cart cart = new Cart();

            // Действие
            cart.AddItem(product1, 1);
            cart.AddItem(product2, 1);
            cart.AddItem(product1, 5);
            List<CartLine> results = cart.Lines.OrderBy(c => c.Product.ProductId).ToList();

            // Утверждение
            Assert.AreEqual(results.Count(), 2);
            Assert.AreEqual(results[0].Quantity, 6);    // 6 экземпляров добавлено в корзину
            Assert.AreEqual(results[1].Quantity, 1);
        }
        [TestMethod]
        public void Can_Remove_Line()
        {
            // Организация - создание нескольких тестовых products
            Product product1 = new Product { ProductId = 1, Name = "Product1" };
            Product product2 = new Product { ProductId = 2, Name = "Product2" };
            Product product3 = new Product { ProductId = 3, Name = "Product3" };
           

            // Организация - создание корзины
            Cart cart = new Cart();

            // Организация - добавление нескольких products в корзину
            cart.AddItem(product1, 1);
            cart.AddItem(product2, 4);
            cart.AddItem(product3, 2);
            cart.AddItem(product2, 1);

            // Действие
            cart.RemoveLine(product2);

            // Утверждение
            Assert.AreEqual(cart.Lines.Where(c => c.Product == product2).Count(), 0);
            Assert.AreEqual(cart.Lines.Count(), 2);
        }
        [TestMethod]
        public void Calculate_Cart_Total()
        {
            // Организация - создание нескольких тестовых products
    
            Product product1 = new Product { ProductId = 1, Name = "Product1", Price = 100 };
            Product product2 = new Product { ProductId = 2, Name = "Product2", Price = 55 };

            // Организация - создание корзины
            Cart cart = new Cart();

            // Действие
            cart.AddItem(product1, 1);
            cart.AddItem(product2, 1);
            cart.AddItem(product1, 5);
            decimal result = cart.ComputeTotalValue();

            // Утверждение
            Assert.AreEqual(result, 655);
        }
        [TestMethod]
        public void Can_Clear_Contents()
        {
            // Организация - создание нескольких тестовых игр
          
            Product product1 = new Product { ProductId = 1, Name = "Product1", Price = 100 };
            Product product2 = new Product { ProductId = 2, Name = "Product2", Price = 55 };


            // Организация - создание корзины
            Cart cart = new Cart();

            // Действие
            cart.AddItem(product1, 1);
            cart.AddItem(product2, 1);
            cart.AddItem(product1, 5);
            cart.Clear();

            // Утверждение
            Assert.AreEqual(cart.Lines.Count(), 0);
        }
        /// <summary>
        /// Проверяем добавление в корзину
        /// </summary>
        [TestMethod]
        public void Can_Add_To_Cart()
        {
            // Организация - создание имитированного хранилища
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new List<Product> {
        new Product {ProductId = 1, Name = "Игра1", Category = "Кат1"},
    }.AsQueryable());

            // Организация - создание корзины
            Cart cart = new Cart();

            // Организация - создание контроллера
            CartController controller = new CartController(mock.Object,null);

            // Действие - добавить игру в корзину
            controller.AddToCart(cart, 1, null);

            // Утверждение
            Assert.AreEqual(cart.Lines.Count(), 1);
            Assert.AreEqual(cart.Lines.ToList()[0].Product.ProductId, 1);
        }

        /// <summary>
        /// После добавления игры в корзину, должно быть перенаправление на страницу корзины
        /// </summary>
        [TestMethod]
        public void Adding_Game_To_Cart_Goes_To_Cart_Screen()
        {
            // Организация - создание имитированного хранилища
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new List<Product> {
        new Product {ProductId = 1, Name = "Product1", Category = "Кат1"},
    }.AsQueryable());

            // Организация - создание корзины
            Cart cart = new Cart();

            // Организация - создание контроллера
            CartController controller = new CartController(mock.Object,null);

            // Действие - добавить игру в корзину
            RedirectToRouteResult result = controller.AddToCart(cart, 2, "myUrl");

            // Утверждение
            Assert.AreEqual(result.RouteValues["action"], "Index");
            Assert.AreEqual(result.RouteValues["returnUrl"], "myUrl");
        }

        // Проверяем URL
        [TestMethod]
        public void Can_View_Cart_Contents()
        {
            // Организация - создание корзины
            Cart cart = new Cart();

            // Организация - создание контроллера
            CartController target = new CartController(null,null);

            // Действие - вызов метода действия Index()
            CartIndexViewModel result
                = (CartIndexViewModel)target.Index(cart, "myUrl").ViewData.Model;

            // Утверждение
            Assert.AreSame(result.Cart, cart);
            Assert.AreEqual(result.ReturnUrl, "myUrl");
        }
        [TestMethod]
        public void Cannot_Checkout_Empty_Cart()
        {
            // Организация - создание имитированного обработчика заказов
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();

            // Организация - создание пустой корзины
            Cart cart = new Cart();

            // Организация - создание деталей о доставке
            ShippingDetails shippingDetails = new ShippingDetails();

            // Организация - создание контроллера
            CartController controller = new CartController(null, mock.Object);

            // Действие
            ViewResult result = controller.Checkout(cart, shippingDetails);

            // Утверждение — проверка, что заказ не был передан обработчику 
            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()),
                Times.Never());

            // Утверждение — проверка, что метод вернул стандартное представление 
            Assert.AreEqual("", result.ViewName);

            // Утверждение - проверка, что-представлению передана неверная модель
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }
        [TestMethod]
        public void Cannot_Checkout_Invalid_ShippingDetails()
        {
            // Организация - создание имитированного обработчика заказов
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();

            // Организация — создание корзины с элементом
            Cart cart = new Cart();
            cart.AddItem(new Product(), 1);

            // Организация — создание контроллера
            CartController controller = new CartController(null, mock.Object);

            // Организация — добавление ошибки в модель
            controller.ModelState.AddModelError("error", "error");

            // Действие - попытка перехода к оплате
            ViewResult result = controller.Checkout(cart, new ShippingDetails());

            // Утверждение - проверка, что заказ не передается обработчику
            mock.Verify(m => m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()),
                Times.Never());

            // Утверждение - проверка, что метод вернул стандартное представление
            Assert.AreEqual("", result.ViewName);

            // Утверждение - проверка, что-представлению передана неверная модель
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }
        [TestMethod]
        public void Can_Delete_Valid_Products()
        {
            // Организация - создание объекта Game
            Product product = new Product { ProductId = 2, Name = "Игра2" };

            // Организация - создание имитированного хранилища данных
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new List<Product>
    {
        new Product { ProductId = 1, Name = "Product1"},
        new Product { ProductId = 2, Name = "Product1"},
        new Product { ProductId = 3, Name = "Product1"},
        new Product { ProductId = 4, Name = "Product1"},
        new Product { ProductId = 5, Name = "Product1"},
    });

            // Организация - создание контроллера
            AdminController controller = new AdminController(mock.Object);

            // Действие - удаление игры
            controller.Delete(product.ProductId);

            // Утверждение - проверка того, что метод удаления в хранилище
            // вызывается для корректного объекта Game
            mock.Verify(m => m.DeleteProduct(product.ProductId));
        }
    }
}