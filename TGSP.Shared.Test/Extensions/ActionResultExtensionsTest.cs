using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TGSP.Shared.Extensions;

namespace TGSP.Shared.Test.Extensions
{
    [TestClass]
    public class ActionResultExtensionsTest
    {
        [TestMethod]
        public void GetViewTest()
        {
            var model = new Model();
            var jsonResult = new JsonResult(model);
            var result = new ActionResult<Model>(jsonResult);

            var view = result.GetView<Model>();
            Assert.IsNotNull(view);
        }

        [TestMethod]
        public void GetViewNullTest()
        {
            var unauthorized = new UnauthorizedResult();
            var result = new ActionResult<Model>(unauthorized);

            var view = result.GetView<Model>();
            Assert.IsNull(view);
        }

        [TestMethod]
        public void IsUnauthorizedResultTest()
        {
            var result = new ActionResult<Model>(new UnauthorizedResult());
            Assert.IsTrue(result.IsUnauthorizedResult());

            result = new ActionResult<Model>(new UnauthorizedObjectResult(result));
            Assert.IsTrue(result.IsUnauthorizedResult());

            result = new ActionResult<Model>(new StatusCodeResult(200));
            Assert.IsFalse(result.IsUnauthorizedResult());

            result = new ActionResult<Model>(new StatusCodeResult(401));
            Assert.IsTrue(result.IsUnauthorizedResult());
        }

        [TestMethod]
        public void IsForbiddenResultTest()
        {
            var result = new ActionResult<Model>(new ForbidResult());
            Assert.IsTrue(result.IsForbiddenResult());

            result = new ActionResult<Model>(new StatusCodeResult(401));
            Assert.IsFalse(result.IsForbiddenResult());

            result = new ActionResult<Model>(new StatusCodeResult(403));
            Assert.IsTrue(result.IsForbiddenResult());
        }

        [TestMethod]
        public void IsBadRequestResultTest()
        {
            var result = new ActionResult<Model>(new BadRequestResult());
            Assert.IsTrue(result.IsBadRequestResult());

            result = new ActionResult<Model>(new BadRequestObjectResult(result));
            Assert.IsTrue(result.IsBadRequestResult());

            result = new ActionResult<Model>(new StatusCodeResult(200));
            Assert.IsFalse(result.IsBadRequestResult());

            result = new ActionResult<Model>(new StatusCodeResult(400));
            Assert.IsTrue(result.IsBadRequestResult());
        }
    }
}