using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Discounts;
using Discounts.Controllers;

namespace Discounts.Tests.Controllers
{
    [TestClass]
    public class ValuesControllerTest
    {
        [TestMethod]
        public void Get()
        {
            // Arrange
            Discounts.Controllers.ApiController controller = new Discounts.Controllers.ApiController();

            // Act
            IEnumerable<string> result = controller.Get();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual("value1", result.ElementAt(0));
            Assert.AreEqual("value2", result.ElementAt(1));
        }

        [TestMethod]
        public void GetById()
        {
            // Arrange
            Discounts.Controllers.ApiController controller = new Discounts.Controllers.ApiController();

            // Act
            string result = controller.Get(5);

            // Assert
            Assert.AreEqual("value", result);
        }

        [TestMethod]
        public void Post()
        {
            // Arrange
            Discounts.Controllers.ApiController controller = new Discounts.Controllers.ApiController();

            // Act
            controller.Post("value");

            // Assert
        }

        [TestMethod]
        public void Put()
        {
            // Arrange
            Discounts.Controllers.ApiController controller = new Discounts.Controllers.ApiController();

            // Act
            controller.Put(5, "value");

            // Assert
        }

        [TestMethod]
        public void Delete()
        {
            // Arrange
            Discounts.Controllers.ApiController controller = new Discounts.Controllers.ApiController();

            // Act
            controller.Delete(5);

            // Assert
        }
    }
}
