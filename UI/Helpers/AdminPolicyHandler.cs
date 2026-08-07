using Microsoft.AspNetCore.Authorization;

namespace UI.Helpers
{
    public class AdminPolicyHandler : AuthorizationHandler<AdminPolicyRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AdminPolicyHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AdminPolicyRequirement requirement)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
            {
                context.Fail();
                return Task.CompletedTask;
            }

            // استخراج اسم الـ Controller و الـ Action من الـ Route
            var controller = httpContext.Request.RouteValues["controller"]?.ToString();
            var action = httpContext.Request.RouteValues["action"]?.ToString();

            if (string.IsNullOrEmpty(controller) || string.IsNullOrEmpty(action))
            {
                context.Fail();
                return Task.CompletedTask;
            }

            // جلب الأدوار المسموحة من المصفوفة
            var allowedRoles = PermissionMatrix.GetRoles(controller, action);
            if (allowedRoles == null)
            {
                // إذا لم يكن هناك تعريف لهذا الـ Action، نسمح بالوصول (أو نرفض حسب رغبتك)
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            // تقسيم الأدوار والتحقق من أن المستخدم يمتلك واحدة منها على الأقل
            var user = context.User;
            var roles = allowedRoles.Split(',', StringSplitOptions.RemoveEmptyEntries);
            if (roles.Any(role => user.IsInRole(role.Trim())))
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }

            return Task.CompletedTask;
        }
    }

    public class AdminPolicyRequirement : IAuthorizationRequirement { }
}
