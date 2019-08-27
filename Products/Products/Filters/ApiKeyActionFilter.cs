using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Products.Filters
{
    public class ApiKeyActionFilter : IActionFilter
    {
        public static readonly string AuthorizationHeaderName = "Authorization";
        public static readonly string AuthorizationApiKeyValue = "ApiKey sample-key"; // key should live in Azure Key Vault or similar (i.e. not in the source code checked into git) and be supplied as an app setting

        public void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var apiKey = filterContext.HttpContext.Request.Headers[AuthorizationHeaderName];
            if(apiKey.Count < 1 || apiKey[0] != AuthorizationApiKeyValue)
            { 
                filterContext.Result = new UnauthorizedResult();
            }
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {

        }
    }
}