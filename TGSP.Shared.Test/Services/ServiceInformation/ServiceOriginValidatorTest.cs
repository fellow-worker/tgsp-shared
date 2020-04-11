using Microsoft.VisualStudio.TestTools.UnitTesting;
using TGSP.Shared.Services.ServiceInformation;
using Moq;
using System.Collections.Generic;

namespace TGSP.Shared.Test.Services.ServiceInformation
{
    [TestClass]
    public class ServiceOriginValidatorTest
    {
        [TestMethod]
        public void IsAllowedOriginEmptyTest()
        {
            var mock = new Mock<IServiceInformationProvider>();
            mock.Setup(v => v.GetServices()).Returns(default(List<Service>));

            var validator = new ServiceOriginValidator(null);
            Assert.IsFalse(validator.IsAllowedOrigin(null));
            Assert.IsFalse(validator.IsAllowedOrigin(string.Empty));

            validator = new ServiceOriginValidator(null);
            Assert.IsFalse(validator.IsAllowedOrigin("http://example.com"));

            validator = new ServiceOriginValidator(mock.Object);
            Assert.IsFalse(validator.IsAllowedOrigin("http://example.com"));

            mock = new Mock<IServiceInformationProvider>();
            mock.Setup(v => v.GetServices()).Returns(new List<Service>());
            Assert.IsFalse(validator.IsAllowedOrigin("http://example.com"));
        }

        [TestMethod]
        public void IsAllowedOriginUnAllowedTest()
        {
            var service = new Service { Origin = "http://example.com", IsAllowedOrigin = false };

            var mock = new Mock<IServiceInformationProvider>();
            mock.Setup(v => v.GetServices()).Returns(new List<Service>() { service });

            var validator = new ServiceOriginValidator(mock.Object);
            Assert.IsFalse(validator.IsAllowedOrigin("http://example.net"));
            Assert.IsFalse(validator.IsAllowedOrigin("http://example.com"));
        }

        [TestMethod]
        public void IsAllowedOriginAllowedTest()
        {
            var service = new Service { Origin = "http://example.com", IsAllowedOrigin = true };

            var mock = new Mock<IServiceInformationProvider>();
            mock.Setup(v => v.GetServices()).Returns(new List<Service>() { service });

            var validator = new ServiceOriginValidator(mock.Object);
            Assert.IsTrue(validator.IsAllowedOrigin("http://example.com"));
        }
    }
}