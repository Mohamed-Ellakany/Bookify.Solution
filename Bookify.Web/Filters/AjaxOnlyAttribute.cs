using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ActionConstraints;

namespace Bookify.Web.Filters
{
    public class AjaxOnlyAttribute : ActionMethodSelectorAttribute
    {
        public override bool IsValidForRequest(RouteContext routeContext, ActionDescriptor action)
        {
            var reqest = routeContext.HttpContext.Request;
            var isAjax = reqest.Headers["X-Requested-With"] == "XMLHttpRequest";
            return isAjax;

        }
    }
}
