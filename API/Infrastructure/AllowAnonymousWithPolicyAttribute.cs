using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Threading.Tasks;


    public class AllowAnonymousWithPolicyAttribute : TypeFilterAttribute, IAllowAnonymous
    {
        public AllowAnonymousWithPolicyAttribute(string Policy) : base(typeof(AllowAnonymousWithPolicyFilter))
        {
            Arguments = new object[] { Policy };
        }
    }
    public class AllowAnonymousWithPolicyFilter : IAsyncAuthorizationFilter
    {
        private readonly IAuthorizationService _authorization;
        public string Policy { get; private set; }

        public AllowAnonymousWithPolicyFilter(string policy, IAuthorizationService authorization)
        {
            Policy = policy;
            _authorization = authorization;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var authorized = await _authorization.AuthorizeAsync(context.HttpContext.User, Policy);
            if (!authorized.Succeeded)
            {
                context.Result = new ForbidResult();
                return;
            }
        }
    }

