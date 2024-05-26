using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json.Linq;

namespace EmployeeClientProject.Filters
{
    public class SessionRequirement : IAuthorizationRequirement
    {
        public SessionRequirement(string sessionHeaderName)
        {
            SessionHeaderName = sessionHeaderName;
        }

        public string SessionHeaderName { get; }
    }
    public class SessionHandler : AuthorizationHandler<SessionRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SessionHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override Task HandleRequirementAsync
        (AuthorizationHandlerContext context, SessionRequirement requirement)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext != null)
            {
                var RoleName = httpContext.Session.GetString("RoleName");
                if (RoleName != "Admin")
                {
                    if (RoleName != requirement.SessionHeaderName)
                    {
                        // You have successfully retrieved the value from the session
                        context.Fail();
                        httpContext.Session.SetString("AUTHORIZATIONSTATUS", "0");
                        return Task.CompletedTask;
                    }
                }
            }
            // If the session value is not found or if HttpContext is null, fail the authorization check
            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }

}
