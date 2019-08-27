using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Primitives;
using Moq;
using NUnit.Framework;
using Products.Filters;

namespace Products.UnitTests
{
    public class ApiKeyActionFilterTests
    {
        private ApiKeyActionFilter _apiKeyActionFilter;
        private ActionExecutingContext _actionExecutingContext;

        [SetUp]
        public void SetUp()
        {
            var httpContext = new DefaultHttpContext();
            _actionExecutingContext = new ActionExecutingContext(
                new ActionContext
                {
                    HttpContext = httpContext,
                    RouteData = new RouteData(),
                    ActionDescriptor = new ActionDescriptor(),
                },
                new List<IFilterMetadata>(),
                new Dictionary<string, object>(),
                new Mock<ControllerBase>().Object);

            _apiKeyActionFilter = new ApiKeyActionFilter();
        }

        [Test]
        public void NoApiKey_ReturnsUnAuthorized()
        {
            _apiKeyActionFilter.OnActionExecuting(_actionExecutingContext);

            Assert.IsInstanceOf<UnauthorizedResult>(_actionExecutingContext.Result);
        }

        [Test]
        public void IncorrectApiKey_ReturnsUnAuthorized()
        {
            _actionExecutingContext.HttpContext.Request.Headers.Add(ApiKeyActionFilter.AuthorizationHeaderName, new StringValues("incorrectkey"));
            _apiKeyActionFilter.OnActionExecuting(_actionExecutingContext);

            Assert.IsInstanceOf<UnauthorizedResult>(_actionExecutingContext.Result);
        }

        [Test]
        public void CorrectApiKey_ReturnsOk()
        {
            _actionExecutingContext.HttpContext.Request.Headers.Add(ApiKeyActionFilter.AuthorizationHeaderName,new StringValues(ApiKeyActionFilter.AuthorizationApiKeyValue));
            _apiKeyActionFilter.OnActionExecuting(_actionExecutingContext);

            Assert.IsNull(_actionExecutingContext.Result);
        }
    }
}
