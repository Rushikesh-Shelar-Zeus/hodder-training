

using HotChocolate.Types.Descriptors;

namespace ContosoPizzaNoSQl.Middleware;

public class AuthMiddleware : ObjectFieldDescriptorAttribute
{
    private readonly string? _requiredRole;
    public AuthMiddleware(string? role = null)
    {
        _requiredRole = role;
    }

    protected override void OnConfigure(IDescriptorContext context, IObjectFieldDescriptor descriptor, System.Reflection.MemberInfo member)
    {
        descriptor.Use(next => async ctx =>
        {
            var httpContext = ctx.Service<HttpContext>();

            var userId = httpContext.Items["UserId"]?.ToString();
            var userRole = httpContext.Items["UserRole"]?.ToString();

            if (string.IsNullOrEmpty(userId))
            {
                ctx.ReportError("Authentication required.");
                return;
            }

            if (_requiredRole != null && !string.Equals(userRole, _requiredRole, StringComparison.OrdinalIgnoreCase))
            {
                ctx.ReportError("You do not have permission to access this resource.");
                return;
            }

            await next(ctx);
        });
    }
}