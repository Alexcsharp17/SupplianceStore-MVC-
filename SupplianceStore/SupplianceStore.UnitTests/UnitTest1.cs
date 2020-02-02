using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using GameStore.WebUI.HtmlHelpers;
using GameStore.WebUI.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SupplianceSrore.WebUI.Controllers;
using SupplianceSrore.WebUI.Models;
using SupplianceStore.Domain.Abstract;
using SupplianceStore.Domain.Entities;

namespace SupplianceStore.UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Can_Paginate()
        {
            // Организация (arrange)
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new List<Product>
    {
                 new Product { ProductId = 1, Name = "Product1"},
                 new Product { ProductId = 2, Name = "Product2"},
                 new Product { ProductId = 3, Name = "Product3"},
                 new Product { ProductId = 4, Name = "Product4"},
                 new Product { ProductId = 5, Name = "Product5"},
    });
            ProductController controller = new ProductController(mock.Object);
            controller.pageSize = 3;

            // Действие (act)
            ProductsListViewModel result = (ProductsListViewModel)controller.List(null,2).Model;

            // Утверждение
            List<Product> games = result.Products.ToList();
            Assert.IsTrue(games.Count == 2);
            Assert.AreEqual(games[0].Name, "Product4");
            Assert.AreEqual(games[1].Name, "Product5");
        }
        [TestMethod]
        public void Can_Generate_Page_Links()
        {

            // Организация - определение вспомогательного метода HTML - это необходимо
            // для применения расширяющего метода
            HtmlHelper myHelper = null;

            // Организация - создание объекта PagingInfo
            PagingInfo pagingInfo = new PagingInfo
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10
            };

            // Организация - настройка делегата с помощью лямбда-выражения
            Func<int, string> pageUrlDelegate = i => "Page" + i;

            // Действие
            MvcHtmlString result = myHelper.PageLinks(pagingInfo, pageUrlDelegate);

            // Утверждение
            Assert.AreEqual(@"<a class=""btn btn-default"" href=""Page1"">1</a>"
                + @"<a class=""btn btn-default btn-primary selected"" href=""Page2"">2</a>"
                + @"<a class=""btn btn-default"" href=""Page3"">3</a>",
                result.ToString());
        }
        [TestMethod]
        public void Can_Send_Pagination_View_Model()
        {
            // Организация (arrange)
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new List<Product>
    {
                 new Product { ProductId = 1, Name = "Product1"},
                 new Product { ProductId = 2, Name = "Product2"},
                 new Product { ProductId = 3, Name = "Product3"},
                 new Product { ProductId = 4, Name = "Product4"},
                 new Product { ProductId = 5, Name = "Product5"},
    });
            ProductController controller = new ProductController(mock.Object);
            controller.pageSize = 3;

            // Act
            ProductsListViewModel result
                = (ProductsListViewModel)controller.List(null,2).Model;

            // Assert
            PagingInfo pageInfo = result.PagingInfo;
            Assert.AreEqual(pageInfo.CurrentPage, 2);
            Assert.AreEqual(pageInfo.ItemsPerPage, 3);
            Assert.AreEqual(pageInfo.TotalItems, 5);
            Assert.AreEqual(pageInfo.TotalPages, 2);
        }
        [TestMethod]
        public void Can_Filter_Products()
        {
            // Организация (arrange)
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new List<Product>
    {
        
                 new Product { ProductId = 1, Name = "Product1", Category="Cat1"},
                 new Product { ProductId = 2, Name = "Product2", Category="Cat2"},
                 new Product { ProductId = 3, Name = "Product3", Category="Cat1"},
                 new Product { ProductId = 4, Name = "Product4", Category="Cat2"},
                 new Product { ProductId = 5, Name = "Product5", Category="Cat3"},

    });
            ProductController controller = new ProductController(mock.Object);
            controller.pageSize = 3;

            // Action
            List<Product> result = ((ProductsListViewModel)controller.List("Cat2", 1).Model)
                .Products.ToList();

            // Assert
            Assert.AreEqual(result.Count(), 2);
            Assert.IsTrue(result[0].Name == "Product2" && result[0].Category == "Cat2");
            Assert.IsTrue(result[1].Name == "Product4" && result[1].Category == "Cat2");
        }
        [TestMethod]
        public void Can_Create_Categories()
        {
            // Организация - создание имитированного хранилища
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new List<Product> {
        
                 new Product { ProductId = 1, Name = "Product1", Category="Cat1"},
                 new Product { ProductId = 2, Name = "Product2", Category="Cat2"},
                 new Product { ProductId = 3, Name = "Product3", Category="Cat1"},
                 new Product { ProductId = 4, Name = "Product4", Category="Cat1"},
                 new Product { ProductId = 5, Name = "Product5", Category="Cat3"},
    });

            // Организация - создание контроллера
            NavController target = new NavController(mock.Object);

            // Действие - получение набора категорий
            List<string> results = ((IEnumerable<string>)target.Menu().Model).ToList();

            // Утверждение
            Assert.AreEqual(results.Count(), 3);
            Assert.AreEqual(results[0], "Cat1");
            Assert.AreEqual(results[1], "Cat2");
            Assert.AreEqual(results[2], "Cat3");
        }
    }
}