using Microsoft.AspNetCore.Mvc;

namespace TGSP.Shared.Extensions
{
    /// <summary>
    /// This class provides extentions for action result
    /// primarily used for test purposes
    /// </summary>
    public static class ActionResultExtensions
    {
        /// <summary>
        /// returns the typed view from an action result
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static T GetView<T>(this ActionResult<T> result) where T : class
        {
            var json = result.Result as JsonResult;
            if(json == null) return null;
            var view = json.Value as T;
            return view;
        }

        /// <summary>
        /// Returns if this is an Unauthorized result
        /// </summary>
        public static bool IsUnauthorizedResult<T>(this ActionResult<T> result) where T : class
        {
            return
                result.Result is UnauthorizedResult ||
                result.Result is UnauthorizedObjectResult ||
                (result.Result is StatusCodeResult && ((StatusCodeResult)result.Result).StatusCode == 401);
        }

        /// <summary>
        /// Returns if this is an Unauthorized result
        /// </summary>
        public static bool IsForbiddenResult<T>(this ActionResult<T> result) where T : class
        {
            return
                result.Result is ForbidResult ||
                (result.Result is StatusCodeResult && ((StatusCodeResult)result.Result).StatusCode == 403);
        }

        /// <summary>
        /// Returns if this is an Unauthorized result
        /// </summary>
        public static bool IsBadRequestResult<T>(this ActionResult<T> result) where T : class
        {
            return
                result.Result is BadRequestResult ||
                result.Result is BadRequestObjectResult ||
                (result.Result is StatusCodeResult && ((StatusCodeResult)result.Result).StatusCode == 400);
        }

    }
}